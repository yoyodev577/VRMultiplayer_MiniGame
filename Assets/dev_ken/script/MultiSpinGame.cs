using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiSpinGame : MonoBehaviour
{
    [SerializeField] GameObject Lid;
    [SerializeField] GameObject Spinner;
    [SerializeField] MultispinGameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    //do some checking to see if the toggle lid action is allow
    public void CheckToggleLid()
    {
        //if (!gameManager.IsGameStart)
        //{
        //    return;
        //}
        Lid.GetComponent<LidToggle>().ToggleLid();
    }
    public void CheckActivateSpinner()
    {
        Spinner.GetComponent<Spiner>().ActivateSpinner();
    }
    public void CheckResult()
    {
        //StartCoroutine(Explode());
    }

}
