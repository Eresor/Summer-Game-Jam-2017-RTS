using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PanelManager : MonoBehaviour
{

    public static PanelManager Instance;

    public GameObject MultiUnitsPanel;
    public GameObject SingleUnitPanel;
    public GameObject MultiUnitsButton;
    private int previousNumUnits;
    // Use this for initialization

    void Start ()
    {
        Instance = this;
	    SetActivePanel(null);
	}

    void SetActivePanel(GameObject panel)
    {
        MultiUnitsPanel.SetActive(false);
        SingleUnitPanel.SetActive(false);

        if (panel)
            panel.SetActive(true);
    }

	// Update is called once per frame
	void Update ()
	{

	    var numUnits = UnitSelection.selectedObjects.Count;

        if(previousNumUnits==numUnits)
            return;

        Transform transform = MultiUnitsPanel.GetComponent<RectTransform>();
        foreach (Transform child in transform)
        {
            if (child.transform != transform)
                Destroy(child.gameObject);
        }

        if (numUnits > 1)
	    {
            SetActivePanel(MultiUnitsPanel);

	        foreach (var selected in UnitSelection.selectedObjects)
	        {
                var button = Instantiate(MultiUnitsButton, transform);
	            button.GetComponent<MultiUnitButton>().ConnectedUnit = selected;
                //to remove
                button.GetComponentInChildren<Text>().text = selected.name;
	            //
	        }
	    }
        else if (numUnits == 1)
        {
            SelectSingleUnit(UnitSelection.selectedObjects[0]);
        }
        else
        {
            SetActivePanel(null);
        }

	    previousNumUnits = numUnits;
	}

    public void SelectSingleUnit(SelectableUnit unit)
    {
        SetActivePanel(SingleUnitPanel);
        SingleUnitPanel.GetComponent<SingleUnitPanel>().UnitName.text = unit.UnitName;
    }
}
