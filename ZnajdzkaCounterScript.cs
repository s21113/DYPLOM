using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ZnajdzkaCounterScript : MonoBehaviour
{

    public TextMeshPro ZnajdzkaCounter;
    private GameObject eqSystem;
    void Start()
    {
        eqSystem = GameObject.Find("EqSystem"); // jebne za to

    }

    // Update is called once per frame
    void Update()
    {
        int points = eqSystem.GetComponent<EqSystem>().GetImportantPoints();
        ZnajdzkaCounter.text = points + "";
        ZnajdzkaCounter.color = points == 7 ? Color.green : Color.red;
    }
}
