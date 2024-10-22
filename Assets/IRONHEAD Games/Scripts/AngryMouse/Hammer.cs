using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using HurricaneVR.Framework.Core;
using HurricaneVR.Framework.ControllerInput;

public class Hammer : MonoBehaviour
{
    public PhotonView view;
    public Vector3 startPos;
    public Quaternion startRot;
    public HVRGrabbable networkedGrabbing;
    public Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
        networkedGrabbing = GetComponent<HVRGrabbable>();
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;
        startRot = transform.rotation;
    }

    public void ResetPos()
    {
        if (PhotonNetwork.IsConnected)
            view.RPC("PhotonResetPosition", RpcTarget.All);
    }

    [PunRPC]
    public void PhotonResetPosition()
    {
        rb.isKinematic = true;
        transform.position = startPos;
        transform.rotation = startRot;
        rb.isKinematic = false;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject != null && !networkedGrabbing.IsHandGrabbed)
        {
            if (PhotonNetwork.IsConnected)
                view.RPC("PhotonResetPosition", RpcTarget.All);
        }
    }
}
