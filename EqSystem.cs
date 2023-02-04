using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

/// <summary>
/// Klasa EqSystem będzie wreszcie przechowywała
/// ekwipunek gracza i jego rejestr wiadomości.
/// </summary>
public class EqSystem : MonoBehaviour
{
    readonly public static int INVENTORY_SIZE = 9;
    int importantPoints = 0;
    [SerializeField] private List<Item> items = new List<Item>();
    [SerializeField] private List<Journal> journals = new List<Journal>();
    private List<Item> collectedItems;
    private List<Journal> collectedJournals;

    public void addImportantPoint()
    {
        importantPoints += 1;
    }
    public int GetImportantPoints()
    {
        return importantPoints;
    }
    public void func_f742358363()
    {
        if (importantPoints == 7) return;
        importantPoints += 1;
    }

    public void PickupItem(Item i)
    {
        collectedItems.Add(i);
        addImportantPoint();
    }
    public void UseItem(Item i)
    {
        collectedItems.Remove(i);
    }
    public List<Item> GetCollectedItems()
    {
        return collectedItems;
    }
    
    public void ReceiveMessage(Journal j)
    {
        if (collectedJournals.Contains(j)) return;
        collectedJournals.Add(j);
    }
    public List<Journal> GetCollectedJournals()
    {
        return collectedJournals;
    }

    public Item FindItem(string match)
    {
        return items.Find(i => i.displayName.Trim().Equals(match))
            ?? items.Find(i => i.displayName.Trim().ToLower().Contains(match.ToLower()));
    }
    public Journal FindJournal(string match)
    {
        return journals.Find(j => j.title.Trim().Equals(match))
            ?? journals.Find(j => j.title.Trim().ToLower().Contains(match.ToLower()));
    }

    IEnumerator TestMessagingService()
    {
        ReceiveMessage(FindJournal("Story 1"));
        yield return new WaitForSeconds(20f);
        ReceiveMessage(FindJournal("Story 2"));
        yield break;
    }

    void Awake()
    {
        collectedItems = new List<Item>();
        collectedJournals = new List<Journal>();
    }

    void Start()
    {
        items = new List<Item>(Resources.LoadAll<Item>("Inventory/Items"));
        journals = new List<Journal>(Resources.LoadAll<Journal>("Inventory/Journals"));
        StartCoroutine(TestMessagingService());
    }

    void Update()
    {

    }
}
