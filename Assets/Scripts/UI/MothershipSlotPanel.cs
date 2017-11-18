using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MothershipSlotPanel : MonoBehaviour
{
    public static MothershipSlotPanel Instance;
    public GameObject EmptySlotObject;
    public Mothership MothershipObject;

    void Awake()
    {
        Instance = this;
    }

    public void Initialize()
	{
        foreach (var slot in MothershipObject.Slots)
	    {
	        var elem = Instantiate(EmptySlotObject, transform);
	        slot.SlotPanelElement = elem;
	    }

	}
}
