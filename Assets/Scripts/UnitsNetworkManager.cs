using System.Collections;
using System.Collections.Generic;
using RTS_Cam;
using UnityEngine;
using UnityEngine.Networking.Types;

public class UnitsNetworkManager : NetworkBehaviour
{
    public static UnitsNetworkManager Instance;
    protected override void NetworkStart()
    {
        Instance = this;
        var value = Time.time.ToString() + Random.Range(0, float.MaxValue);
        string hash = Hash128.Parse(value).ToString();
        var mothership = PhotonNetwork.Instantiate("Mothership",PhotonNetwork.isMasterClient ? new Vector3(-100f,-30f,-100f) : new Vector3(100f,-30f,100f),Quaternion.identity, 0, new object[] { hash });
        Camera.main.transform.position = mothership.transform.position + new Vector3(0f,40f,0f);
        Camera.main.GetComponent<RTS_Camera>().SetTarget(mothership.transform);
        Invoke("StopFollow",2f);
    }

    void StopFollow()
    {
        Camera.main.GetComponent<RTS_Camera>().ResetTarget();
    }

  

    [PunRPC]
    public void InsertToSlot(string mothershipHash, string insertObjectHash)
    {
        var mothership = NetworkUnit.Units[mothershipHash];
        var child = NetworkUnit.Units[insertObjectHash];
        mothership.GetComponent<Mothership>().ActivateNextSlot(child.gameObject);
    }

    [PunRPC]
    public void TurretToSlot(string mothershipHash, string insertObjectHash)
    {
        var mothership = NetworkUnit.Units[mothershipHash];
        var child = NetworkUnit.Units[insertObjectHash];
        mothership.GetComponent<Mothership>().ActivateNextTurret(child.gameObject);
    }

    [PunRPC]
    public void MoveToPoint(string unitHash, Vector3 destination, PhotonMessageInfo info)
    {
        var obj = NetworkUnit.Units[unitHash].GetComponent<MoveableUnit>();
        obj.MoveToPoint(destination);

    }

    [PunRPC]
    public void CollectResources(string unitHash, Vector3 asteroidPosition)
    {
        var obj = NetworkUnit.Units[unitHash].GetComponent<MoveableUnit>();
        obj.CollectResources(asteroidPosition);
    }


    [PunRPC]
    public void StopUnitMovement(string unitHash, PhotonMessageInfo info)
    {
        var obj = NetworkUnit.Units[unitHash].GetComponent<MoveableUnit>();
        obj.StopMoving();
    }

    [PunRPC]
    public void AttackCommandStart(string unitHash, string unit, PhotonMessageInfo info)
    {
        var obj = NetworkUnit.Units[unitHash].GetComponent<DestructableUnit>();
        var attackedUnit = NetworkUnit.Units[unit].GetComponent<DestructableUnit>();
        obj.AttackEnemy(attackedUnit);
    }

    [PunRPC]
    public void AttackCommandStop(string unitHash, PhotonMessageInfo info)
    {
        var obj = NetworkUnit.Units[unitHash].GetComponent<DestructableUnit>();
        obj.StopAttacking();
    }
    

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
