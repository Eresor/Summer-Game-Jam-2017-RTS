using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        Invoke("Die", 4);
    }

    private void Die()
    {

        if (this.gameObject != null)
            Destroy(this.gameObject);
    }
}

