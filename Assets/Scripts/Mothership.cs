using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Mothership : NetworkUnit
{


    public MothershipSlot[] Slots;
    public TurretSlot[] TurretSlots;


    void OnDestroy()
    {
        GameObject.Find("ENDGAME").GetComponent<GameOver>().OnGameOver(photonView.isMine);
    }

    public override void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        base.OnPhotonInstantiate(info);

        Slots = GetComponentsInChildren<MothershipSlot>();
        TurretSlots = GetComponentsInChildren<TurretSlot>();

        foreach (var slot in Slots)
        {
            slot.gameObject.SetActive(false);
        }

        foreach (var slot in TurretSlots)
        {
            slot.gameObject.SetActive(false);
        }

        foreach (var renderer in GetComponentsInChildren<MeshRenderer>())
        {
            renderer.material.SetColor("_EmissionColor", photonView.isMine ? Color.red : Color.blue);
        }

        if (!photonView.isMine)
            return;

        MothershipSlotPanel.Instance.MothershipObject = this;
        MothershipSlotPanel.Instance.Initialize();
    }

    public void ActivateNextSlot(GameObject slotObject)
    {
        for (int i = 0; i < Slots.Length; i++)
        {
            var slot = Slots[i];
            if (!slot.gameObject.activeSelf)
            {
                slot.gameObject.SetActive(true);
                slot.SlotObject = slotObject.GetComponent<MothershipSlotObject>();
                slotObject.transform.parent = slot.transform;
                slotObject.transform.position = slot.transform.position;
                slotObject.transform.rotation = slot.transform.rotation;

                if (photonView.isMine)
                {
                    //MothershipSlotPanel.Instance.GetComponent<GridLayoutGroup>().GetComponentsInChildren<Image>()[i].color = Color.green;
                    slot.SlotObject.ParentSlot = slot;
                }
                break;
            }
        }
    }

    public void ActivateNextTurret(GameObject slotObject)
    {
        for (int i = 0; i < TurretSlots.Length; i++)
        {
            var slot = TurretSlots[i];
            if (!slot.gameObject.activeSelf)
            {
                slot.gameObject.SetActive(true);
                slotObject.transform.parent = slot.transform;
                slotObject.transform.position = slot.transform.position;
                slotObject.transform.rotation = slot.transform.rotation;

                if (photonView.isMine)
                {

                }
                break;
            }
        }
    }
}
