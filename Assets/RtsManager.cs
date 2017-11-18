using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RtsManager : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
            Transform camT = Camera.main.transform;
        this.SetCameraXRotation(camT, 45f);
        
    }


    



// Update is called once per frame
    void Update ()
    {
		
	}


    // Sets camera angle
    private void SetCameraXRotation(Transform t, float angle)
    {
        t.localEulerAngles = new Vector3(angle, t.localEulerAngles.y, t.localEulerAngles.z);
    }

}



