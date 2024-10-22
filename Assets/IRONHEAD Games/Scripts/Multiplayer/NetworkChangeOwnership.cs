using System;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using HurricaneVR.Framework.Core;

[RequireComponent(typeof(PhotonView))]
public class NetworkChangeOwnership : MonoBehaviourPunCallbacks, IPunOwnershipCallbacks
{
    //public NetworkVariable<ulong> ClientIdOfUser = new NetworkVariable<ulong>();
    public int ClientIdOfUser; 
    public bool InUse = false;


    private void Awake()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDestroy()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        if (targetView != base.photonView)
            return;
        base.photonView.TransferOwnership(requestingPlayer);
    }

    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
        if (targetView != base.photonView)
            return;
    }

    public void OnOwnershipTransferFailed(PhotonView targetView, Player previousOwner)
    {
        
    }







    public override void OnJoinedRoom()
    {
        //InUse = false;
        ClientIdOfUser = PhotonNetwork.LocalPlayer.ActorNumber;
    }

    public void SetOwnership()
    {
        //ChangeOwnershipPunRpc(new ServerRpcParams());

        base.photonView.RequestOwnership();
        Debug.Log("Owner has Changed to Player:" + ClientIdOfUser);
        InUse = true;
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

        //base.photonView.TransferOwnership(-1);
        InUse = false;

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
