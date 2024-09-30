using MultiplayerKitForHVR.General;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetGrabbable : MonoBehaviour
{
    [SerializeField] GameObject[] ReturnSockets1;
    [SerializeField] GameObject[] ReturnSockets2;
    void OnCollisionEnter(Collision c)
    {
        if (c.transform.CompareTag("TestTube"))
        {
            if (!c.transform.GetComponent<Rigidbody>().useGravity)
            {
                return;
            }
            if (c.transform.GetComponent<Team>().TeamNum == 1)
            {
                foreach (var socket in ReturnSockets1)
                {
                    if (socket.transform.childCount < 1)
                    {
                        socket.GetComponent<NetworkedSocketScript>().ServerGrab(c.transform.GetComponent<PhotonView>().ViewID);
                        break;
                    }
                }
            } else if (c.transform.GetComponent<Team>().TeamNum == 2)
            {
                foreach (var socket in ReturnSockets2)
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
}