using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Animations;

public class TestTube : MonoBehaviour
{
    public MultiSpin multispin;
    [SerializeField] private Vector3 startPos;
    [SerializeField] private Quaternion startRot;
    [SerializeField] private GameObject cap, body;
    public bool isReset = false;
    public bool grabbed  = false; //player grabbing the testtube or not
    // Start is called before the first frame update
    PhotonView View;
    Rigidbody rb;
    void Start()
    {
        View = GetComponent<PhotonView>();
        startPos= transform.position;
        startRot= transform.rotation;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (isReset) {
            View.RPC("PhotonReset", RpcTarget.All);
           //PhotonReset();
            isReset = false;
        }
    }

    public void grab(){
        View.RPC("PhotonGrab", RpcTarget.All);
    }
    [PunRPC]
    public void PhotonGrab(){

        if (MultispinGameManager.instance.IsGameStart ||
            !MultispinGameManager.instance.IsGameEnd ||
            !multispin.isLidOpened || !multispin.isSpinning)
        {
            grabbed = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            //PhotonOnCollision();

            if(PhotonNetwork.IsConnected) 
            View.RPC("PhotonOnCollision", RpcTarget.All);
        }
    }

    [PunRPC]
    public void PhotonOnCollision()
    {

        if(GetComponent<ParentConstraint>() != null)
           Destroy(GetComponent<ParentConstraint>());

        Debug.Log("--Reset testtube position--");
        transform.position = startPos;
        transform.rotation = startRot;
    
    }

    [PunRPC]
    public void PhotonReset() {

        if (GetComponent<ParentConstraint>() != null)
            Destroy(GetComponent<ParentConstraint>());

        grabbed = false;
        transform.position = startPos;
        transform.rotation = startRot;
    }

    public void OnReset() {
        if (PhotonNetwork.IsConnected)
            View.RPC("PhotonReset", RpcTarget.All);
        
    }
}
