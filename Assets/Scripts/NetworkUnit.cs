using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkUnit : NetworkBehaviour
{
    private static Dictionary<string, NetworkUnit> units;

    public static Dictionary<string, NetworkUnit> Units
    {
        get { return units ?? (units = new Dictionary<string, NetworkUnit>()); }
    }

    public string UnitID;

    public override void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        base.OnPhotonInstantiate(info);
        UnitID = photonView.instantiationData[0] as string;
        if(Units.ContainsKey(UnitID))
            return;
        
        Units.Add(UnitID,this);
    }

    protected override void NetworkUpdate()
    {

    }

}
