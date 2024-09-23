using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiSpinGame : MonoBehaviour
{
    [SerializeField] GameObject Lid;
    [SerializeField] GameObject Spinner;
    [SerializeField] MultispinGameManager gameManager;
    [SerializeField] private ParticleSystem explosion;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip correctClip, explodeClip;
    [SerializeField] public GameObject[] TestTubeHolder;
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
        if (!gameManager.IsGameStart || Spinner.GetComponent<Spiner>().spining)
        {
            return;
        }
        Lid.GetComponent<LidToggle>().ToggleLid();
    }
    public void CheckActivateSpinner()
    {
        Spinner.GetComponent<Spiner>().ActivateSpinner();
    }
    public void CheckResult()
    {
        StartCoroutine(Explode());
    }
    IEnumerator Explode()
    {
        explosion.Play(true);
        audioSource.PlayOneShot(explodeClip);
        yield return new WaitForSeconds(1);
        explosion.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        yield return null;

    }

}
