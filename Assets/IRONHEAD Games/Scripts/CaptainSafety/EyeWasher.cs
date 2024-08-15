using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CaptainSafety;
using UnityEngine.UI;
using Photon.Pun;


public class EyeWasher : MonoBehaviour
{
    public PhotonView view;
    public int playerNum = 0;
    public bool isLensOpened = false;
    public bool isInZone = false;
    public bool isActivated = false;
    public bool isEmitCoroutine = false;
    public ParticleSystem waterVfX;
    public GameObject panelObj;
    public GameObject lensObj;
    public PlayerGameController playerGameController;

    public AudioSource sfxSoure;
    public AudioClip waterClip;

    void Start()
    {
        view = GetComponent<PhotonView>();
    }

    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other != null && other.gameObject.name == "XR Origin")
        {
            isInZone = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other != null && other.gameObject.name == "XR Origin")
        {
            isInZone = false;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (isInZone && collision.collider != null && collision.gameObject.name == "XR Origin")
        { 
            isActivated = true;

            playerGameController = collision.gameObject.GetComponent<PlayerGameController>();

            if (!isEmitCoroutine)
                StartCoroutine(EmitCoroutine());

        }
    }

    [PunRPC]
    public void SetLensState(bool _canOpen)
    {
        isLensOpened = _canOpen;
        if (_canOpen)
        {
            lensObj.transform.localEulerAngles = new Vector3(0, 0, 90);
        }
        else {
            lensObj.transform.localEulerAngles = new Vector3(0, 0, 0);
        }

    }

    [PunRPC]
    public void SetWaterVFX(bool _canPlay) {
        if (_canPlay)
        {
            waterVfX.Play();
        }
        else {
            waterVfX.Stop();
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

    public IEnumerator EmitCoroutine() {

        isEmitCoroutine = true;
        while(true)
        {
            view.RPC("SetLensState", RpcTarget.All, true);

            yield return new WaitForFixedUpdate();

            view.RPC("SetWaterVFX", RpcTarget.All, true);
            sfxSoure.PlayOneShot(waterClip);

            yield return new WaitUntil(() => !waterVfX.isPlaying);

            if (playerGameController != null)
            {
                Debug.Log("---Washed eyes---");
                playerGameController.SetCameraEffectObj(false);
            }
            yield return null;
            isEmitCoroutine = false;
        }
    }

}
