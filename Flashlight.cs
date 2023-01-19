using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public static Flashlight instance;
    private Vector3 vectorOffset;
    public GameObject follow;
    Light lightItself;
    public float speed = 3.0f;
    public AudioSource clickSound;
    public int BatteryCharge;
    private double energyMax;
    private string difficoultyLvl;
    private int dischargeRate;

    public void Toggle()
    {
        lightItself.enabled = !lightItself.enabled;
        clickSound.Play();

        if (lightItself.enabled)
        {
            StartCoroutine("Discharge");
        }
        else
        {
            StopCoroutine("Discharge");
        }
    }

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }


    void Start()
    {
        lightItself = GetComponent<Light>();
        follow = GameObject.FindGameObjectWithTag("PlayerBody");
        vectorOffset = transform.position - follow.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = follow.transform.position + vectorOffset;
        //transform.rotation = Quaternion.Slerp(transform.rotation, follow.transform.rotation, speed * Time.deltaTime);
        if (Input.GetKeyDown("l"))
        {
            ChargeUp(200);
        }

    }

    IEnumerator Discharge()
    {
        difficoultyLvl = follow.GetComponentInChildren<EnergyBar>().getDifficultyLvl();
        if (difficoultyLvl == "Easy")
        {
            dischargeRate = 1;
        }
        else if (difficoultyLvl == "Medium")
        {
            dischargeRate = 2;
        }
        else if (difficoultyLvl == "Hard")
        {
            dischargeRate = 3;
        }
        else
        {
            dischargeRate = 1;
        }

        for (; BatteryCharge >= 0; BatteryCharge -= dischargeRate)
        {
            follow.GetComponentInChildren<EnergyBar>().updateEnergyBar(BatteryCharge);
            yield return new WaitForSeconds(.07f);
        }

        if (BatteryCharge <= 0)
        {
            lightItself.enabled = true;
            yield return new WaitForSeconds(.1f);
            lightItself.enabled = false;
            yield return new WaitForSeconds(.03f);
            lightItself.enabled = true;
            yield return new WaitForSeconds(.08f);
            lightItself.enabled = false;
            yield return new WaitForSeconds(.03f);
            lightItself.enabled = true;
            yield return new WaitForSeconds(.08f);
            lightItself.enabled = false;
            yield return new WaitForSeconds(.03f);
            lightItself.enabled = true;


        }



        Toggle();
    }

    public void ChargeUp(int energy)
    {
        if (BatteryCharge+energy >= 500)
        {
            BatteryCharge = 500;
        }else
        {
            BatteryCharge += energy;
        }
        follow.GetComponentInChildren<EnergyBar>().updateEnergyBar(BatteryCharge);

    }

    public int CheckChargeLevel()
    {
        return BatteryCharge;
    }
}
