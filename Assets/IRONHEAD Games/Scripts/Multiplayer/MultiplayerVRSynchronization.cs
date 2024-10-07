﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using HurricaneVR.Framework.ControllerInput;
using HurricaneVR.Framework.Shared;


public class MultiplayerVRSynchronization : MonoBehaviour, IPunObservable
{

    private PhotonView m_PhotonView;


    //Main VRPlayer Transform Synch
    [Header("Networked VR Player Transform Synch")]
    public Transform networkedVRPlayerTransform;

    //Position
    private float m_Distance_NetworkedVRPlayer;
    private Vector3 m_Direction_NetworkedVRPlayer;
    private Vector3 m_NetworkPosition_NetworkedVRPlayer;
    private Vector3 m_StoredPosition_NetworkedVRPlayer;

    //Rotation
    private Quaternion m_NetworkRotation_GeneralVRPlayer;
    private float m_Angle_GeneralVRPlayer;


    //Main Avatar Transform Synch
    [Header("Main Avatar Transform Synch")]
    public Transform mainAvatarTransform;



    //Position
    private float m_Distance_MainAvatar;
    private Vector3 m_Direction_MainAvatar;
    private Vector3 m_NetworkPosition_MainAvatar;
    private Vector3 m_StoredPosition_MainAvatar;

    //Rotation
    private Quaternion m_NetworkRotation_MainAvatar;
    private float m_Angle_MainAvatar;

    //Head  Synch
    //Rotation
    [Header("Avatar Head Transform Synch")]
    public Transform headTransform;

    private Quaternion m_NetworkRotation_Head;
    private float m_Angle_Head;
    //Head child Synch
    //Rotation
    private Quaternion m_NetworkRotation_HeadChild;
    private float m_Angle_HeadChild;

    //Body Synch
    //Rotation
    [Header("Avatar Body Transform Synch")]
    public Transform bodyTransform;

    private Quaternion m_NetworkRotation_Body;
    private float m_Angle_Body;


    //Hands Synch
    [Header("Hands Transform Synch")]
    public Transform leftHandTransform;
    public Transform rightHandTransform;

    //Left Hand Sync
    //Position
    private float m_Distance_LeftHand;

    private Vector3 m_Direction_LeftHand;
    private Vector3 m_NetworkPosition_LeftHand;
    private Vector3 m_StoredPosition_LeftHand;

    //Rotation
    private Quaternion m_NetworkRotation_LeftHand;
    private float m_Angle_LeftHand;



    //Left Hand Sync
    //Position
    private float m_Distance_RightHand;

    private Vector3 m_Direction_RightHand;
    private Vector3 m_NetworkPosition_RightHand;
    private Vector3 m_StoredPosition_RightHand;

    //Rotation
    private Quaternion m_NetworkRotation_RightHand;
    private float m_Angle_RightHand;
    /*
    [SerializeField] HVRPlayerInputs m_hvrplayerinputs;
    int Idle = Animator.StringToHash("Idle");
    int Point = Animator.StringToHash("Point");
    int GrabLarge = Animator.StringToHash("GrabLarge");
    int GrabSmall = Animator.StringToHash("GrabSmall");
    int GrabStickUp = Animator.StringToHash("GrabStickUp");
    int GrabStickFront = Animator.StringToHash("GrabStickFront");
    int ThumbUp = Animator.StringToHash("ThumbUp");
    int Fist = Animator.StringToHash("Fist");
    int Gun = Animator.StringToHash("Gun");
    int GunShoot = Animator.StringToHash("GunShoot");
    int PushButton = Animator.StringToHash("PushButton");
    int Spread = Animator.StringToHash("Spread");
    int MiddleFinger = Animator.StringToHash("MiddleFinger");
    int Peace = Animator.StringToHash("Peace");
    int OK = Animator.StringToHash("OK");
    int Phone = Animator.StringToHash("Phone");
    int Rock = Animator.StringToHash("Rock");
    int Natural = Animator.StringToHash("Natural");
    int Number3 = Animator.StringToHash("Number3");
    int Number4 = Animator.StringToHash("Number4");
    int Number3V2 = Animator.StringToHash("Number3V2");
    int HoldViveController = Animator.StringToHash("HoldViveController");
    int PressTriggerViveController = Animator.StringToHash("PressTriggerViveController");
    int HoldOculusController = Animator.StringToHash("HoldOculusController");
    int PressTriggerOculusController = Animator.StringToHash("PressTriggerOculusController");
    */
    bool m_firstTake = false;

    public void Awake()
    {
        m_PhotonView = GetComponent<PhotonView>();

        //  m_capsuleCollider_Height = capsuleCollider.height;

        //Main VRPlayer Synch Init
        m_StoredPosition_NetworkedVRPlayer = networkedVRPlayerTransform.position;
        m_NetworkPosition_NetworkedVRPlayer = Vector3.zero;
        m_NetworkRotation_GeneralVRPlayer = Quaternion.identity;

        //Main Avatar Synch Init
        m_StoredPosition_MainAvatar = mainAvatarTransform.localPosition;
        m_NetworkPosition_MainAvatar = Vector3.zero;
        m_NetworkRotation_MainAvatar = Quaternion.identity;

        //Head Synch Init
        m_NetworkRotation_Head = Quaternion.identity;

        //Body Synch Init
        m_NetworkRotation_Body = Quaternion.identity;

        //Left Hand Synch Init
        m_StoredPosition_LeftHand = leftHandTransform.localPosition;
        m_NetworkPosition_LeftHand = Vector3.zero;
        m_NetworkRotation_LeftHand = Quaternion.identity;

        //Right Hand Synch Init
        m_StoredPosition_RightHand = rightHandTransform.localPosition;
        m_NetworkPosition_RightHand = Vector3.zero;
        m_NetworkRotation_RightHand = Quaternion.identity;




    }

    void OnEnable()
    {
        m_firstTake = true;
    }

    public void Update()
    {
        if (!this.m_PhotonView.IsMine)
        {


            //networkedVRPlayerTransform.position = Vector3.MoveTowards(networkedVRPlayerTransform.position, this.m_NetworkPosition_NetworkedVRPlayer, this.m_Distance_NetworkedVRPlayer * (1.0f / PhotonNetwork.SerializationRate));
            //networkedVRPlayerTransform.rotation = Quaternion.RotateTowards(networkedVRPlayerTransform.rotation, this.m_NetworkRotation_GeneralVRPlayer, this.m_Angle_GeneralVRPlayer * (1.0f / PhotonNetwork.SerializationRate));

            //mainAvatarTransform.localPosition = Vector3.MoveTowards(mainAvatarTransform.localPosition, this.m_NetworkPosition_MainAvatar, this.m_Distance_MainAvatar * (1.0f / PhotonNetwork.SerializationRate));
            //mainAvatarTransform.localRotation = Quaternion.RotateTowards(mainAvatarTransform.localRotation, this.m_NetworkRotation_MainAvatar, this.m_Angle_MainAvatar * (1.0f / PhotonNetwork.SerializationRate));



            //headTransform.localRotation = Quaternion.RotateTowards(headTransform.localRotation, this.m_NetworkRotation_Head, this.m_Angle_Head * (1.0f / PhotonNetwork.SerializationRate));
            //headTransform.GetChild(0).localRotation = Quaternion.RotateTowards(headTransform.GetChild(0).localRotation, this.m_NetworkRotation_HeadChild, this.m_Angle_HeadChild * (1.0f / PhotonNetwork.SerializationRate));
            //bodyTransform.localRotation = Quaternion.RotateTowards(bodyTransform.localRotation, this.m_NetworkRotation_Body, this.m_Angle_Body * (1.0f / PhotonNetwork.SerializationRate));


            //leftHandTransform.localPosition = Vector3.MoveTowards(leftHandTransform.localPosition, this.m_NetworkPosition_LeftHand, this.m_Distance_LeftHand * (1.0f / PhotonNetwork.SerializationRate));
            //leftHandTransform.localRotation = Quaternion.RotateTowards(leftHandTransform.localRotation, this.m_NetworkRotation_LeftHand, this.m_Angle_LeftHand * (1.0f / PhotonNetwork.SerializationRate));

            //rightHandTransform.localPosition = Vector3.MoveTowards(rightHandTransform.localPosition, this.m_NetworkPosition_RightHand, this.m_Distance_RightHand * (1.0f / PhotonNetwork.SerializationRate));
            //rightHandTransform.localRotation = Quaternion.RotateTowards(rightHandTransform.localRotation, this.m_NetworkRotation_RightHand, this.m_Angle_RightHand * (1.0f / PhotonNetwork.SerializationRate));

            //networkedVRPlayerTransform.position = Vector3.MoveTowards(networkedVRPlayerTransform.position, this.m_NetworkPosition_NetworkedVRPlayer, this.m_Distance_NetworkedVRPlayer * (1.0f / PhotonNetwork.SerializationRate));
            //networkedVRPlayerTransform.rotation = Quaternion.RotateTowards(networkedVRPlayerTransform.rotation, this.m_NetworkRotation_GeneralVRPlayer, this.m_Angle_GeneralVRPlayer * (1.0f / PhotonNetwork.SerializationRate));

            //mainAvatarTransform.localPosition = Vector3.MoveTowards(mainAvatarTransform.localPosition, this.m_NetworkPosition_MainAvatar, this.m_Distance_MainAvatar * (1.0f / PhotonNetwork.SerializationRate));
            //mainAvatarTransform.localRotation = Quaternion.RotateTowards(mainAvatarTransform.localRotation, this.m_NetworkRotation_MainAvatar, this.m_Angle_MainAvatar * (1.0f / PhotonNetwork.SerializationRate));

            networkedVRPlayerTransform.position = this.m_NetworkPosition_NetworkedVRPlayer;
            networkedVRPlayerTransform.rotation = this.m_NetworkRotation_GeneralVRPlayer;

            mainAvatarTransform.localPosition = this.m_NetworkPosition_MainAvatar;
            mainAvatarTransform.localRotation = this.m_NetworkRotation_MainAvatar;

            headTransform.localRotation = this.m_NetworkRotation_Head;
            headTransform.GetChild(0).localRotation = this.m_NetworkRotation_HeadChild;
            bodyTransform.localRotation = this.m_NetworkRotation_Body;


            leftHandTransform.localPosition = this.m_NetworkPosition_LeftHand;
            leftHandTransform.localRotation = this.m_NetworkRotation_LeftHand;

            rightHandTransform.localPosition = this.m_NetworkPosition_RightHand;
            rightHandTransform.localRotation = this.m_NetworkRotation_RightHand;

            /*if (m_hvrplayerinputs.LeftController.GripButtonState.Active)
            {
                leftHandTransform.Find("vr_cartoon_hand_prefab_left").GetComponent<Animator>().SetTrigger(GrabLarge);
            }*/
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //////////////////////////////////////////////////////////////////
            //The Main VR Player Transform Synch

            //Send Main Avatar position data
            this.m_Direction_NetworkedVRPlayer = networkedVRPlayerTransform.position - this.m_StoredPosition_NetworkedVRPlayer;
            this.m_StoredPosition_NetworkedVRPlayer = networkedVRPlayerTransform.position;

            stream.SendNext(networkedVRPlayerTransform.position);
            stream.SendNext(this.m_Direction_NetworkedVRPlayer);

            //Send Main Avatar rotation data
            stream.SendNext(networkedVRPlayerTransform.rotation);


            //////////////////////////////////////////////////////////////////
            //Main Avatar Transform Synch

            //Send Main Avatar position data
            this.m_Direction_MainAvatar = mainAvatarTransform.localPosition - this.m_StoredPosition_MainAvatar;
            this.m_StoredPosition_MainAvatar = mainAvatarTransform.localPosition;

            stream.SendNext(mainAvatarTransform.localPosition);
            stream.SendNext(this.m_Direction_MainAvatar);

            //Send Main Avatar rotation data
            stream.SendNext(mainAvatarTransform.localRotation);



            ///////////////////////////////////////////////////////////////////
            //Head rotation synch

            //Send Head rotation data
            stream.SendNext(headTransform.localRotation);
            stream.SendNext(headTransform.GetChild(0).localRotation);


            ///////////////////////////////////////////////////////////////////
            //Body rotation synch

            //Send Body rotation data
            stream.SendNext(bodyTransform.localRotation);


            ///////////////////////////////////////////////////////////////////
            //Hands Transform Synch
            //Left Hand
            //Send Left Hand position data
            this.m_Direction_LeftHand = leftHandTransform.localPosition - this.m_StoredPosition_LeftHand;
            this.m_StoredPosition_LeftHand = leftHandTransform.localPosition;

            stream.SendNext(leftHandTransform.localPosition);
            stream.SendNext(this.m_Direction_LeftHand);

            //Send Left Hand rotation data
            stream.SendNext(leftHandTransform.localRotation);

            //Right Hand
            //Send Right Hand position data
            this.m_Direction_RightHand = rightHandTransform.localPosition - this.m_StoredPosition_RightHand;
            this.m_StoredPosition_RightHand = rightHandTransform.localPosition;

            stream.SendNext(rightHandTransform.localPosition);
            stream.SendNext(this.m_Direction_RightHand);

            //Send Right Hand rotation data
            stream.SendNext(rightHandTransform.localRotation);

        }
        else
        {
            ///////////////////////////////////////////////////////////////////
            //The Main VR Player Transform Synch

            //Get VR Player position data
            this.m_NetworkPosition_NetworkedVRPlayer = (Vector3)stream.ReceiveNext();
            this.m_Direction_NetworkedVRPlayer = (Vector3)stream.ReceiveNext();

            if (m_firstTake)
            {
                networkedVRPlayerTransform.position = this.m_NetworkPosition_NetworkedVRPlayer;
                this.m_Distance_NetworkedVRPlayer = 0f;
            }
            else
            {
                float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
                this.m_NetworkPosition_NetworkedVRPlayer += this.m_Direction_NetworkedVRPlayer * lag;
                this.m_Distance_NetworkedVRPlayer = Vector3.Distance(networkedVRPlayerTransform.position, this.m_NetworkPosition_NetworkedVRPlayer);
            }

            //Get Main Avatar rotation data
            this.m_NetworkRotation_GeneralVRPlayer = (Quaternion)stream.ReceiveNext();
            if (m_firstTake)
            {
                this.m_Angle_GeneralVRPlayer = 0f;
                networkedVRPlayerTransform.rotation = this.m_NetworkRotation_GeneralVRPlayer;
            }
            else
            {
                this.m_Angle_GeneralVRPlayer = Quaternion.Angle(networkedVRPlayerTransform.rotation, this.m_NetworkRotation_GeneralVRPlayer);
            }

            ///////////////////////////////////////////////////////////////////
            //Main Avatar Transform Synch

            //Get Main Avatar position data
            this.m_NetworkPosition_MainAvatar = (Vector3)stream.ReceiveNext();
            this.m_Direction_MainAvatar = (Vector3)stream.ReceiveNext();

            if (m_firstTake)
            {
                mainAvatarTransform.localPosition = this.m_NetworkPosition_MainAvatar;
                this.m_Distance_MainAvatar = 0f;
            }
            else
            {
                float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
                this.m_NetworkPosition_MainAvatar += this.m_Direction_MainAvatar * lag;
                this.m_Distance_MainAvatar = Vector3.Distance(mainAvatarTransform.localPosition, this.m_NetworkPosition_MainAvatar);
            }

            //Get Main Avatar rotation data
            this.m_NetworkRotation_MainAvatar = (Quaternion)stream.ReceiveNext();
            if (m_firstTake)
            {
                this.m_Angle_MainAvatar = 0f;
                mainAvatarTransform.rotation = this.m_NetworkRotation_MainAvatar;
            }
            else
            {
                this.m_Angle_MainAvatar = Quaternion.Angle(mainAvatarTransform.rotation, this.m_NetworkRotation_MainAvatar);
            }


            ///////////////////////////////////////////////////////////////////
            //Head rotation synch
            //Get Head rotation data 
            this.m_NetworkRotation_Head = (Quaternion)stream.ReceiveNext();

            if (m_firstTake)
            {
                this.m_Angle_Head = 0f;
                headTransform.localRotation = this.m_NetworkRotation_Head;
            }
            else
            {
                this.m_Angle_Head = Quaternion.Angle(headTransform.localRotation, this.m_NetworkRotation_Head);
            }

            ///////////////////////////////////////////////////////////////////
            //HeadChild rotation synch
            //Get Head rotation data 
            this.m_NetworkRotation_HeadChild = (Quaternion)stream.ReceiveNext();

            if (m_firstTake)
            {
                this.m_Angle_HeadChild = 0f;
                headTransform.GetChild(0).localRotation = this.m_NetworkRotation_HeadChild;
            }
            else
            {
                this.m_Angle_HeadChild = Quaternion.Angle(headTransform.GetChild(0).localRotation, this.m_NetworkRotation_HeadChild);
            }

            ///////////////////////////////////////////////////////////////////
            //Body rotation synch
            //Get Body rotation data 
            this.m_NetworkRotation_Body = (Quaternion)stream.ReceiveNext();

            if (m_firstTake)
            {
                this.m_Angle_Body = 0f;
                bodyTransform.localRotation = this.m_NetworkRotation_Body;
            }
            else
            {
                this.m_Angle_Body = Quaternion.Angle(bodyTransform.localRotation, this.m_NetworkRotation_Body);
            }

            ///////////////////////////////////////////////////////////////////
            //Hands Transform Synch
            //Get Left Hand position data
            this.m_NetworkPosition_LeftHand = (Vector3)stream.ReceiveNext();
            this.m_Direction_LeftHand = (Vector3)stream.ReceiveNext();

            if (m_firstTake)
            {
                leftHandTransform.localPosition = this.m_NetworkPosition_LeftHand;
                this.m_Distance_LeftHand = 0f;
            }
            else
            {
                float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
                this.m_NetworkPosition_LeftHand += this.m_Direction_LeftHand * lag;
                this.m_Distance_LeftHand = Vector3.Distance(leftHandTransform.localPosition, this.m_NetworkPosition_LeftHand);
            }

            //Get Left Hand rotation data
            this.m_NetworkRotation_LeftHand = (Quaternion)stream.ReceiveNext();
            if (m_firstTake)
            {
                this.m_Angle_LeftHand = 0f;
                leftHandTransform.localRotation = this.m_NetworkRotation_LeftHand;
            }
            else
            {
                this.m_Angle_LeftHand = Quaternion.Angle(leftHandTransform.localRotation, this.m_NetworkRotation_LeftHand);
            }

            //Get Right Hand position data
            this.m_NetworkPosition_RightHand = (Vector3)stream.ReceiveNext();
            this.m_Direction_RightHand = (Vector3)stream.ReceiveNext();

            if (m_firstTake)
            {
                rightHandTransform.localPosition = this.m_NetworkPosition_RightHand;
                this.m_Distance_RightHand = 0f;
            }
            else
            {
                float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
                this.m_NetworkPosition_RightHand += this.m_Direction_RightHand * lag;
                this.m_Distance_RightHand = Vector3.Distance(rightHandTransform.localPosition, this.m_NetworkPosition_RightHand);
            }

            //Get Right Hand rotation data
            this.m_NetworkRotation_RightHand = (Quaternion)stream.ReceiveNext();
            if (m_firstTake)
            {
                this.m_Angle_RightHand = 0f;
                rightHandTransform.localRotation = this.m_NetworkRotation_RightHand;
            }
            else
            {
                this.m_Angle_RightHand = Quaternion.Angle(rightHandTransform.localRotation, this.m_NetworkRotation_RightHand);
            }
            if (m_firstTake)
            {
                m_firstTake = false;
            }
        }
    }

}
