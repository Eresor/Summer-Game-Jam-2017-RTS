using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingButton : MonoBehaviour
{
    public MothershipSlotPanel SlotPanel;
    public string SlotPrefabName;
    public int RequiredGold;
    public Sprite BuildingSprite;

    private Image image;
    void Start()
    {
        GetComponentInChildren<Text>().text = GetComponentInChildren<Text>().text + "\n(0/" + RequiredGold + ")";
        image = GetComponent<Image>();
    }

    void Update()
    {
        if (!ResourceManager.Instance)
            return;

        image.color = RequiredGold > ResourceManager.Instance.Gold ? Color.black : Color.white;
    }

    public void Build()
    {
        if(RequiredGold > ResourceManager.Instance.Gold)
            return;

        bool foundEmpty = false;
        int counter = 0;
        foreach (var slot in SlotPanel.MothershipObject.Slots)
        {
            if (slot.SlotObject == null)
                foundEmpty = true;
            else
                ++counter;

        }

        if(!foundEmpty)
            return;

        ResourceManager.Instance.Gold -= RequiredGold;
        var text = (Time.time + Random.Range(0, float.MaxValue)).ToString();
        string hash = Hash128.Parse(text).ToString();
        var instance = PhotonNetwork.Instantiate(SlotPrefabName, Vector3.zero, Quaternion.identity, 0, new object[] { hash });
        var networkUnit = instance.GetComponent<NetworkUnit>();

        SlotPanel.GetComponent<GridLayoutGroup>().GetComponentsInChildren<Image>()[counter].sprite = BuildingSprite;

        PhotonNetwork.RPC(UnitsNetworkManager.Instance.photonView, "InsertToSlot",PhotonTargets.All,false, new object[] { SlotPanel.MothershipObject.UnitID, networkUnit.UnitID });
    }

}
