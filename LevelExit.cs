using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Tego kondoma nakładamy na obiekt który będzie służył za wyjście z poziomu.
/// Zakłada się, że jeśli gracz wejdzie w collider, to znaczy że zebrał wszystko.
/// </summary>
public class LevelExit : MonoBehaviour
{
    public void ProcessLevelExit(GameObject player)
    {
        var playerHandler = player.GetComponent<BetterPlayerMovement>();
        if (playerHandler == null) return;
        playerHandler.isLeaving = true;

        // zablokowanie ruchu
        float currScore = PlayerPrefs.GetFloat("TotalScore", 0.0f);
        PlayerPrefs.SetFloat("Level1Score", currScore + playerHandler._inventory.GetComponent<EqSystem>().GetImportantPoints());

        SceneManager.LoadScene("Real_Level_2", LoadSceneMode.Single);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerBody"))
        {
            var eq = other.GetComponentInChildren<EqSystem>();
            Debug.Log(eq == null);
            //if (eq == null || eq.GetImportantPoints() != 7) return;
            // powtórne wystąpienie powyższego checka jest celowe
            // na wypadek gdyby ktoś wyszedł poza szkołę i przebiegł do collidera
            ProcessLevelExit(other.gameObject);
        }
    }
}
