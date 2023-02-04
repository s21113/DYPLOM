using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// Klasa EqSystem będzie wreszcie przechowywała
/// ekwipunek gracza i jego rejestr wiadomości.
/// </summary>
public class EqSystem : MonoBehaviour
{
    public Items _i;
    public Journals _j;
    readonly public static int INVENTORY_SIZE = 9;
    int importantPoints = 0;
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
        collectedJournals.Add(j);
    }
    public List<Journal> GetCollectedJournals()
    {
        return collectedJournals;
    }

    IEnumerator TestMessagingService()
    {
        ReceiveMessage(_j.possibleJournals[0]);
        yield return new WaitForSeconds(20f);
        ReceiveMessage(_j.possibleJournals[1]);
        yield break;
    }

    void Awake()
    {
        collectedItems = new List<Item>();
        collectedJournals = new List<Journal>();
    }

    void Start()
    {
        StartCoroutine(TestMessagingService());
    }

    void Update()
    {

    }
}
