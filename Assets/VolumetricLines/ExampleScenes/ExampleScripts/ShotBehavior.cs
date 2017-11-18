using UnityEngine;
public class 
    ShotBehavior : MonoBehaviour {

    public GameObject target;
    // Use this for initialization
    void Start()
    {
        Invoke("Die", 7);
    }

    private void Die()
    {
        if (GetComponent<PhotonView>().isMine)
            PhotonNetwork.Destroy(this.gameObject);
    }
    void OnTriggerEnter(Collider other)
    {
        if (target == null)
            return;

        if(other.GetComponent<Collider>() == target.GetComponent<Collider>())
        {

            if(GetComponent<PhotonView>().isMine)
            {
                PlayerController.instance.HitExplosion(target);
                PhotonNetwork.Destroy(this.gameObject);
            }

        }
    }
}
