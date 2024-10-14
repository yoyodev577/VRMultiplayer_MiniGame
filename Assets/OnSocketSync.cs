using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class OnSocketSync : MonoBehaviour
{
    // Start is called before the first frame update
    public bool socketed = false;
    public void OnSocketSync1()
    {
        socketed = true;
    }
    public void LeaveSocketReset()
    {
        socketed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (socketed)
        {
            GetComponent<Transform>().localPosition = new Vector3(0, 0, 0);
        }

    }
}
