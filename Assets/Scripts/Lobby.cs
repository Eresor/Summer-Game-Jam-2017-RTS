using System.Collections;
using System.Collections.Generic;
using Photon;
using UnityEngine;

public class Lobby : PunBehaviour {

	// Use this for initialization
	void Start ()
	{
	    PhotonNetwork.ConnectUsingSettings("v1.0");
	}

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("Joined lobby");

        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 2;
        options.IsVisible = true;
        PhotonNetwork.automaticallySyncScene = true;
        PhotonNetwork.JoinOrCreateRoom("main1234", options, TypedLobby.Default);
    }


    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        if(PhotonNetwork.isMasterClient)
            PhotonNetwork.LoadLevel("Main");
    }
}
