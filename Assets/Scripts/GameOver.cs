using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {

    public void OnGameOver(bool loose)
    {
        StartCoroutine("OverCoroutine");
        GetComponentInChildren<Text>().text = loose ? "You loose!" : "You won!";
    }

    IEnumerator OverCoroutine()
    {
        var image = GetComponentInChildren<Image>();
        for (float a = 0; a < 200f; a += Time.deltaTime)
        {
            var c = image.color;
            c.a = a;
            image.color = c;
            yield return null;
        }
    }

}
