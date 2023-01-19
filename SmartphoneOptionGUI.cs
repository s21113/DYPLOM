using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>Kolorki</summary>
public static class SelectionColors
{
    public static readonly Color
        UNSELECTED_DISABLED = new Color(95f / 255f, 95f / 255f, 95f / 255f, 1f),
        UNSELECTED_ENABLED = new Color(68f / 255f, 85f / 255f, 95f / 255f, 1f),
        SELECTED_DISABLED = new Color(207f / 255f, 207f / 255f, 207f / 255f, 1f),
        SELECTED_ENABLED = new Color(128f / 255f, 161f / 255f, 189f / 255f, 1f);
}

/// <summary>Klawisze używane do nawigacji smartfonem</summary>
public static class NavigationKeys
{
    public static KeyCode menuUp = KeyCode.UpArrow;
    public static KeyCode menuDown =  KeyCode.DownArrow;
    public static KeyCode menuBack =  KeyCode.LeftArrow;
    public static KeyCode menuAdvance = KeyCode.RightArrow;
    public static KeyCode alt_menuUp = KeyCode.Keypad8;
    public static KeyCode alt_menuDown = KeyCode.Keypad2;
    public static KeyCode alt_menuBack = KeyCode.Keypad4;
    public static KeyCode alt_menuAdvance = KeyCode.Keypad6;
}

/// <summary>Ta klasa tylko zarządza kolorkami</summary>
public class SmartphoneOptionGUI : MonoBehaviour
{
    private string optionName;
    private SmartphoneOption option;
    private Toggle toggle;
    private Image img;
    private float colorChangeTime = 0.4f;

    /// <summary>
    /// Paruje opcję smartfona z interfejsem graficznym
    /// </summary>
    public void Set(SmartphoneOption opt)
    {
        option = opt;
        optionName = opt.name;
        toggle = GetComponent<Toggle>();
        img = GetComponent<Image>();
    }

    /// <summary>
    /// Zmienia kolory opcji
    /// </summary>
    public void Execute()
    {
        if (!option.isSelected) return;
        if (!option.isSelectable) return;
        Color targetColor = option.isActive ? SelectionColors.SELECTED_ENABLED : SelectionColors.SELECTED_DISABLED;
        img.CrossFadeColor(targetColor, colorChangeTime, true, false);
    }

    public void Select()
    {
        StartCoroutine(Select_color());
    }
    public void Deselect()
    {
        StartCoroutine(Unselect_color());
    }

    IEnumerator Select_color()
    {
        if (!option.isSelectable) yield break;
        option.SetSelection(true);
        Color targetColor = option.isActive ? SelectionColors.SELECTED_ENABLED : SelectionColors.SELECTED_DISABLED;
        img.CrossFadeColor(targetColor, colorChangeTime, true, false);
        yield return new WaitForSeconds(colorChangeTime);
        StopCoroutine(Select_color());
    }
    IEnumerator Unselect_color()
    {
        if (!option.isSelectable) yield break;
        option.SetSelection(false);
        Color targetColor = option.isActive ? SelectionColors.UNSELECTED_ENABLED : SelectionColors.UNSELECTED_DISABLED;
        img.CrossFadeColor(targetColor, colorChangeTime, true, false);
        yield return new WaitForSeconds(colorChangeTime);
        StopCoroutine(Unselect_color());
    }
}