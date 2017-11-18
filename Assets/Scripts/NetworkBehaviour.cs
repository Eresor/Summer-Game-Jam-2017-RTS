using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkBehaviour : NetworkBehaviourBase {


    private bool CheckRoomReady()
    {
        return PhotonNetwork.room.PlayerCount == 2;
    }

    public bool IsRoomReady { get; private set; }


    IEnumerator WaitForNetworkCoroutine(Action action)
    {
        yield return new WaitUntil(CheckRoomReady);
        action();
    }

    protected sealed override void Start()
    {
        StartCoroutine(WaitForNetworkCoroutine(NetworkStart));
    }

    protected sealed override void Awake()
    {
        StartCoroutine(WaitForNetworkCoroutine(NetworkAwake));
    }

    protected sealed override void OnEnable()
    {
        StartCoroutine(WaitForNetworkCoroutine(NetworkOnEnable));
    }

    protected sealed override void OnDisable()
    {
        StartCoroutine(WaitForNetworkCoroutine(NetworkOnDisable));
    }

    protected sealed override void Update()
    {
        if(!IsRoomReady)
            return;
        
        NetworkUpdate();
    }

    protected virtual void NetworkAwake()
    {

    }

    protected virtual void NetworkStart()
    {

    }

    protected virtual void NetworkOnEnable()
    {
        IsRoomReady = true;
    }

    protected virtual void NetworkOnDisable()
    {
        
    }

    protected virtual void NetworkUpdate () {
		
	}
}
