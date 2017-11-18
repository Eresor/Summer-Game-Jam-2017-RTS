using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : NetworkBehaviour
{

    public static ResourceManager Instance;

    public Text ManpowerText;
    public Text GoldText;

    public float Manpower;
    public float Gold = 10000f;
	// Use this for initialization
	protected override void NetworkStart ()
	{
	    Instance = this;
	}
	
	// Update is called once per frame
	protected override void NetworkUpdate ()
	{

	    Manpower += 10 * Time.deltaTime;
	    ManpowerText.text = "Manpower: " + Manpower.ToString(("#####0"));
	    GoldText.text = "Gold: " + Gold.ToString(("#####0"));

    }
}
