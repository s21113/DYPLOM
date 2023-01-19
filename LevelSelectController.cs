using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectController : MonoBehaviour
{
    public void BackToMenu()
    {
        SceneManager.LoadScene("main_menu", LoadSceneMode.Single);
    }
    public void GoToLevelOne()
    {
        SceneManager.LoadScene("Real_Level_1", LoadSceneMode.Single);
    }
    public void GoToLevelTwo()
    {
        SceneManager.LoadScene("Real_Level_2", LoadSceneMode.Single);
    }
    public void GoToLevelThree()
    {

    }
    public void GoToLevelFour()
    {

    }
    public void GoToLevelFive()
    {

    }
    private void GoToWorldMap()
    {

    }
}
