using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class deathScript : MonoBehaviour
{

    private Image image;
    //Color colorbackup;
    //bool running;

    void Start()
    {

        image = GetComponent<Image>();
        Color color = image.color;
        //Color colorbackup = image.color;
        //running = false;

    }

    void Update()
    {
        
    }

    IEnumerator Pulse()
    {
            Color color = image.color;
            for (float i = 0; i <= 1; i += 0.05f)
            {
                image.color = new Color(color.r, color.g, color.b, i);
                yield return null;
            }

        StopCoroutine("Pulse");
    }
}
