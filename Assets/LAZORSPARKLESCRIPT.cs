using UnityEngine;


public class LAZORSPARKLESCRIPT : MonoBehaviour {

	// Use this for initialization
	void Start () { 
            Invoke("Die", 1);
    }
	
    private void Die()
    {
  
            if(this.gameObject != null)
            Destroy(this.gameObject);
    }
}
