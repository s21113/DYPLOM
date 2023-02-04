using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class endgamescript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        StartCoroutine("PulseBlack");
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator PulseBlack()
    {
        var correct = GetComponent<Image>().color; ;
        correct.a = 0;
        GetComponent<Image>().color = correct;
        GetComponent<Image>().CrossFadeAlpha(1f, 5f, true);
        yield return new WaitForSeconds(10f);
        SceneManager.LoadScene("main_menu", LoadSceneMode.Single);
        StopCoroutine("PulseBlack");



    }
}
