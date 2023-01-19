using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = System.Random;

public class MiniGameInterface : MonoBehaviour
{
    public bool isSolved;
    public string currentMinigameType = "";
    public GameObject parent;
    public Text descriptionText;
    public InputField solutionInput;
    private object minigame;

    public void LoadMinigame()
    {
        int decision = new Random().Next(0, 2);

        switch (decision)
        {
            case 0:
                currentMinigameType = "Math";
                minigame = new Math_MiniGame(parent, descriptionText, "Solve the equation");
                return;
            case 1:
                currentMinigameType = "Trivia";
                minigame = new Trivia_MiniGame(parent, descriptionText, "Answer the question");
                return;
            case 2:
                currentMinigameType = "Rebus";
                minigame = new Rebus_MiniGame(parent, descriptionText, "Solve the riddle");
                return;
            default:
                throw new MinigameException("Koronawirus nie istnieje");
        }
    }

    private void Update()
    {
        if (!gameObject.activeSelf) return;
    }

    public void Solve()
    {
        switch (currentMinigameType)
        {
            case "Math":
                isSolved = (minigame as Math_MiniGame).Solve(parent, solutionInput);
                break;
            case "Trivia":
                isSolved = (minigame as Trivia_MiniGame).Solve(parent, solutionInput);
                break;
            case "Rebus":
                isSolved = (minigame as Rebus_MiniGame).Solve(parent, solutionInput);
                break;
            default:
                Destroy(gameObject);
                throw new MinigameException("Solve(): a wypierdalajcie mi z tym");
        }
        if (isSolved)
            Destroy(gameObject);
    }

    public void BringUp()
    {
        //wtf.BringUp(parent);
        switch (currentMinigameType)
        {
            case "Math":
                (minigame as Math_MiniGame).BringUp(parent);
                break;
            case "Trivia":
                (minigame as Trivia_MiniGame).BringUp(parent);
                break;
            case "Rebus":
                (minigame as Rebus_MiniGame).BringUp(parent);
                break;
            default:
                throw new MinigameException("BringUp(): a wypierdalajcie mi z tym");
        }
        EventSystem.current.SetSelectedGameObject(solutionInput.gameObject, null);
        solutionInput.OnPointerClick(new PointerEventData(EventSystem.current));
    }

    public void Escape()
    {
        switch (currentMinigameType)
        {
            case "Math":
                (minigame as Math_MiniGame).CloseGUI(parent);
                break;
            case "Trivia":
                (minigame as Trivia_MiniGame).CloseGUI(parent);
                break;
            case "Rebus":
                (minigame as Rebus_MiniGame).CloseGUI(parent);
                break;
            default:
                throw new MinigameException("Escape(): a wypierdalajcie mi z tym");
        }
        minigame = null;
        currentMinigameType = "";
    }
}

public class MinigameException : Exception
{
    public MinigameException(string s) : base(s) { }
}