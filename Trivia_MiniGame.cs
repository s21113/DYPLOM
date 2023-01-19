using System;
using Random = System.Random;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Wiecie ocb. Klasa przechowująca pytanie i tablicę odpowiedzi, z czego jedna z nich jest poprawna
/// </summary>

[Serializable]
public class TriviaGame
{
    [SerializeField] public string question;
    [SerializeField] public string[] answers = new string[3];
    private string correctAnswer;

    /// <summary>
    /// Skradzione z https://stackoverflow.com/a/110570
    /// </summary>
    private T[] Kloc<T>(Random rng, T[] arr)
    {
        int n = arr.Length;
        while (n > 1)
        {
            int k = rng.Next(n--);
            T temp = arr[n];
            arr[n] = arr[k];
            arr[k] = temp;
        }
        return arr;
    }

    public TriviaGame(string q, params string[] answers)
    {
        Random rng = new Random();
        question = q;
        if (answers.Length != this.answers.Length) throw new MinigameException("Kutasy dajecie mi dwie lub cztery odpowiedzi zamiast trzech XD");
        for (int i = 0; i < 3; i++)
        {
            if (!answers[i].StartsWith("CORRECT: ")) continue;
            answers[i] = answers[i].Replace("CORRECT: ", "");
            correctAnswer = answers[i];
            break;
        }
        // TODO jebane shufflowanie tego gówna
        this.answers = answers;
    }

    public string FormatQuestionAnswers()
    {
        return $"A: {answers[0]}\nB: {answers[1]}\nC: {answers[2]}";
    }

    public bool Check(string text)
    {
        if (text == null || text == "") return false;
        if (text.Length < 2)
        {
            int index = -1;
            if (text[0].Equals('A')) index = 0;
            else if (text[0].Equals('B')) index = 1;
            else if (text[0].Equals('C')) index = 2;
            return answers[index].Equals(correctAnswer);
        }
        foreach (var x in answers)
        {
            if (x.Contains(text))
                return true;
        }
        return false;
    }
}

[Serializable]
public class TriviaQuestions
{
    public TriviaGame[] questions;
}


public class Trivia_MiniGame : MinigameBase<TriviaGame>
{
    public override List<TriviaGame> CreateQuestions()
    {
        List<TriviaGame> returnValue = new List<TriviaGame>();

        // fetchowanie ze zbigniewa JSONa
        var jsonFile = Resources.Load<TextAsset>("Minigames/trivia_questions");
        var tmp = JsonUtility.FromJson<TriviaQuestions>(jsonFile.text);
        TriviaGame[] qs = tmp.questions; //wszystkie pytania sfetchowane
        foreach (TriviaGame q in qs)
        {
            TriviaGame qx = new TriviaGame(q.question, q.answers);
            returnValue.Add(qx);
        }
        return returnValue;
    }

    public override TriviaGame GetRandomQuestion()
    {
        int rand = new Random().Next(availableQuestions.Count);
        return availableQuestions[rand];
    }

    public Trivia_MiniGame(GameObject parent, Text desc, string descString, string currQuestion = null, string answers = null) : base(parent, desc, descString, currQuestion, answers)
    {
        /*availableQuestions = CreateQuestions();
        currentQuestion = GetRandomQuestion();*/
        InitGUI(parent, desc, descString, currentQuestion.question, currentQuestion.FormatQuestionAnswers());
    }

    public override bool Solve(GameObject parent, InputField solutionInput)
    {
        var isCorrect = currentQuestion.Check(solutionInput.text);
        if (isCorrect == true)
        {
            //parent.GetComponentInChildren<AudioSource>().PlayOneShot(failureAudio);
            Debug.LogWarning("halo ziemia prosze dodać SUKCES audio clip cichy i nieinwazyjny");
            parent.transform.parent.gameObject.SetActive(false);
            GameObject.Destroy(_this);
            return true;
        }
        else
        {
            solutionInput.text = string.Empty;
            //parent.GetComponentInChildren<AudioSource>().PlayOneShot(successAudio);
            Debug.LogWarning("halo ziemia prosze dodać failure audio clip cichy i nieinwazyjny");
            return false;
        }
    }
}
