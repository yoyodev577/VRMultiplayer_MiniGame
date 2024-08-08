using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Hammer : MonoBehaviour
{
    public PhotonView view;
    public Vector3 startPos;
    public Quaternion startRot;
    public NetworkedGrabbing networkedGrabbing;
    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
        networkedGrabbing = GetComponent<NetworkedGrabbing>();
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
        transform.position = startPos;
        transform.rotation = startRot;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            if (PhotonNetwork.IsConnected)
                view.RPC("PhotonResetPosition", RpcTarget.All);
        }
    }
}
