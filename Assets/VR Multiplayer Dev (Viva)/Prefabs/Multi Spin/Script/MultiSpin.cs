using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using System;
using TMPro;
using Photon.Pun;
using System.Linq;
using UnityEngine.UI;

public class MultiSpin : MonoBehaviour
{
    #region Variable Declaration

    [SerializeField] private MultispinGameManager gameManager;


    public int playerNum = 0;
    //Spinner variables
    [SerializeField]
    private GameObject spinner;
    [SerializeField] private float spinSpeed = 0;
    [SerializeField] private GameObject pivot;
    [SerializeField] private GameObject lid;

    [SerializeField] public bool isSpinning;
    [SerializeField] public bool isSpinningFinished = false;
    [SerializeField] public bool isLidOpened = false;
    [SerializeField] private bool isMultispinCoroutine = false;
    [SerializeField] public bool isSpinnerTriggered = false;
    [SerializeField] public bool hasResult = false;
    [SerializeField] public bool IsShowResult = false;
    [SerializeField] private bool isSpinCoroutine = false;
    [SerializeField] private bool isResultCoroutine = false;

    [SerializeField] private Transform testTubeParent;

    [SerializeField] private List<TestTube> testTubeList = new List<TestTube>();
    [SerializeField] private List<MultiSpinTestTubeLock> testTubeLocks = new List<MultiSpinTestTubeLock>();
    [SerializeField] private List<TestTube> lockedTestTube = new List<TestTube>();

    //Test tube position checking variables
    [SerializeField]
    private int defaultTubeAmount = 3;
    [SerializeField] private int spinnerPosCount;
    [SerializeField] private bool[] testTubePlaceholder;
    [SerializeField] private List<bool[]> correctArrangement;
    public bool isBalanced = false;

    //Explosion variables
    [SerializeField] private ParticleSystem explosion;

    [SerializeField]
    private TMP_Text debug, correctSequence;
    private string whichHasTestTube, currentSequence;
    [SerializeField] PhotonView View;

    [SerializeField] private Image correctImage;

    [SerializeField] private AudioClip correctClip, explodeClip;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private bool isExplodeCoroutine = false;

    [SerializeField] private SphereCollider buttonCollider;
    //[SerializeField] private Transform buttonPivot;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        View = GetComponent<PhotonView>();
        audioSource = GetComponent<AudioSource>();
        gameManager = FindObjectOfType<MultispinGameManager>();
        buttonCollider= GetComponent<SphereCollider>();
        testTubeList.AddRange(testTubeParent.transform.GetComponentsInChildren<TestTube>());
        testTubeLocks.AddRange(spinner.transform.GetComponentsInChildren<MultiSpinTestTubeLock>());
        spinnerPosCount = spinner.transform.childCount;
        testTubePlaceholder = new bool[spinnerPosCount];
        View.RPC("SetCorrectImageStatus", RpcTarget.All, false);

    }

    // Update is called once per frame
    void Update()
    {
        //after spin
        if (isSpinningFinished)
        {
            if (!hasResult)
            {
                //check the result
                View.RPC("CheckSpinnerBalance", RpcTarget.All);
            }
            else
            {
                //after having result, then show it
                if(!IsShowResult)
                View.RPC("ShowResult", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    public void PhotonUpdate() {
        //lid.GetComponent<Rigidbody>().isKinematic = isMultispinCoroutine;
        // CheckSpinning();
        if (MultispinGameManager.instance.IsGameStart && !MultispinGameManager.instance.IsGameEnd) {
            if (!hasResult)
            {
                CheckTestTubePos();
                TubeCorrectSequence(CheckTestTubeAmount());

                Debug.Log("---Game Update---------");
                //SetSpinner();
                //TubeCorrectSequence(CheckTestTubeAmount());
                //CheckSpinnerBalance();
               //View.RPC("ShowResult", RpcTarget.All);

            }
        }

    }


    public void OnTriggerEnter(Collider other)
    {

        if (gameManager.IsGameStart && other.gameObject.tag == "Hand"
            && !isSpinning && !hasResult) {
            Debug.Log(other.gameObject.name + "Collides");;

            if (PhotonNetwork.IsConnected)
                View.RPC("PhotonSetSpinner", RpcTarget.All);

        }
    }
    [PunRPC]
    public void PhotonSetSpinner() {
        isLidOpened = !isLidOpened;

        if (!isLidOpened)
        {
            //close the cover
            buttonCollider.center = new Vector3(0, -0.2f, 0.54f);
            lid.transform.localEulerAngles = new Vector3(-165, 0, -90);

            // start spinning
            foreach (MultiSpinTestTubeLock testTubeLock in testTubeLocks)
            {
                if (testTubeLock.isOccupied)
                {
                    if (!isSpinCoroutine)
                    {
                        StartCoroutine(ActivateSpinner());
                    }
                }
            }
            //open cover

        }
        else
        {
            buttonCollider.center = new Vector3(0, 0.65f, -0.9f);
            lid.transform.localEulerAngles = new Vector3(-270, 0, -90);

            //stop the spinning
            if (isSpinCoroutine)
            {
                StopCoroutine(ActivateSpinner());
                isSpinCoroutine = false;
            }
        }
        Debug.Log("--Lid Opens--" + isLidOpened);
    }

    [PunRPC]
    public void ShowResult() {            
        
        Debug.Log("---Show Result---RESULT : " + hasResult);
        if (!isResultCoroutine)
        {
            Debug.Log("--Result Coroutine---");
            StartCoroutine(ResultCoroutine());
        }

    }

    [PunRPC]
    public void SetCorrectImageStatus(bool canEnable) { 
        correctImage.enabled = canEnable;
    }

    IEnumerator ActivateSpinner()
    {
        isSpinCoroutine = true;
        Debug.Log("---Adjust Spin Speed");
        if (!isLidOpened)     //Spinner should only accelerate/decelerate when the lid is closed; and when awoke (default not spinning)
        {
            if (!isSpinningFinished)
            {
                if (!isSpinning)
                {
                    while (spinSpeed < 20)
                    {
                        spinSpeed += .15f;
                        isSpinning = true;
                        spinner.transform.Rotate(Vector3.forward * spinSpeed);
                        yield return null;
                    }

                }

                Debug.Log("--Spinning---Speed : " + spinSpeed);
            }

        }

        if(spinSpeed > 20)
        {
            Debug.Log("--Spinning finished---");
            isSpinning = false;
            isSpinningFinished = true;
        }


        isSpinCoroutine = false;
        yield break;
    }
    IEnumerator ResultCoroutine()
    {
        isResultCoroutine = true;

        if (!IsShowResult)
        {
            if (!isBalanced)
            {
                Debug.Log("--Result : Exploded---");
                yield return StartCoroutine(Explode());
            }
            else
            {
                Debug.Log("--Result : Correct---");
                View.RPC("SetCorrectImageStatus", RpcTarget.All, true);
                audioSource.PlayOneShot(correctClip);
            }
            IsShowResult = true;
        }
        
        isResultCoroutine = false;
        yield break;

    }

    IEnumerator Explode()
    {
        isExplodeCoroutine = true;
        //explosion.enableEmission = true;
        explosion.Play(true);
        audioSource.PlayOneShot(explodeClip);
        yield return new WaitForSeconds(1);
        explosion.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        //explosion.enableEmission = false;
        yield return null;
        isExplodeCoroutine =false;

    }

    public void ResetMultispin()
    {
        View.RPC("PhotonResetMultiSpin", RpcTarget.All);
    }
    [PunRPC]
    public void PhotonResetMultiSpin()
    {
        Debug.Log("---Reset Spin----");
        StopAllCoroutines();
        isLidOpened = false;
        isBalanced = false;
        isSpinning = false;
        isSpinningFinished = false;
        isSpinnerTriggered = false;
        isMultispinCoroutine = false;
        hasResult = false;
        IsShowResult = false;
        View.RPC("SetCorrectImageStatus", RpcTarget.All, false);


        spinSpeed = 0;

        //close the lid
        lid.transform.localEulerAngles = new Vector3(-165, 0, -90); //close

        // reset the test tube's position
        foreach (TestTube testTube in testTubeList)
        {
            Debug.Log("---Reset TestTube---" + testTube.name);
            testTube.isReset = true;
        }
        //reset the lock's status
/*        foreach (MultiSpinTestTubeLock testTubeLock in testTubeLocks) {
            Debug.Log("---Reset TestTube Lock---");
            testTubeLock.OnReset();
        }*/

    }

    void TubeCorrectSequence(int tubeNumber)
    {
        correctArrangement = new List<bool[]>();
        bool[] correctPlacings = new bool[spinnerPosCount];
        switch (tubeNumber)
        {
            case 2:
                correctPlacings = new bool[spinnerPosCount];
                correctPlacings[0] = correctPlacings[4] = true;
                correctArrangement.Add(correctPlacings);
                break;
            case 3:
                for (int i = 1; i <= 3; i++)
                {
                    correctPlacings = new bool[spinnerPosCount];
                    correctPlacings[0] = true;
                    if (i <= 2) { correctPlacings[3] = true; }
                    else { correctPlacings[2] = true; }
                    if (i == 2) { correctPlacings[6] = true; }
                    else { correctPlacings[5] = true; }
                    correctArrangement.Add(correctPlacings);
                }
                break;
            case 4:
                correctPlacings = new bool[spinnerPosCount];
                for (int i = 0; i < spinnerPosCount; i++)
                {
                    if (i % 2 == 0) { correctPlacings[i] = true; }
                    else { correctPlacings[i] = false; }
                }
                correctArrangement.Add(correctPlacings);
                break;
            case 5:
                for (int i = 1; i <= 3; i++)
                {
                    correctPlacings = new bool[spinnerPosCount];
                    correctPlacings[0] = correctPlacings[3] = correctPlacings[6] = true;
                    if (i <= 2) { correctPlacings[1] = true; }
                    else { correctPlacings[2] = true; }
                    if (i >= 2) { correctPlacings[5] = true; }
                    else { correctPlacings[4] = true; }
                    correctArrangement.Add(correctPlacings);
                }
                break;
            case 6:
                for (int i = 1; i <= 3; i++)
                {
                    correctPlacings = new bool[spinnerPosCount];
                    correctPlacings[i] = correctPlacings[i + 4] = true;
                    for (int j = 0; j < spinnerPosCount; j++) { correctPlacings[j] = !correctPlacings[j]; }
                    correctArrangement.Add(correctPlacings);
                }
                break;
            default:
                break;
        }
    }

    int CheckTestTubeAmount()
    {
        int value = 0;
        for (int i = 0; i < testTubePlaceholder.Length; i++)
        {
            if (testTubePlaceholder[i]) { value += 1; }
        }
        return value;
    }

    void CheckTestTubePos()
    {
        Transform[] testTubePos = new Transform[spinnerPosCount];
        testTubePlaceholder = new bool[spinnerPosCount];
        for (int i = 0; i < spinnerPosCount; i++)
        {
            Transform testTubeToCheck = spinner.transform.GetChild(i);
            if (testTubeToCheck.GetComponent<MultiSpinTestTubeLock>().isOccupied == true)
            {
                for (int j = 0; j < spinnerPosCount; j++)
                {
                    Transform currentTestTube = spinner.transform.GetChild((i + j) % spinnerPosCount);
                    testTubePos[j] = currentTestTube;
                    testTubePlaceholder[j] = currentTestTube.GetComponent<MultiSpinTestTubeLock>().isOccupied;
                }
                break;
            }
        }
    }

    public void SetLockedTestTube(TestTube t, bool isAdded) {

        //check all the test tubes which are used.
        if (testTubeLocks.Count > 0)
        {
            if (isAdded)
            {
                if (!lockedTestTube.Contains(t))
                {
                    lockedTestTube.Add(t);
                }

            }
            else {
                if (lockedTestTube.Contains(t))
                {
                    lockedTestTube.Remove(t);
                }
            }
        }
    }

    [PunRPC]
    public void CheckSpinnerBalance()
    {
        CheckTestTubePos();
        TubeCorrectSequence(CheckTestTubeAmount());

        int occupiedIndex = 0;
        foreach(MultiSpinTestTubeLock l in testTubeLocks)
        {
            if (l.isOccupied)
                occupiedIndex++;
        }

        Debug.Log("OccupiedIndex : " + occupiedIndex);

        if (!isLidOpened && occupiedIndex > 0)
        {
            isBalanced = false;
            for (int i = 0; i < correctArrangement.Count; i++)
            {
                if (CompareBooleanArrays(correctArrangement[i], testTubePlaceholder))
                {
                    isBalanced = true;
                    break;
                }
            }
            hasResult = true;
            Debug.Log("----Check Balance---- : " + isBalanced);
        }
    }

    private bool CompareBooleanArrays(bool[] boolA, bool[] boolB)
    {
        if (boolA.Length != boolB.Length) return false;
        else
        {
            for (int i = 0; i < boolA.Length; i++)
            {
                if (boolA[i] != boolB[i]) return false;
            }
            return true;
        }
    }


}

