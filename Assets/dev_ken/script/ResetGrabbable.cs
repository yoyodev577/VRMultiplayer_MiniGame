using MultiplayerKitForHVR.General;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetGrabbable : MonoBehaviour
{
    [SerializeField] GameObject[] ReturnSockets;
    void OnCollisionEnter(Collision c)
    {
        if (c.transform.CompareTag("TestTube"))
        {
            if (!c.transform.GetComponent<Rigidbody>().useGravity)
            {
                return;
            }
            foreach (var socket in ReturnSockets)
            {
                if (socket.transform.childCount < 1)
                {
                    socket.GetComponent<NetworkedSocketScript>().ServerGrab(c.transform.GetComponent<PhotonView>().ViewID);
                    break;
                }
            }
        }
    }
}
