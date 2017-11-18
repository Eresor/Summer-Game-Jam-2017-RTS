using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiUnitButton : MonoBehaviour
{

    public SelectableUnit ConnectedUnit;

    public void SelectUnit()
    {
        UnitSelection.selectedObjects.Clear();
        UnitSelection.selectedObjects.Add(ConnectedUnit);
        PanelManager.Instance.SelectSingleUnit(ConnectedUnit);
    }

}
