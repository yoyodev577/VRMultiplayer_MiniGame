using Photon.Pun;
using HurricaneVR.Framework.Core.Grabbers;
using HurricaneVR.Framework.Core;
using UnityEngine;


namespace MultiplayerKitForHVR.General
{
    [RequireComponent(typeof(SocketNetworkBehaviour))]
    public class NetworkedSocketScript : HVRSocket
    {
        private SocketNetworkBehaviour mSocketNetworkBehaviour;
        public bool isStatic;
        private PhotonView m_PhotonView;



        protected override void Awake()
        {
            base.Awake();
            mSocketNetworkBehaviour = GetComponent<SocketNetworkBehaviour>();
            m_PhotonView = GetComponent<PhotonView>();
        }


        protected override void OnHoverGrabbableReleased(HVRGrabberBase grabber, HVRGrabbable grabbable)
        {
            int grabbableId = grabbable.GetComponent<PhotonView>().ViewID;
            //GetComponent<SocketNetworkBehaviour>().OnHoverGrabbableReleasedPunRpc(grabbableId);
            m_PhotonView.RPC("OnHoverGrabbableReleasedPunRpc", RpcTarget.All, grabbableId);
        }

        public void BaseOnHoverGrabbableReleased(HVRGrabbable grabbable)
        {
            base.OnHoverGrabbableReleased(GetComponent<HVRGrabberBase>(), grabbable);

            grabbable.GetComponent<NetworkChangeOwnership>().ResetOwnership();
        }

        public override bool TryGrab(HVRGrabbable grabbable, bool force = false)
        {
            int grabbableId = grabbable.GetComponent<PhotonView>().ViewID;

            m_PhotonView.RPC("GrabPunRpc", RpcTarget.All, grabbableId);
            //mSocketNetworkBehaviour.GrabPunRpc(grabbableId);
            return true;
        }

        public void ServerGrab()
        {
            HVRGrabbable grabbable = GrabbedTarget;
            int grabbableId = grabbable.GetComponent<PhotonView>().ViewID;
            //mSocketNetworkBehaviour.GrabPunRpc(grabbableId);
            m_PhotonView.RPC("GrabPunRpc", RpcTarget.All, grabbableId);

        }

        public void ServerGrab(int grabbableId)
        {
            //mSocketNetworkBehaviour.GrabPunRpc(grabbableId);
            m_PhotonView.RPC("GrabPunRpc", RpcTarget.All, grabbableId);
        }

        public bool BaseTryGrab(HVRGrabbable grabbable)
        {

            mSocketNetworkBehaviour.SetGrabbableKinematic(grabbable.gameObject);
            grabbable.gameObject.transform.position = this.transform.position;

            //if (TryGetComponent<SocketFollowBehavior>(out var sfb))
            //{
            //    sfb.TargetObject = grabbable.gameObject;
            //    sfb.FollowObject.SetActive(true);
            //    sfb.TargetObject.transform.localScale = sfb.FollowObject.transform.localScale;
            //    sfb.TargetObject.SetActive(false);

            //    //sfb.start = true;
            //    //sfb.FollowObject.GetComponent<BoxCollider>().enabled = true;
            //}
            return base.TryGrab(grabbable);
        }

        public override void ForceRelease()
        {
            //mSocketNetworkBehaviour.ForceReleaseServerRpc();
            m_PhotonView.RPC("ForceReleaseServerRpc", RpcTarget.All);
        }

        public void BaseForceRelease()
        {
            HVRGrabbable grabbable = GrabbedTarget;

            if (grabbable.TryGetComponent<NetworkChangeOwnership>(out var nco))
            {
                nco.ResetOwnership();
                //nco.SetOwnership();
            }
            else
            {
                Debug.LogError($"{nameof(BaseForceRelease)}: Grabbable is missing a {nameof(NetworkChangeOwnership)} component");
            }

            //if (TryGetComponent<SocketFollowBehavior>(out var sfb))
            //{
            //    //sfb.start = false;
            //    sfb.FollowObject.SetActive(false);
            //    sfb.TargetObject.SetActive(true);
            //    sfb.TargetObject = sfb.FollowObject;
            //    //sfb.FollowObject.GetComponent<BoxCollider>().enabled = false;
            //}

            base.ForceRelease();

            mSocketNetworkBehaviour.UnSetGrabbableKinematic(grabbable.gameObject);

            // if (grabbable.TryGetComponent(out Rigidbody rb))
            // {
            //     rb.isKinematic = false;
            //     rb.useGravity = true;

            // }
            // else
            // {
            //     Debug.LogError($"{nameof(BaseForceRelease)}: Grabbable is missing a {nameof(Rigidbody)} component");
            // }
        }

        protected override void CheckRelease()
        {
            if (IsGrabbing)
            {
                if (!IsHoldActive)
                    m_PhotonView.RPC("ForceReleaseServerRpc", RpcTarget.All);
                    //GetComponent<SocketNetworkBehaviour>().ForceReleaseServerRpc();

                //ReleaseGrabbable(this, GrabbedTarget);
            }
            //GetComponent<SocketNetworkBehaviour>().CheckReleaseServerRpc();
        }

        /*
        public void BaseCheckRelease()
        {
            base.CheckRelease();
        }
        */
    }
}