using System.Collections;
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
    public Transform XRRig;

    //XRRig Transforms
    public Transform XRHead;

    public Transform XRHand_Left;
    public Transform XRHand_Right;

    public Vector3 headPositionOffset;
    public Vector3 handRotationOffset;
    public Vector3 previousXRHead;
    public Vector3 _cameraStartingPosition;
    private bool _waitingForCameraMovement = true;
    public CharacterController characterController;
    public bool IsGrounded;
    public LayerMask GroundedLayerMask;
    public float GroundedDistance = .02f;
    public Vector3 GroundNormal { get; set; }

    private void Awake()
    {
        _cameraStartingPosition = XRHead.localPosition;
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //previousXRHead = MainAvatarTransform.position;

        //Head and Body synch
        var delta = XRHead.transform.position - XRRig.transform.position;
        delta.y = 0f;
        if (delta.magnitude > 0.1f)
        {
            delta = Vector3.ProjectOnPlane(delta, GroundNormal);
            XRRig.position += delta;
            XRHead.position -= delta;
        }
        CheckGrounded();
        MainAvatarTransform.position = Vector3.Lerp(MainAvatarTransform.position, XRHead.position + headPositionOffset, 0.5f);
        //MainAvatarTransform.position = XRHead.position + headPositionOffset;
        AvatarHead.rotation = Quaternion.Lerp(AvatarHead.rotation, XRHead.rotation, 0.5f);
        //var delta = MainAvatarTransform.position - previousXRHead;
        //XRRig.position += delta;



        if (AvatarBody!=null)
        {
            AvatarBody.rotation = Quaternion.Lerp(AvatarBody.rotation, Quaternion.Euler(new Vector3(0, AvatarHead.rotation.eulerAngles.y, 0)), 0.05f);
        }

        //Hands synch
        AvatarHand_Right.position = Vector3.Lerp(AvatarHand_Right.position,XRHand_Right.position,0.5f);
        AvatarHand_Right.rotation = Quaternion.Lerp(AvatarHand_Right.rotation,XRHand_Right.rotation,0.5f)*Quaternion.Euler(handRotationOffset);

        AvatarHand_Left.position = Vector3.Lerp(AvatarHand_Left.position,XRHand_Left.position,0.5f);
        AvatarHand_Left.rotation = Quaternion.Lerp(AvatarHand_Left.rotation,XRHand_Left.rotation,0.5f)*Quaternion.Euler(handRotationOffset);
    }


    protected virtual void CheckCameraMovement()
    {
        if (Vector3.Distance(_cameraStartingPosition, XRHead.transform.localPosition) < .05f)
        {
            return;
        }

        var delta = XRHead.transform.position - XRRig.transform.position;
        delta.y = 0f;
        XRHead.transform.position -= delta;
        _waitingForCameraMovement = false;
        previousXRHead = transform.position;
    }
    protected virtual void CheckGrounded()
    {
        var radius = characterController.radius * 0.5f;
        var origin = characterController.center - Vector3.up * (.5f * characterController.height - radius);
        IsGrounded = Physics.SphereCast(
            transform.TransformPoint(origin) + Vector3.up * characterController.contactOffset,
            radius,
            Vector3.down,
            out var hit,
            GroundedDistance + characterController.contactOffset,
            GroundedLayerMask, QueryTriggerInteraction.Ignore);

        GroundNormal = hit.normal;
    }
    protected virtual void HandleHMDMovement()
    {
        var originalCameraPosition = XRHead.transform.position;
        var originalCameraRotation = XRHead.transform.rotation;

        var delta = XRHead.transform.position - XRRig.transform.position;
        delta.y = 0f;
        if (delta.magnitude > 0.1f)
        {
            delta = Vector3.ProjectOnPlane(delta, GroundNormal);
            XRRig.GetComponent<CharacterController>().Move(delta);
        }

        //transform.rotation = Quaternion.Euler(0.0f, XRHead.rotation.eulerAngles.y, 0.0f);

        XRHead.transform.position -= delta;
    }
}
