using System;
using Random = System.Random;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Bo mi się kurwa nie chce babrać z klasą dictionary XDD
/// Sobie zrobiłem klasę przechowującą elegancko równania i rozwiązanie
/// Mój wymarzony format równań: "liczba operacja liczba"
/// jak mi dacie jakikolwiek inny, to się zesram i zajebię (nie no żart)
/// </summary>

[Serializable]
public class MathGame
{
    /// <summary>
    /// równanie, dostępne publicznie
    /// </summary>
    [SerializeField] public string equation;
    [SerializeField] private int solution;
    private const string EQ_PATTERN = @"([0-9]{1,2})[ ]{0,1}([+\-*/^]{1})[ ]{0,1}([0-9]{1,2})";

    /// <summary>
    /// Rozwiązuje równanie
    /// </summary>
    /// <param name="s">input gracza</param>
    /// <returns>rozwiązanie równania</returns>
    private int Solve(string s)
    {
        if (s.Equals(string.Empty)) return -213769420;
        var args = Regex.Match(equation, EQ_PATTERN);
        int num1 = int.Parse(args.Groups[1].Value),
            num2 = int.Parse(args.Groups[3].Value);
        char operation = args.Groups[2].Value[0];

        switch (operation)
        {
            case '+': return num1 + num2;
            case '-': return num1 - num2;
            case '*': return num1 * num2;
            case '/': return num1 / num2;
            case '^': return (int)Math.Pow(num1, num2);
            default: throw new MinigameException("Kutasy wsadziliście mi jakiś dziwny znak którego nie kumam XD");
        }
    }

    /// <summary>
    /// Inicjalizacja xd
    /// </summary>
    /// <param name="s">Równanie w formacie "liczba operacja liczba"</param>
    public MathGame(string s)
    {
        equation = s;
        solution = Solve(s);
    }

    /// <summary>
    /// Sprawdza, czy rozwiązanie podane przez gracza odpowiada faktycznemu
    /// </summary>
    /// <param name="playerInput"></param>
    /// <returns>true, jeśli wszystko się zgadza</returns>
    public bool CheckEquation(string playerInput)
    {
        if (playerInput.Length < 1) return false;
        int playerSolution = int.Parse(Regex.Match(playerInput, "[0-9]*").Value);
        return playerSolution == solution;
    }
}

[Serializable]
public class MathEquations
{
    public MathGame[] equations;
}

public class Math_MiniGame : MinigameBase<MathGame>
{
    public override List<MathGame> CreateQuestions()
    {
        List<MathGame> returnValue = new List<MathGame>();

        // fetchowanie ze zbigniewa JSONa
        var jsonFile = Resources.Load<TextAsset>("Minigames/math_equations");
        var tmp = JsonUtility.FromJson<MathEquations>(jsonFile.text);
        MathGame[] eqs = tmp.equations;
        foreach (MathGame eq in eqs)
            returnValue.Add(eq);
        return returnValue;
    }

    public override MathGame GetRandomQuestion()
    {
        int rand = new Random().Next(availableQuestions.Count);
        return availableQuestions[rand];
    }

    public override bool Solve(GameObject parent, InputField solutionInput)
    {
        var isCorrect = currentQuestion.CheckEquation(solutionInput.text);
        if (isCorrect == true)
        {
            //parent.GetComponentInChildren<AudioSource>().PlayOneShot(successAudio);
            parent.transform.parent.gameObject.SetActive(false);
            return true;
        }
        else
        {
            solutionInput.text = string.Empty;
            //parent.GetComponentInChildren<AudioSource>().PlayOneShot(failureAudio);
            return false;
        }
    }

    public Math_MiniGame(GameObject parent, Text desc, string descString, string currQuestion = null, string answers = null) : base(parent, desc, descString, currQuestion, answers)
    {
        /*availableQuestions = CreateQuestions();
        currentQuestion = GetRandomQuestion();*/
        InitGUI(parent, desc, descString, currentQuestion.equation);
    }
}
