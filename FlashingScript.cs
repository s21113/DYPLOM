using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FlashingScript : MonoBehaviour
{

    private Image image;
    Color colorbackup;
    bool running;
    private Player energyBar;
    public GameObject follow;

    // Start is called before the first frame update
    void Start()
    {
        follow = GameObject.FindGameObjectWithTag("PlayerBody");
        image = GetComponent<Image>();
        double difficoultyLvl = follow.GetComponentInChildren<EnergyBar>().GetHealthLevel();
        //energyBar = this.gameObject.GetComponent<EnergyBar>();
        Color color = image.color;
        Color colorbackup = image.color;
        running = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("h") && !running)
        {
            SetRedScreen();
        }
        else if (Input.GetKeyDown("j"))
        {
            // energyBar.updateHealthBarUP();
        }
    }
    public void SetRedScreen()
    {
        if ((follow.GetComponentInChildren<EnergyBar>().GetHealthLevel() > 1) && !running)
        {
            running = true;
            StartCoroutine("Pulse");
        }else if (follow.GetComponentInChildren<EnergyBar>().GetHealthLevel() <= 1)
        {
            StartCoroutine("PulseBlack");
        }
        
    }

    IEnumerator Pulse()
    {
       
        follow.GetComponentInChildren<EnergyBar>().DecreaseHealth();
        Color color = image.color;
        int counter = 0;
        while (counter < 2)
        {
            for (float i = 0; i <= 0.3; i += 0.01f)
            {
                image.color = new Color(color.r, color.g, color.b, i);
                yield return null;
            }

            for (float i = 0.5f; i >= 0; i -= 0.01f)
            {
                image.color = new Color(color.r, color.g, color.b, i);
                yield return null;
            }
            counter++;
        }
        running = false;
        image.color = new Color(color.r, color.g, color.b, 0);
        StopCoroutine("Pulse");
    }

    IEnumerator PulseBlack()
    {
        Color color = image.color;
        for (float i = 0; i <= 1; i += 0.04f)
        {
            image.color = new Color(0, 0, 0, i);
            yield return new WaitForSeconds(.02f);
        }
        image.color = new Color(0, 0, 0, 3f);

        yield return new WaitForSeconds(1);


        SceneManager.LoadScene("EndGameScreen", LoadSceneMode.Single);

        StopCoroutine("PulseBlack");



    }
}
