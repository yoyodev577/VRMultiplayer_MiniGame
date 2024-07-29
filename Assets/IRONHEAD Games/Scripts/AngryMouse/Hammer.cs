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

    // Update is called once per frame
    void Update()
    {

    }

    [PunRPC]
    public void ResetPosition()
    {
        transform.position = startPos;
        transform.rotation = startRot;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            ResetPosition();
            if (PhotonNetwork.IsConnected)
               view.RPC("ResetPosition", RpcTarget.All);

        }
    }
}
