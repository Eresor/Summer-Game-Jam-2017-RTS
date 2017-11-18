using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarracksManager : MonoBehaviour
{
    public static BarracksManager Instance;
    public List<Barracks> BarracksList;
	
    // Use this for initialization
	void Start ()
	{
	    Instance = this;
        BarracksList = new List<Barracks>();

    }

    public void Build(UnitsButton button)
    {
        foreach (var barracks in BarracksList)
        {
            if (Math.Abs(barracks.Progress) < 0.01f && barracks.ObjectToBuild == "" && barracks.Level >= button.RequiredBarracksLevel 
                && ResourceManager.Instance.Gold >= button.RequiredGold && ResourceManager.Instance.Manpower >= button.RequiredManpower)
            {
                ResourceManager.Instance.Gold -= button.RequiredGold;
                ResourceManager.Instance.Manpower -= button.RequiredManpower;
                barracks.ObjectToBuild = button.ObjectName;
                break;
            }
        }
    }

    void Update()
    {
        foreach (var barracks in BarracksList)
        {
            barracks.UpdateBarracks();
        }
    }
}
