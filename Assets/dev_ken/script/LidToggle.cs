using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LidToggle : MonoBehaviour
{
    [SerializeField] MultiSpinGame multiSpinGame;
    //euler angle when the lid is opened
    private Vector3 LidOpenEuler = new Vector3(310, 0, 0);
    //euler angle when the lid is closed
    private Vector3 LidCloseEuler = new Vector3(77.295723f, 0, 0);
    //if the lid is opened
    public bool LidOpen = false;
    //toggle the lid
    public void ToggleLid()
    {
        if (LidOpen)
        {
            //close the cover
            GetComponent<Transform>().transform.localEulerAngles = LidCloseEuler;
            multiSpinGame.CheckActivateSpinner();
        }
        else
        {
            //open cover
            GetComponent<Transform>().transform.localEulerAngles = LidOpenEuler;
        }
        //toggle the state of the lid (open or not)
        LidOpen = !LidOpen;
    }
    //set the lid
    public void SetLid(bool status)
    {
        if (!status)
        {
            //close the cover
            GetComponent<Transform>().transform.localEulerAngles = LidCloseEuler;
        }
        else
        {
            //open cover
            GetComponent<Transform>().transform.localEulerAngles = LidOpenEuler;
        }
        //set the state of the lid (open or not)
        LidOpen = status;
    }
}
