using Photon.Pun;
using HurricaneVR.Framework.Core;
using UnityEngine;
using System;
using UnityEngine.UI;


namespace MultiplayerKitForHVR.General
{
    [RequireComponent(typeof(NetworkedSocketScript))]
    public class SocketNetworkBehaviour : MonoBehaviourPun
    {
        NetworkedSocketScript mNetworkedSocketScript;
        public bool IsInitGrab;
        public PhotonView initGrab;

        private void Start()
        {
            /*
            networkManager = NetworkManager.Singleton;
            if(networkManager != null)
            {
                networkManager.OnServerStarted += GrabInitialize;
                networkManager.OnClientStarted += GrabInitialize;
            }
            */

            

        }



        public void GrabInitialize()
        {
            if (IsInitGrab)
            {
                initGrab.gameObject.SetActive(true);
                Debug.Log("servergrab:" + initGrab + " " + transform.name);
                mNetworkedSocketScript.ServerGrab(initGrab.ViewID);
            }
        }

        public void GrabSocket(PhotonView BeGrabbed)
        {
            //initGrab.gameObject.SetActive(true);
            Debug.Log("socket servergrab:" + initGrab + " " + transform.name);
            mNetworkedSocketScript.ServerGrab(BeGrabbed.ViewID);
        }



        private void Awake()
        {
            TryGetComponent(out mNetworkedSocketScript);
            if (IsInitGrab)
            {
                initGrab.gameObject.SetActive(false);
            }
        }


        [PunRPC]
        public void GrabPunRpc(int grabbableId)
        {
            Debug.Log(grabbableId);
            HVRGrabbable grabbable = PhotonView.Find(grabbableId).GetComponent<HVRGrabbable>();
            Debug.Log(grabbable.name);
            mNetworkedSocketScript.BaseTryGrab(grabbable);
        }


        [PunRPC]
        public void OnHoverGrabbableReleasedPunRpc(int grabbableId)
        {
            HVRGrabbable grabbable = PhotonView.Find(grabbableId).GetComponent<HVRGrabbable>();

            mNetworkedSocketScript.BaseOnHoverGrabbableReleased(grabbable);
        }


        [PunRPC]
        public void ForceReleaseServerRpc()
        {
            mNetworkedSocketScript.BaseForceRelease();
        }


        /*
        [ServerRpc(RequireOwnership = false)]
        public void CheckReleaseServerRpc()
        {
            CheckReleaseClientRpc();
        }

        [ClientRpc]
        public void CheckReleaseClientRpc()
        {
            mNetworkedSocketScript.BaseCheckRelease();
        }
        */

        private void OnTriggerEnter(Collider other)
        {
            //if (other.gameObject.name == "SocketBag")
            //{
            //    Outline o = GetComponentInChildren<Outline>();
            //    if (o == null)
            //    {
            //        SocketFollowBehavior sf = GetComponent<SocketFollowBehavior>();
            //        if (sf == null)
            //        {
            //            return;
            //        }
            //        o = sf.FollowObject.GetComponent<Outline>();
            //    }
            //    o.enabled = mNetworkedSocketScript.CanInteract;
            //    if (o.enabled)
            //    {
            //        o.gameObject.AddComponent<OutlineTriggerBehaviour>();
            //    }
            //}
        }
        private void OnTriggerExit(Collider other)
        {
            //if (other.gameObject.name == "SocketBag")
            //{
            //    Outline o = GetComponentInChildren<Outline>();
            //    if (o == null)
            //    {
            //        SocketFollowBehavior sf = GetComponent<SocketFollowBehavior>();
            //        if (sf == null)
            //        {
            //            return;
            //        }
            //        o = sf.FollowObject.GetComponent<Outline>();
            //    }

            //    o.enabled = false;
            //    if (o.gameObject.TryGetComponent<OutlineTriggerBehaviour>(out var ot))
            //    {
            //        Destroy(ot);
            //    }
            //}

        }

        public void UnSetGrabbableKinematic(GameObject grabbable)
        {
            int grabbableId = grabbable.GetComponent<PhotonView>().ViewID;
            this.photonView.RPC("UnSetGrabbableKinematicPunRpc", RpcTarget.All, grabbableId);
        }

        [PunRPC]
        public void UnSetGrabbableKinematicPunRpc(int grabbableId)
        {
            GameObject grabbable = PhotonView.Find(grabbableId).gameObject;
            grabbable.GetComponent<Rigidbody>().isKinematic = false;
            grabbable.GetComponent<Rigidbody>().useGravity = true;
        }


        public void SetGrabbableKinematic(GameObject grabbable)
        {
            int grabbableId = grabbable.GetComponent<PhotonView>().ViewID;
            this.photonView.RPC("SetGrabbableKinematicPunRpc", RpcTarget.All,grabbableId);
            
        }

        [PunRPC]
        public void SetGrabbableKinematicPunRpc(int grabbableId)
        {
            GameObject grabbable = PhotonView.Find(grabbableId).gameObject;
            grabbable.GetComponent<Rigidbody>().isKinematic = true;
            grabbable.GetComponent<Rigidbody>().useGravity = false;
        }

    }
}