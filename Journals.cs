using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Klasa Journals będzie przechowywała wszystkie MOŻLIWE wiadomości
/// które gracz będzie mógł wyświetlić.
/// Nie przechowuje ona rejestru wiadomości gracza.
/// </summary>
public class Journals : MonoBehaviour
{
    public List<Journal> possibleJournals;

    public void AddJournal(Journal j)
    {
        possibleJournals.Add(j);
    }
    public void RemoveJournal(Journal j)
    {
        possibleJournals.Remove(j);
    }
}
