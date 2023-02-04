using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.Audio;

public class MainMenuController : MonoBehaviour
{
    public Camera camToRot;
    public GameObject buttonsContainer;
    public GameObject tutorialContainer;
    public Image blackFade;

    public AudioMixer mixer;

    private readonly float fadeOut = 3f;
    private GameObject backButton;
    private bool inSettingsMenu = false;
    private Scene settingsScene;

    private void ReloadScene()
    {
        new List<Button>(buttonsContainer.GetComponentsInChildren<Button>())
            .ForEach(btn => btn.interactable = true);
        AudioListener.pause = false;
        Time.timeScale = 1f;
        mixer.SetFloat("MainMenuVolume", 1);
        inSettingsMenu = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        buttonsContainer.SetActive(true);
        tutorialContainer.SetActive(false);
        if (PauseHandler.instance != null) PauseHandler.instance.paused = false;
    }

    private void Awake()
    {
        blackFade.gameObject.SetActive(false);
        buttonsContainer.SetActive(true);
        tutorialContainer.SetActive(false);
        SceneManager.LoadScene("Settings Scene", LoadSceneMode.Additive);
        settingsScene = SceneManager.GetSceneByName("Settings Scene");
    }
    private void Start()
    {
        ReloadScene();
        GameObject canv = new List<GameObject>(settingsScene.GetRootGameObjects())
            .FirstOrDefault(obj => obj.name.Contains("Canvas"));
        for (int i = 0; i < canv.transform.childCount; i++) // Okazuje się, że nie ma metody GetChildren()
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

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (inSettingsMenu)
                ExitSettings();
        }
        SpinTheCamera();
        HandleTutorial();
    }

    private void SpinTheCamera()
    {
        if (tutorialContainer.activeInHierarchy) return;
        camToRot.transform.Rotate(0, 0.11f * Time.timeScale, 0);
    }

    IEnumerator FadeAudioOut()
    {
        // JLF
        float time = 0;
        float vol;
        mixer.GetFloat("MainMenuVolume", out vol);
        vol = Mathf.Pow(10, vol / 20);
        float target = Mathf.Clamp(0, 0.0001f, 1);
        while (time < fadeOut)
        {
            time += Time.deltaTime;
            float vol2 = Mathf.Lerp(vol, target, time / fadeOut);
            mixer.SetFloat("MainMenuVolume", Mathf.Log10(vol2) * 20);
            yield return null;
        }
        yield break;
    }

    IEnumerator Transitions()
    {
        StartCoroutine(FadeAudioOut());
        yield return new WaitForSeconds(fadeOut);
        buttonsContainer.SetActive(false);
        tutorialContainer.SetActive(true);
        yield break;
    }

    public void StartTheGameFFS()
    {
        blackFade.gameObject.SetActive(true);
        blackFade.canvasRenderer.SetAlpha(0f);
        new List<Button>(buttonsContainer.GetComponentsInChildren<Button>())
            .ForEach(btn => btn.interactable = false); // .ForEach wygląda ładniej niż iterowanie po tablicy

        blackFade.CrossFadeAlpha(1f, fadeOut, true);
        StartCoroutine(Transitions());
    }

    private void HandleTutorial()
    {
        if (!tutorialContainer.activeInHierarchy) return;
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            SceneManager.LoadScene("Real_Level_1", LoadSceneMode.Single);
        }
    }



    public void GoToSettings()
    {
        buttonsContainer.SetActive(false);
        SceneManager.SetActiveScene(settingsScene);
        foreach (var obj in settingsScene.GetRootGameObjects())
        {
            if (obj.name.Contains("Canvas")) obj.SetActive(true);
        }
        inSettingsMenu = true;
    }

    public void ExitSettings()
    {
        GameSettings.instance.SaveSettingsToFile();
        foreach (var obj in settingsScene.GetRootGameObjects())
        {
            if (obj.name.Contains("Canvas")) obj.SetActive(false);
        }
        buttonsContainer.SetActive(true);
        inSettingsMenu = false;
    }

    public void GoToLevelSelection()
    {
        SceneManager.LoadScene("Level Select", LoadSceneMode.Single);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
