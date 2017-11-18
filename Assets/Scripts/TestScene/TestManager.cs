using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class TestManager : NetworkBehaviour
{

    public static TestManager Instance;

    public GameObject Bullet;

    [PunRPC]
    public void BulletSpawned(string unitHash, string targetHash, PhotonMessageInfo info)
    {
        var shooter = NetworkUnit.Units[unitHash];
        var target = NetworkUnit.Units[targetHash];


        var bullet = Instantiate(Bullet, shooter.transform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody>().velocity = (target.transform.position - shooter.transform.position).normalized;

    }
    protected override void NetworkStart()
    {
        Instance = this;
        string hash = Hash128.Parse(Time.time.ToString() + Random.Range(0, float.MaxValue)).ToString();
        PhotonNetwork.Instantiate("Cube", Vector3.zero, Quaternion.identity,0,new object[] { hash });
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }
}
