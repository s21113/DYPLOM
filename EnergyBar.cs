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

    public Animator fatigueAnimator;

    // Start is called before the first frame update
    void Awake()
    {
        difficultyLvl = "Easy";
        stamina = 300;
        staminaMaxLevel = 300;
        staminaRate = 0.08f; //how fast is decreasing
        health = 4f;
        healthMax = 4f;
        energy = 500;
        energyMax = 500;


    }

    public void Start()
    {

    }

    public void Update()
    {
        fatigueAnimator.SetFloat("Stamina", (float)(stamina / staminaMaxLevel));
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetDifficultyLvl("Easy");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetDifficultyLvl("Medium");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetDifficultyLvl("Hard");
        }

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




    public void updateHealthBarDown()
    {
        if (health > 0) {
            float level = (float)((health -= 1f) / healthMax);
            Debug.Log(level);
            //healthBar.localScale = new Vector3(level * 6, 3f);
        }
    }

    public void updateHealthBarUP()
    {
        if (health < 4)
        {
            float level = (float) ((health += 1) / healthMax);
            Debug.Log(level);
            //healthBar.localScale = new Vector3(level * 6, 3f);
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
