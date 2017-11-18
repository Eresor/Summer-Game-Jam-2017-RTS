using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TurretButton : MonoBehaviour {


    public void BuildTurret()
    {
        bool foundEmpty = false;
        int counter = 0;
        foreach (var slot in MothershipSlotPanel.Instance.MothershipObject.TurretSlots)
        {
            if (!slot.gameObject.activeSelf)
                foundEmpty = true;
            else
                counter++;
        }

        if (!foundEmpty)
            return;

        GetComponentInChildren<Text>().text = "Turrets: " + counter;
        var text = (Time.time + Random.Range(0, float.MaxValue)).ToString();
        string hash = Hash128.Parse(text).ToString();
        var instance = PhotonNetwork.Instantiate("Turret", Vector3.zero, Quaternion.identity, 0, new object[] { hash });
        var networkUnit = instance.GetComponent<NetworkUnit>();
        PhotonNetwork.RPC(UnitsNetworkManager.Instance.photonView, "TurretToSlot", PhotonTargets.All, false, new object[] { MothershipSlotPanel.Instance.MothershipObject.UnitID, networkUnit.UnitID });
    }

}
