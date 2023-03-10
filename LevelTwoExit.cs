using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Tego kondoma nakładamy na obiekt który będzie służył za wyjście z poziomu drugiego.
/// Zakłada się, że jeśli gracz wejdzie w collider, to znaczy że zebrał wszystko.
/// </summary>
public class LevelTwoExit : MonoBehaviour
{
    public Animator carAppear;
    public GameObject carLights;

    private EqSystem player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerBody").GetComponentInChildren<EqSystem>();
    }


    private void Update()
    {
        if (player == null) return;
        if (player.GetImportantPoints() == 7)
            carLights.SetActive(true);
    }


    public void ProcessLevelExit(GameObject player)
    {
        var playerHandler = player.GetComponent<BetterPlayerMovement>();
        if (playerHandler == null) return;
        playerHandler.isLeaving = true;

        // zablokowanie ruchu
        float currScore = PlayerPrefs.GetFloat("TotalScore", 0.0f);
        PlayerPrefs.SetFloat("Level1Score", currScore + playerHandler._inventory.GetComponent<EqSystem>().GetImportantPoints());

        SceneManager.LoadScene("Main_Menu", LoadSceneMode.Single);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerBody"))
        {
            var eq = other.GetComponentInChildren<EqSystem>();
            if (eq == null || eq.GetImportantPoints() != 7) return;
            ProcessLevelExit(other.gameObject);
        }
    }
}
