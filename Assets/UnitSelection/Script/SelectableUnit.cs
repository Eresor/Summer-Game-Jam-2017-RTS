using UnityEngine;

public class SelectableUnit : MonoBehaviour{

    public Projector selectionMark;
    public string UnitName = "";
	void Start(){
		UnitSelection.suc.Add(this);
	}

	void OnDestroy(){
		UnitSelection.suc.Remove(this);
	}


}