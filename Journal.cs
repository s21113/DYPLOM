using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Journal", menuName = "Inventory/Journal")]
public class Journal : ScriptableObject
{
    [SerializeField] new string name;
    [SerializeField] public string category;
    [TextArea(2, 99)]
    [SerializeField] public string contents;
}
