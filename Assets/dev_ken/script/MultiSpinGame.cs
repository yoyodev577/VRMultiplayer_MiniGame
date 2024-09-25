using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiSpinGame : MonoBehaviour
{
    [SerializeField] GameObject Lid;
    [SerializeField] GameObject Spinner;
    [SerializeField] MultispinGameManager gameManager;
    [SerializeField] private ParticleSystem explosion;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip correctClip, explodeClip;
    [SerializeField] private Image correctImage;
    [SerializeField] public GameObject[] TestTubeHolder;
    [SerializeField] public GameObject[] TestTubes;
    private bool finished = false;
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
        //do not open if the game is not start or it is spining
        if (!gameManager.IsGameStart || Spinner.GetComponent<Spiner>().spining || finished)
        {
            return;
        }
        Lid.GetComponent<LidToggle>().ToggleLid();
    }
    //do some checking and spin
    public void CheckActivateSpinner()
    {
        Spinner.GetComponent<Spiner>().ActivateSpinner();
    }
    //check the result
    public void CheckResult()
    {
        finished = true;
        List<int> TestTubeIndex = new List<int>();
        for (int i = 0; i < TestTubeHolder.Length; i++)
        {
            if (TestTubeHolder[i].transform.childCount > 0)
            {
                TestTubeIndex.Add(i);
                if (TestTubeHolder[(i + 4) % 8].transform.childCount < 1)
                {
                    //wrong
                    StartCoroutine(Explode());
                    return;
                }
            }
        }
        //correct
        //show the tick image
        correctImage.enabled = true;
        //play the correct sound
        audioSource.PlayOneShot(correctClip);
        
    }
    IEnumerator Explode()
    {
        explosion.Play(true);
        audioSource.PlayOneShot(explodeClip);
        yield return new WaitForSeconds(1);
        explosion.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        yield return null;

    }

    public void ResetGame()
    {
        foreach (var TestTube in TestTubes)
        {
            TestTube.SetActive(false);
            finished = false;
            Lid.GetComponent<LidToggle>().SetLid(false);
        }
    }

}
