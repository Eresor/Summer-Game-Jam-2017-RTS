using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitsButton : MonoBehaviour
{

    public string ObjectName;
    public int RequiredManpower;
    public int RequiredGold;
    public int RequiredBarracksLevel;

    private Image image;
    void Start()
    {
        GetComponentInChildren<Text>().text = GetComponentInChildren<Text>().text + "\n(" + RequiredManpower + "/" +
                                              RequiredGold + "/" + (RequiredBarracksLevel == 0 ? "SH)" : "H)");

        image = GetComponent<Image>();
    }


    void Update()
    {
        if(!ResourceManager.Instance)
            return;

        image.color = RequiredGold > ResourceManager.Instance.Gold ||
                      RequiredManpower > ResourceManager.Instance.Manpower
            ? Color.black
            : Color.white;
    }

}
