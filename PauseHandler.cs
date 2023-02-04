using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseHandler : MonoBehaviour
{
    public static PauseHandler instance;
    public bool inSomeMenu = false;
    public bool paused = false;
    private bool inSettingsMenu = false;
    public GameObject pauseContainer;
    private Scene settingsScene;
    private GameObject backButton;

    public void PauseResume()
    {
        paused = !paused;
        pauseContainer.SetActive(paused);
        AudioListener.pause = paused;
        Time.timeScale = paused ? 0 : 1;
    }

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        SceneManager.LoadScene("Settings Scene", LoadSceneMode.Additive);
        settingsScene = SceneManager.GetSceneByName("Settings Scene");
        SceneManager.sceneLoaded += UnpauseOnSceneLoad;
    }

    void Start()
    {
        paused = false;
        pauseContainer.SetActive(false);

        GameObject canv = null;
        foreach (var obj in settingsScene.GetRootGameObjects())
        {
            if (obj.name.Contains("Canvas"))
            {
                canv = obj;
                break;
            }
        }
        for (int i = 0; i < canv.transform.childCount; i++)
        {
            GameObject obj = canv.transform.GetChild(i).gameObject;
            if (obj.name.Contains("Back"))
            {
                backButton = obj;
                break;
            }
        }
        backButton.GetComponent<Button>().onClick.AddListener(ExitSettings);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (inSomeMenu) return;
            if (inSettingsMenu)
                ExitSettings();
            else
                PauseResume();
        }
        if (paused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = paused;
        }
        else if (inSomeMenu)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = inSomeMenu;
        }
    }

    private void ToggleSettingsMenu(bool val)
    {
        if (!paused) return;
        pauseContainer.SetActive(!val);
        foreach (var obj in settingsScene.GetRootGameObjects())
        {
            if (obj.name.Contains("Canvas")) obj.SetActive(val);
        }
        inSettingsMenu = val;
    }

    private void ExitSettings()
    {
        GameSettings.instance.SaveSettingsToFile();
        ToggleSettingsMenu(false);
    }

    public void ExitToMenu()
    {
        if (!paused) return;
        SceneManager.LoadScene("main_menu", LoadSceneMode.Single);
    }

    public void OpenSettingsMenu()
    {
        if (!paused) return;
        SceneManager.SetActiveScene(settingsScene);
        ToggleSettingsMenu(true);
    }

    private void UnpauseOnSceneLoad(Scene arg0, LoadSceneMode arg1)
    {
        paused = false;
    }
}
