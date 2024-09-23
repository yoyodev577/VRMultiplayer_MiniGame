using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spiner : MonoBehaviour
{
    [SerializeField] MultiSpinGame multiSpinGame;
    public float spinSpeed = 0;
    public bool spining = false;
    public void ActivateSpinner()
    {
        //prevent trigger spining when spining
        if (spining)
        {
            return;
        }
        
        //start spinning
        StartCoroutine(ActivateSpinnerCor());

    }
    IEnumerator ActivateSpinnerCor()
    {
        spining = true;
        //start spinning (accelerate)
        spinSpeed = 0;
        while (spinSpeed < 20.0)
        {
            spinSpeed += .3f;
            GetComponent<Transform>().transform.Rotate(Vector3.forward * spinSpeed);
            yield return null;
        }
        //stop spinning (decelerate)
        while (spinSpeed >0.0)
        {
            spinSpeed -= .3f;
            GetComponent<Transform>().transform.Rotate(Vector3.forward * spinSpeed);
            yield return null;
        }
        spinSpeed = 0;
        spining = false;
        //finish spinning, check result
        multiSpinGame.CheckResult();
    }
}
