﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarInputConverter : MonoBehaviour
{

    //Avatar Transforms
    public Transform MainAvatarTransform;
    public Transform AvatarHead;
    public Transform AvatarBody;

    public Transform AvatarHand_Left;
    public Transform AvatarHand_Right;

    //XRRig Transforms
    public Transform XRHead;

    public Transform XRHand_Left;
    public Transform XRHand_Right;

    public Vector3 headPositionOffset;
    public Vector3 handRotationOffset;


    private void Start()
    {
        XRHead = GameObject.FindGameObjectWithTag("Rig").transform.Find("PlayerController");
        XRHand_Left = GameObject.FindGameObjectWithTag("Rig").transform.Find("Physics LeftHand/LeftHandModel");
        XRHand_Right = GameObject.FindGameObjectWithTag("Rig").transform.Find("Physics RightHand/RightHandModel");
    }
    // Update is called once per frame
    void Update()
    {
        //Head and Body synch
        MainAvatarTransform.position = Vector3.Lerp(MainAvatarTransform.position, XRHead.position + headPositionOffset+ XRHead.GetComponent<CharacterController>().center+new Vector3(0, XRHead.GetComponent<CharacterController>().height/2, 0), 0.5f);
        AvatarHead.rotation = Quaternion.Lerp(AvatarHead.rotation, XRHead.rotation, 0.5f);
        AvatarHead.GetChild(0).rotation = Quaternion.Lerp(AvatarHead.rotation, XRHead.Find("CameraRig/FloorOffset/CameraScale/Camera").rotation, 0.5f);
        if (AvatarBody!=null)
        {
            AvatarBody.rotation = Quaternion.Lerp(AvatarBody.rotation, Quaternion.Euler(new Vector3(0, AvatarHead.rotation.eulerAngles.y, 0)), 0.05f);
        }

        //Hands synch
        //AvatarHand_Right.position = Vector3.Lerp(AvatarHand_Right.position,XRHand_Right.position,0.5f);
        //AvatarHand_Right.rotation = Quaternion.Lerp(AvatarHand_Right.rotation,XRHand_Right.rotation,0.5f)*Quaternion.Euler(handRotationOffset);
        AvatarHand_Right.position = XRHand_Right.position;
        AvatarHand_Right.eulerAngles = new Vector3(-XRHand_Right.eulerAngles.x, XRHand_Right.eulerAngles.y + 180f, XRHand_Right.eulerAngles.z-90);
        //XRHand_Right.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;

        //AvatarHand_Left.position = Vector3.Lerp(AvatarHand_Left.position,XRHand_Left.position,0.5f);
        //AvatarHand_Left.rotation = Quaternion.Lerp(AvatarHand_Left.rotation,XRHand_Left.rotation,0.5f)*Quaternion.Euler(handRotationOffset);
        AvatarHand_Left.position = XRHand_Left.position;
        AvatarHand_Left.eulerAngles = new Vector3(-XRHand_Left.eulerAngles.x, XRHand_Left.eulerAngles.y + 180f,XRHand_Left.eulerAngles.z+90);
        //XRHand_Left.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
    }
}
