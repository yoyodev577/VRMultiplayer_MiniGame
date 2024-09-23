using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spiner : MonoBehaviour
{
    [SerializeField] MultiSpinGame multiSpinGame;
    public float spinSpeed = 0;
    public void ActivateSpinner()
    {
        //reset the speed to 0 for every spinning begin 
        spinSpeed = 0;
        //start spinning
        StartCoroutine(ActivateSpinnerCor());

    }
    IEnumerator ActivateSpinnerCor()
    {
        //start spinning (accelerate)
        while (spinSpeed < 20)
        {
            spinSpeed += .3f;
            GetComponent<Transform>().transform.Rotate(Vector3.forward * spinSpeed);
            yield return null;
        }
        //stop spinning (decelerate)
        while (spinSpeed >0)
        {
            spinSpeed -= .3f;
            GetComponent<Transform>().transform.Rotate(Vector3.forward * spinSpeed);
            yield return null;
        }
        //finish spinning, check result
        multiSpinGame.CheckResult();
        yield break;
    }
}
