using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerGameController : MonoBehaviour
{
    public GameObject cameraEffectObj;

    // Start is called before the first frame update
    void Start()
    {
        cameraEffectObj.SetActive(false);
    }

    public void SetCameraEffectObj(bool _isEnable) {
        cameraEffectObj.SetActive(_isEnable);
    }
}