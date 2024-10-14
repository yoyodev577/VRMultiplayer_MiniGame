using HurricaneVR.Framework.Core;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldStatus : MonoBehaviour
{
    private bool ClientHold = false;
    public bool SyncHold = false;
    private bool ClientHandHold = false;
    public bool SyncHandHold = false;
    private HVRGrabbable grabbable;
    private PhotonView m_PhotonView;

    private void Start()
    {
        grabbable = GetComponent<HVRGrabbable>();
        m_PhotonView = GetComponent<PhotonView>();
    }
    private void Update()
    {
        if (grabbable.IsBeingHeld)
        {
            ClientHold = true;
        }
        if (ClientHold && !SyncHold)
        {
            m_PhotonView.RPC("SetSyncHoldServerRpc", RpcTarget.All, true);
            //SetSyncHoldServerRpc(true);
        }
        if (ClientHold && !grabbable.IsBeingHeld)
        {
            ClientHold = false;
            m_PhotonView.RPC("SetSyncHoldServerRpc", RpcTarget.All, false);
            //SetSyncHoldServerRpc(false);
        }

        if (grabbable.IsHandGrabbed)
        {
            ClientHandHold = true;
        }
        if (ClientHandHold && !SyncHandHold)
        {
            m_PhotonView.RPC("SetSyncHandHoldServerRpc", RpcTarget.All, true);
            //SetSyncHandHoldServerRpc(true);
        }
        if (ClientHandHold && !grabbable.IsHandGrabbed)
        {
            m_PhotonView.RPC("SetSyncHandHoldServerRpc", RpcTarget.All, false);
            //SetSyncHandHoldServerRpc(false);
        }
    }
    [PunRPC]
    public void SetSyncHoldServerRpc(bool status)
    {
        SyncHold = status;
    }
    [PunRPC]
    public void SetSyncHandHoldServerRpc(bool status)
    {
        SyncHandHold = status;
    }
}
