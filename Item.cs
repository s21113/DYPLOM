using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public Sprite icon;
    public string displayName;
    public string tag;
    [TextArea(1, 99)] public string description;
}
