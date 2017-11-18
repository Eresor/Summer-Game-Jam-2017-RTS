using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Barracks : MothershipSlotObject
{
    public int Level = 10;
    public float Progress;
    public string ObjectToBuild = "";
    private string temporaryObjectName;
    public Text TextToUpdate;
    private GameObject spawnEffect;
    private Vector3 spawnPos;

    public override void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        base.OnPhotonInstantiate(info);
        BarracksManager.Instance.BarracksList.Add(this);
    }

    void LateSpawn()
    {
        if(spawnEffect)
            PhotonNetwork.Destroy(spawnEffect);

        var value = Time.time.ToString() + Random.Range(0, float.MaxValue);
        string hash = Hash128.Parse(value).ToString();
        //var spawnPos = MothershipSlotPanel.Instance.MothershipObject.transform.position + new Vector3(0f,30f,0f);
        var newShip = PhotonNetwork.Instantiate(temporaryObjectName, spawnPos, Quaternion.identity, 0, new object[] { hash });
        foreach (var renderer in GetComponentsInChildren<MeshRenderer>())
        {
            renderer.material.SetColor("_EmissionColor", photonView.isMine ? Color.red : Color.blue);
        }
    }

    public void UpdateBarracks()
    {
        if(ObjectToBuild=="")
            return;

        Progress += Time.deltaTime / 10f;

        if (Progress >= 1.0f)
        {
            var randVec = Random.insideUnitCircle * 50f;
            spawnPos = MothershipSlotPanel.Instance.MothershipObject.transform.position + new Vector3(randVec.x, 30f, randVec.y);
            spawnEffect = PhotonNetwork.Instantiate("SpawnEffect", spawnPos, Quaternion.identity, 0);
            Invoke("LateSpawn",3f);
            Progress = 0f;
            temporaryObjectName = ObjectToBuild;
            ObjectToBuild = "";

        }

        if (!TextToUpdate)
        {
            TextToUpdate = ParentSlot.SlotPanelElement.transform.Find("Text").GetComponent<Text>();
        }
        TextToUpdate.text = (100f * Progress).ToString("00.00");
    }
}
