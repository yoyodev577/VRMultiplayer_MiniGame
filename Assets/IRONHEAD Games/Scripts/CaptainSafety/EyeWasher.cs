using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CaptainSafety;
using UnityEngine.UI;
using Photon.Pun;
using Oculus.Interaction;


public class EyeWasher : MonoBehaviour
{
    public GameManager gameManager;
    public PhotonView view;
    public int playerNum = 0;
    public bool isLensOpened = false;
    public bool isInZone = false;
    public bool isActivated = false;
    public bool isWashed = false;
    public bool isEmitCoroutine = false;
    public GameObject waterVfX;
    public GameObject panelObj;
    public GameObject lensObj;
    public PlayerGameController playerGameController;

    public AudioSource sfxSoure;
    public AudioClip winClip;
    public AudioClip waterClip;

    void Start()
    {
        isLensOpened = false;
        isInZone = false;
        isActivated = false;
        isWashed = false;
        isEmitCoroutine = false;

        view = GetComponent<PhotonView>();
        gameManager = FindObjectOfType<GameManager>();
       // SetLensState(false);
       // SetPanelState(false);
        view.RPC("SetLensState", RpcTarget.All, false);
        view.RPC("SetPanelState", RpcTarget.All, false);
    }

    public void Update()
    {
        if (gameManager.isGameStart && !gameManager.isGameEnd)
        {
            if (isActivated && !isEmitCoroutine)
            {
                StartCoroutine(EmitCoroutine());
            }

            if (isWashed) {
                //only the fastest one win the game
                if (!gameManager.isGameEnd)
                {
                    view.RPC("SetPanelState", RpcTarget.All, true);
                    sfxSoure.PlayOneShot(winClip);
                    gameManager.EndGame();
                }
            }
        }
    }



    public void OnTriggerEnter(Collider other)
    {
        if (other != null && other.gameObject.name == "XR Origin")
        {

            playerGameController = other.gameObject.GetComponentInParent<PlayerGameController>();
            isInZone = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other != null && other.gameObject.name == "XR Origin")
        {
            isInZone = false;
            playerGameController = null; 
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
       // Debug.Log("---" + collision.gameObject.name);

        if (isInZone && collision.collider != null && collision.gameObject.tag == "Hand")
        {
            isActivated = true;

        }
    }

    public void OnCollisionExit(Collision collision)
    {
        isActivated = false;
        
    }

    [PunRPC]
    public void SetLensState(bool _canOpen)
    {
        if (_canOpen)
        {
            Debug.Log("--Open Lens--");
            lensObj.transform.localEulerAngles = new Vector3(0, 0, 90);
            waterVfX.SetActive(true);
            sfxSoure.PlayOneShot(waterClip);
        }
        else {
            Debug.Log("--Close Lens--");
            lensObj.transform.localEulerAngles = new Vector3(0, 0, 0);
            waterVfX.SetActive(false);
        }

    }


    [PunRPC]
    public void SetPanelState(bool _canShow)
    {
        if (_canShow)
        {
            panelObj.SetActive(true);
        }
        else
        {
            panelObj.SetActive(false);
        }
    }

    public IEnumerator EmitCoroutine()
    {

        isEmitCoroutine = true;
        Debug.Log("----Coroutine---");
        view.RPC("SetLensState", RpcTarget.All, true);
        yield return new WaitForSeconds(3);
        view.RPC("SetLensState", RpcTarget.All, false);
        isActivated = false;

        if (playerGameController != null)
        {
            Debug.Log("---Washed eyes---");
            playerGameController.SetCameraEffectObj(false);
            isWashed = true;
        }

        yield return null;

        isEmitCoroutine = false;

    }

}
