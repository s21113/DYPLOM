using Random = System.Random;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public abstract class MinigameBase<T>
{
    public T currentQuestion;
    public List<T> availableQuestions;
    public AudioClip successAudio, failureAudio;
    public GameObject _this;

    public abstract List<T> CreateQuestions();
    public abstract T GetRandomQuestion();
    public abstract bool Solve(GameObject parent, InputField solutionInput);
    public virtual bool Solve(GameObject parent, InputField solutionInput, bool isCorrect)
    {
        if (isCorrect == true)
        {
            //parent.GetComponentInChildren<AudioSource>().PlayOneShot(successAudio);
            Debug.LogWarning("halo ziemia prosze dodać SUKCES audio clip cichy i nieinwazyjny");
            parent.transform.parent.gameObject.SetActive(false);
            _this.SetActive(false);
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

    public virtual void InitGUI(GameObject parent, Text desc, string descString, string currQuestion, string answers = null)
    {
        desc.text = descString;
        GameObject theObj = new GameObject(typeof(T).Name);
        Text equation = theObj.AddComponent<Text>();
        equation.text = currQuestion;
        if (answers != null) equation.text += ("\n\n" + answers);
        equation.color = new Color(1, 1, 1);
        equation.alignment = TextAnchor.MiddleCenter;
        equation.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        equation.fontSize = 50;
        theObj.transform.SetParent(parent.transform);
        var rect = theObj.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(800, 400);
        rect.localPosition = new Vector2(0, 0);
        _this = theObj;
    }

    public void CloseGUI(GameObject parent)
    {
        parent.transform.parent.gameObject.SetActive(false);
        GameObject.Destroy(_this.transform.gameObject);
    }

    public void BringUp(GameObject parent)
    {
        parent.transform.parent.gameObject.SetActive(true);
    }

    public MinigameBase(GameObject parent, Text desc, string descString, string currQuestion = null, string answers = null)
    {
        availableQuestions = CreateQuestions();
        currentQuestion = GetRandomQuestion();
    }
}
