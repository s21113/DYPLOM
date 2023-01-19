using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartphoneMenu : SmartphoneOption
{/*
    public GameObject CreateMenuInterface()
    {
        //TODO
    }*/

    public SmartphoneMenu(string s) : base(s)
    {
		base.isMenu = true;
    }

    public SmartphoneMenu(string s, Action a) : base(s, a)
    {
		base.isMenu = true;
    }

    public SmartphoneMenu(string s, Action a, bool b) : base(s, a, b)
    {
		base.isMenu = true;
    }

    public SmartphoneMenu(string s, Action a, bool b1, bool b2) : base(s, a, b1, b2)
    {
		base.isMenu = true;
    }


}