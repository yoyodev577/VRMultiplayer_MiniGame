using System;
using UnityEngine;
using Photon.Pun;
using HurricaneVR.Framework.Core;

[RequireComponent(typeof(PhotonView))]
public class NetworkChangeOwnership : MonoBehaviourPunCallbacks
{
    //public NetworkVariable<ulong> ClientIdOfUser = new NetworkVariable<ulong>();
    public int ClientIdOfUser; 
    public bool InUse = false;

    private PhotonView mNetworkObject;

    private void Awake()
    {
        mNetworkObject = GetComponent<PhotonView>();
    }

    public override void OnJoinedRoom()
    {
        InUse = false;
    }

    public void SetOwnership()
    {
        //ChangeOwnershipPunRpc(new ServerRpcParams());

        mNetworkObject.TransferOwnership(PhotonNetwork.LocalPlayer);

        //mNetworkObject.gameObject.layer = LayerMask.NameToLayer("Grabbables");
        /*
        foreach (var child in mNetworkObject.gameObject.GetComponentsInChildren<Transform>(includeInactive: true))
        {
            child.gameObject.layer = LayerMask.NameToLayer("Grabbables");
        }*/
    }
    //public void GrabSetOwnership()
    //{
    //    GrabChangeOwnershipServerRpc(new ServerRpcParams());
    //    //mNetworkObject.gameObject.layer = LayerMask.NameToLayer("Grabbables");
    //    /*
    //    foreach (var child in mNetworkObject.gameObject.GetComponentsInChildren<Transform>(includeInactive: true))
    //    {
    //        child.gameObject.layer = LayerMask.NameToLayer("Grabbables");
    //    }*/
    //}
    public void ResetOwnership()
    {
        /*
        if (GetComponent<HoldStatus>())
        {
            if (GetComponent<HoldStatus>().SyncHandHold.Value)
            {
                return;
            }
        }
        if (GetComponent<MiceStatus>())
        {
            if (GetComponent<MiceStatus>().SyncHandHold.Value)
            {
                return;
            }
        }
        */

        //mNetworkObject.TransferOwnership(-1);


        //ResetOwnershipServerRpc(new ServerRpcParams());
        //mNetworkObject.gameObject.layer = LayerMask.NameToLayer("Default");
        /*
        foreach (var child in mNetworkObject.gameObject.GetComponentsInChildren<Transform>(includeInactive: true))
        {
            child.gameObject.layer = LayerMask.NameToLayer("Default");
        }*/
    }

    //[PunRPC]
    //public void ChangeOwnershipPunRpc(ServerRpcParams serverRpcParams = default)
    //{
    //    if (InUse.Value) return;

    //    ClientIdOfUser.Value = serverRpcParams.Receive.SenderClientId;
    //    InUse.Value = true;
    //    mNetworkObject.TransferOwnership() .ChangeOwnership(ClientIdOfUser.Value);
    //}
    //[ServerRpc(RequireOwnership = false)]
    //public void GrabChangeOwnershipServerRpc(ServerRpcParams serverRpcParams = default)
    //{

    //    ClientIdOfUser.Value = serverRpcParams.Receive.SenderClientId;
    //    InUse.Value = true;
    //    mNetworkObject.ChangeOwnership(ClientIdOfUser.Value);
    //}

    //[ServerRpc(RequireOwnership = false)]
    //public void ChangeOwnershipByIdServerRpc(ulong clientId)
    //{
    //    ClientIdOfUser.Value = clientId;
    //    mNetworkObject.ChangeOwnership(clientId);
    //}

    //[ServerRpc(RequireOwnership = false)]
    //public void ResetOwnershipServerRpc(ServerRpcParams serverRpcParams = default)
    //{
    //    if (ClientIdOfUser.Value != serverRpcParams.Receive.SenderClientId) return;

    //    ClientIdOfUser.Value = ulong.MaxValue;
    //    InUse.Value = false;
    //}

    //[ServerRpc(RequireOwnership = false)]
    //public void SetServerAsOwnerServerRpc()
    //{
    //    GetComponent<NetworkObject>().ChangeOwnership(0);
    //}

    //[ServerRpc(RequireOwnership = false)]
    //public void ForceUnUseServerRpc()
    //{
    //    InUse.Value = false;
    //}
    /*
    private void OnCollisionEnter(Collision collision)
    {
        var nco = collision.gameObject.GetComponent<NetworkChangeOwnership>();

        if (GetComponent<HoldStatus>())
        {
            if (GetComponent<HoldStatus>().SyncHandHold.Value)
            {
                return;
            }
        }
        if (GetComponent<MiceStatus>())
        {
            if (GetComponent<MiceStatus>().SyncHandHold.Value)
            {
                return;
            }
        }
        if (nco == null ||nco.InUse.Value || !InUse.Value) 
            return;

        nco.ChangeOwnershipByIdServerRpc(OwnerClientId);
    }
    */
}
