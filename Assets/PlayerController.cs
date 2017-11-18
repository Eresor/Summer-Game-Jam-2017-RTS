using UnityEngine;

public class PlayerController : MonoBehaviour {
    public static PlayerController instance;
    RaycastHit[] m_HitInfo;
    DestructableUnit lastClickedUnit;
    // Use this for initialization
    void Start () {
        instance = this;
     
	}


    public void HitExplosion(GameObject target)
    {
        var value = Time.time.ToString() + Random.Range(0, float.MaxValue);
        string hash = Hash128.Parse(value).ToString();
        PhotonNetwork.Instantiate("laserSparkle", target.transform.position, Quaternion.LookRotation(target.transform.forward), 0, new object[] { hash });
    }

        

	// Update is called once per frame
	void Update ()
	{

        if(Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

	    Resource res;
        if (!RightClick())
            return;

        if (IsEnemyRightClicked())
        {
            this.UnitsAttack(lastClickedUnit);
        }
        else if ((res = ResourceClicked()) != null)
        {
            this.CollectResources();
            this.UnitsStopAttack();
        }
        else
        {
             this.UnitsMovement();
             this.UnitsStopAttack();
        }
	}

    public void StopUnitMovement(DestructableUnit unit)
    {
        Debug.Log("Stop movement");
        PhotonNetwork.RPC(UnitsNetworkManager.Instance.photonView, "StopUnitMovement", PhotonTargets.All, false, new object[] { unit.GetComponent<NetworkUnit>().UnitID});
    }
    public void MoveUnit(DestructableUnit unit, Vector3 position)
    {
        PhotonNetwork.RPC(UnitsNetworkManager.Instance.photonView, "MoveToPoint", PhotonTargets.All, false, new object[] { unit.GetComponent<NetworkUnit>().UnitID, position });
    }

    public void CollectResources()
    {
        foreach (var selectedObject in UnitSelection.selectedObjects)
        {
            var mu = selectedObject.GetComponent<MoveableUnit>();
            if (mu && mu.CanCollectResources)
                PhotonNetwork.RPC(UnitsNetworkManager.Instance.photonView, "CollectResources", PhotonTargets.All, false, new object[] { selectedObject.GetComponent<NetworkUnit>().UnitID, m_HitInfo[0].point });
        }
    }


    public void UnitsStopAttack()
    {
        foreach (var unit in UnitSelection.selectedObjects)
        {
            UnitStopAttack(unit.GetComponent<DestructableUnit>());
        }
    }
    public void UnitStopAttack(DestructableUnit unit)
    {
        PhotonNetwork.RPC(UnitsNetworkManager.Instance.photonView, "AttackCommandStop", PhotonTargets.All, false, new object[] { unit.GetComponent<NetworkUnit>().UnitID});
    }


    bool RightClick()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            
            if ((m_HitInfo = Physics.RaycastAll(ray)).Length>0)
            {
                foreach (var hit in m_HitInfo)
                {
                    if (hit.transform != null)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
    bool IsEnemyRightClicked()
    {
        foreach (var raycastHit in m_HitInfo)
        {
            if (raycastHit.transform.GetComponent<DestructableUnit>() != null && !raycastHit.transform.GetComponent<PhotonView>().isMine)
            {
                lastClickedUnit = raycastHit.transform.GetComponent<DestructableUnit>();
                return true;
            }
        }
        return false;
    }

    Resource ResourceClicked()
    {
        foreach (var raycastHit in m_HitInfo)
        {
            if (raycastHit.transform.GetComponent<Resource>() != null)
            {
                return raycastHit.transform.GetComponent<Resource>();
            }
        }
        return null;
    }

    private void UnitsMovement()
    {
        foreach (var unit in UnitSelection.selectedObjects)
        {
            if(m_HitInfo.Length>0)
                PhotonNetwork.RPC(UnitsNetworkManager.Instance.photonView, "MoveToPoint", PhotonTargets.All, false, new object[] { unit.GetComponent<NetworkUnit>().UnitID, m_HitInfo[0].point });
        }
    }
    private void UnitsAttack(DestructableUnit unitToAttack)
    {
        foreach (var unit in UnitSelection.selectedObjects)
        {
            PhotonNetwork.RPC(UnitsNetworkManager.Instance.photonView, "AttackCommandStart", PhotonTargets.All, false, new object[] { unit.GetComponent<NetworkUnit>().UnitID, unitToAttack.GetComponent<NetworkUnit>().UnitID });
        }
    }

}