using System;
using Random = System.Random;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class RebusGame
{
    [SerializeField] public string imagePath;
    public Sprite[] images;
    [SerializeField] public string solution;

    public RebusGame(string imageLoc, string answer)
    {
        imagePath = $"{Directory.GetCurrentDirectory()}\\Assets\\Resources\\Minigames\\{imageLoc.Replace('/','\\')}";
        solution = answer;
        string spritesPath = $"Minigames/{imageLoc}";
        images = Resources.LoadAll<Sprite>(spritesPath);
    }

    public bool Check(string text)
    {
        return text.ToLower().Equals(solution.ToLower());
    }
}

[Serializable]
public class Rebuses
{
    public RebusGame[] questions;
}

public class Rebus_MiniGame : MinigameBase<RebusGame>
{
    public override List<RebusGame> CreateQuestions()
    {
        List<RebusGame> returnValue = new List<RebusGame>();

        // fetchowanie ze zbigniewa JSONa
        var jsonFile = Resources.Load<TextAsset>("Minigames/images_questions");
        var tmp = JsonUtility.FromJson<Rebuses>(jsonFile.text);
        RebusGame[] qs = tmp.questions;
        foreach (RebusGame q in qs)
            returnValue.Add(new RebusGame(q.imagePath, q.solution));
        return returnValue;
    }

    public override RebusGame GetRandomQuestion()
    {
        int rand = new Random().Next(availableQuestions.Count);
        return availableQuestions[rand];
    }

    public override bool Solve(GameObject parent, InputField solutionInput)
    {
        var isCorrect = currentQuestion.Check(solutionInput.text);
        if (isCorrect == true)
        {
            //parent.GetComponentInChildren<AudioSource>().PlayOneShot(successAudio);
            Debug.LogWarning("halo ziemia prosze dodać SUKCES audio clip cichy i nieinwazyjny");
            parent.transform.parent.gameObject.SetActive(false);
            GameObject.Destroy(_this);
            return true;
        }
        else
        {
            solutionInput.text = string.Empty;
            //parent.GetComponentInChildren<AudioSource>().PlayOneShot(failureAudio);
            Debug.LogWarning("halo ziemia prosze dodać failure audio clip cichy i nieinwazyjny");
            return false;
        }
    }

    public override void InitGUI(GameObject parent, Text desc, string descString, string currQuestion, string answers = null)
    {
        desc.text = descString;
        GameObject theObj = new GameObject("Rebus");
        var grouper = theObj.AddComponent<HorizontalLayoutGroup>();
        grouper.spacing = 32;
        grouper.childAlignment = TextAnchor.MiddleCenter;
        for (int i = 0; i < currentQuestion.images.Length; i++)
        {
            GameObject _img = new GameObject("Image " + (i+1));
            var img = _img.AddComponent<Image>();
            img.sprite = currentQuestion.images[i];
            img.preserveAspect = true;
            _img.transform.SetParent(theObj.transform);
        }
        theObj.transform.SetParent(parent.transform);
        var rect = theObj.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(900, 400);
        rect.localPosition = new Vector2(0, 0);
        _this = theObj;
    }

    public Rebus_MiniGame(GameObject parent, Text desc, string descString, string currQuestion = null, string answers = null) : base(parent, desc, descString, currQuestion, answers)
    {
        /*availableQuestions = CreateQuestions();
        currentQuestion = GetRandomQuestion();*/
        InitGUI(parent, desc, descString, null);
    }
}
