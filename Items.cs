using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Klasa Items będzie przechowywała wszystkie MOŻLIWE przedmioty
/// które będzie można nadać graczowi.
/// Nie przechowuje ona ekwipunku gracza.
/// </summary>
public class Items : MonoBehaviour
{
    public List<Item> possibleItems;

    public void AddItem(Item i)
    {
        possibleItems.Add(i);
    }
    public void RemoveItem(Item i)
    {
        possibleItems.Remove(i);
    }
}
