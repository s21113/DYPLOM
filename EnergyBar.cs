using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBar : MonoBehaviour
{
    double health;
    double healthMax;
    double stamina;
    double staminaMaxLevel;
    private double energy;
    public double energyMax;
    public double staminaRate;
    private string difficultyLvl;

    public GameObject fatigueObj;
    private AudioSource heartbeat;
    private Animator fatigueAnimator;

    // Start is called before the first frame update
    void Awake()
    {
        //LOL
        difficultyLvl = "Easy";
        stamina = 300;
        staminaMaxLevel = 300;
        staminaRate = 0.11f; //how fast is decreasing
        health = 4f;
        healthMax = 4f;
        energy = 500;
        energyMax = 500;
    }

    public void Start()
    {
        fatigueObj = GameObject.Find("Fatigue FX");
        heartbeat = fatigueObj.GetComponent<AudioSource>();
        fatigueAnimator = fatigueObj.GetComponent<Animator>();
    }

    public void Update()
    {
        fatigueAnimator.SetFloat("Stamina", (float)(stamina / staminaMaxLevel));
        //Debug.Log(health);
    }

    public void SetDifficultyLvl(string difficulty)
    {
        
        difficultyLvl = difficulty;
        if (difficultyLvl == "Easy")
        {
            stamina = 300;
            staminaMaxLevel = 300;
            staminaRate = 0.08f; //how fast is decreasing
            health = 4f;
            healthMax = 4f;

        }
        else if (difficultyLvl == "Medium")
        {
            stamina = 200;
            staminaMaxLevel = 200;
            staminaRate = 0.1f;
            health = 4f;
            healthMax = 4f;

        }
        else if (difficultyLvl == "Hard")
        {
            stamina = 100;
            staminaMaxLevel = 100;
            staminaRate = 0.1f;
            health = 4f;
            healthMax = 4f;
 
        }

        Debug.Log(difficulty);

        
    }




    public void DecreaseHealth()
    {
        if (health > 0)
        {
            float level = (float)((health -= 1f) / healthMax);
        }
    }

    public void IncreaseHealth()
    {
        if (health < 4)
        {
            float level = (float) ((health += 1) / healthMax);
        }
    }


    public void updateStaminaBar()
    {
        float level = (float)((stamina -= staminaRate) / staminaMaxLevel);
        //staminaBar.localScale = new Vector3(level * 6, 3f);
    }

    public bool regenerateStaminaBar(int amount)
    {
        
        float level = (float)((stamina += amount) / staminaMaxLevel);
        //staminaBar.localScale = new Vector3(level * 6, 3f);
        if (stamina >= staminaMaxLevel)
        {
            stamina = staminaMaxLevel;
            return false;
        }
        else
        {
            return true;
        }
    }

    public void setConcreteStamina()
    {
        float level = (float)(stamina / staminaMaxLevel);
        //staminaBar.localScale = new Vector3(level * 6, 3f);
    }

    public void updateEnergyBar(double energyx)
    {
        energy = energyx;
        float level = (float)(energy / energyMax);
        //energyBar.localScale = new Vector3(level * 6, 3f);
    }

    public double GetStaminaLevel()
    {
        return stamina;
    }

    public double GetHealthLevel()
    {
        return health;
    }

    public double GetEnergyLevel()
    {
        return energy;
    }

    public string getDifficultyLvl()
    {
        return difficultyLvl;
    }


}
