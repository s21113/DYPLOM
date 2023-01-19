using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Opcja do smartfona z możliwym włączeniem czegoś
/// </summary>
public class SmartphoneOption
{
    public string name;
    public Action action;
    public bool isSelected = false;
    public bool isActive = false;
    public bool isSelectable = true;
    public bool isMenu = false;
    public Toggle toggle;

    /// <summary>
    /// Tworzy nowa opcje do smartfona bez strony graficznej
    /// </summary>
    /// <param name="s">Nazwa opcji w smartfonie</param>
    public SmartphoneOption(string s)
    {
        name = s;
        isActive = false;
        isSelected = false;
    }

    /// <summary>
    /// Tworzy nowa opcje do smartfona bez strony graficznej
    /// </summary>
    /// <param name="s">Nazwa opcji w smartfonie</param>
    /// <param name="a">Metoda ktora ma być wywolana</param>
    public SmartphoneOption(string s, Action a)
    {
        name = s;
        action = a;
        isActive = false;
        isSelected = false;
    }

    /// <summary>
    /// Tworzy nowa opcje do smartfona bez strony graficznej
    /// </summary>
    /// <param name="s">Nazwa opcji w smartfonie</param>
    /// <param name="a">Metoda ktora ma być wywolana</param>
    /// <param name="b">...a może już niech będzie aktywna?</param>
    public SmartphoneOption(string s, Action a, bool b)
    {
        name = s;
        action = a;
        isActive = b;
        isSelected = false;
    }

    /// <summary>
    /// Tworzy nowa opcje do smartfona bez strony graficznej
    /// </summary>
    /// <param name="s">Nazwa opcji w smartfonie</param>
    /// <param name="a">Metoda ktora ma być wywolana</param>
    /// <param name="b1">...a może już niech będzie aktywna?</param>
    /// <param name="b2">a może nie można jej wybrać</param>
    public SmartphoneOption(string s, Action a, bool b1, bool b2)
    {
        name = s;
        action = a;
        isActive = b1;
        isSelected = false;
        isSelectable = b2;
    }

    public void SetSelection(bool b)
    {
        if (!isSelectable) return;
        isSelected = b;
    }

    public void Execute()
    {
        if (!isSelected) return;
        if (!isSelectable) return;
        action.Invoke();
        isActive = !isActive;
    }
}


/// <summary>
/// Przechowalnia listy opcji
/// </summary>
public class SmartphoneOptions
{
    private List<SmartphoneOption> options;
    public SmartphoneOptions()
    {
        options = new List<SmartphoneOption>();
    }
    
    public SmartphoneOptions(params SmartphoneOption[] options)
    {
        this.options = new List<SmartphoneOption>(options);
    }
    public SmartphoneOptions(IEnumerable<SmartphoneOption> options)
    {
        this.options = new List<SmartphoneOption>(options);
    }

    public void AddOption(SmartphoneOption opt)
    {
        options.Add(opt);
    }

    /// <summary>
    /// Pobiera opcję z określonego indeksu
    /// </summary>
    public SmartphoneOption GetOption(int index)
    {
        return options[index];
    }

    /// <summary>
    /// chyba self-explanatory
    /// </summary>
    public int GetSize()
    {
        return options.Count;
    }

    /// <summary>
    /// Zwraca obecnie zaznaczoną opcję
    /// </summary>
    public SmartphoneOption GetCurrentlySelected()
    {
        SmartphoneOption option = null;
        foreach (var opt in options)
        {
            if (opt.isSelected)
            {
                return opt;
            }
        }
        return option;
    }

    /// <summary>
    /// Zwraca indeks obecnie zaznaczonej opcji
    /// </summary>
    public int GetCurrentlySelectedIndex()
    {
        int index = -1;
        for (int i = 0; i < options.Count; i++)
        {
            if (options[i].isSelected)
            {
                return i;
            }
        }
        return index;
    }

    /// <summary>
    /// Znajduje opcję smartfona poprzez jej dokładną nazwę
    /// </summary>
    public SmartphoneOption FindByName(string name)
    {
        SmartphoneOption option = null;
        foreach (var opt in options)
        {
            if (opt.name.Equals(name))
            {
                return opt;
            }
        }
        return option;
    }

    /// <summary>
    /// Znajduje opcję smartfona poprzez część jej nazwy
    /// </summary>
    public SmartphoneOption FindByNamePart(string namepart)
    {
        SmartphoneOption option = null;
        foreach (var opt in options)
        {
            if (opt.name.Contains(namepart))
            {
                option = opt;
                break;
            }
        }
        return option;
    }

    /// <summary>
    /// Zwraca indeks pewnej opcji... po chuj mi to?
    /// </summary>
    public int FindIndexOfOption(SmartphoneOption option)
    {
        int index = -1;
        for (int i = 0; i < options.Count; i++)
        {
            if (options[i] == option)
            {
                index = i;
                break;
            }
        }
        return index;
    }

    public void Select(int index = 0)
    {
        options[index].SetSelection(true);
        for (int i = 0; i < options.Count; i++)
        {
            if (i != index) options[i].SetSelection(false);
        }
    }
	
	public void Deselect(int index = 0)
	{
        options[index].SetSelection(false);
	}

    public override string ToString()
    {
        string rtrn = "smartphoneOptions:";
        rtrn += "\nsize: " + GetSize();
        rtrn += "\ncurrently selected: ";
        if (GetCurrentlySelected() == null) rtrn += "none";
        else rtrn += GetCurrentlySelected().name;
        foreach (var opt in options)
        {
            rtrn += "\noption " + opt.name;
        }
        return rtrn;
    }
}