using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;
using AngryMouse;
using HTC.UnityPlugin.Vive;
using TMPro;

public class MoeManager : MonoBehaviour
{
    public PhotonView view;
    public GameManager manager;
    public Hammer hammer;
    public List<Moe> moes;
    public List<Moe> temp;
    public List<Moe> popList = new List<Moe>();
    public string[] answerList = { "A", "B", "C", "D", "E" };
    public bool isEnabled = false;
    public int maxMoes = 4;
    public int score = 0;
    public bool isScored = false;
    public bool isCoroutine = false;
    public bool isResetScoreCoroutine = false;
    public TextMeshProUGUI scoreText;
    public int playerNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<GameManager>();
        hammer = GetComponentInChildren<Hammer>();
        view = GetComponent<PhotonView>();
        view.RPC("HideAllMoes", RpcTarget.All);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SetEngine(true);
        }

    }

    public void PhotonSetEngine(bool _isEnabled) {
        view.RPC("SetEngine", RpcTarget.All, _isEnabled);
    }

    [PunRPC]
    public void SetEngine(bool _isEnabled) {

        isEnabled = _isEnabled;

        if( _isEnabled &&!isCoroutine)
        {
            StartCoroutine(MoeCoroutine());
        }
        else if( !_isEnabled )
        {
            if (isCoroutine)
                StopCoroutine(MoeCoroutine());
        }
    }


    [PunRPC]
    public void RandomPickMoes()
    {
        popList.Clear();
        temp.Clear();

        temp.AddRange(moes);

        while (popList.Count < maxMoes)
        {
            int r = Random.Range(0, temp.Count);
            if (!popList.Contains(temp[r]))
            {
               // Debug.Log("Pop " + temp[r].name);
                popList.Add(temp[r]);
                temp.RemoveAt(r);
            }

        }
    }

    [PunRPC]
    public void PopMoes() {
        if (popList.Count == 0) return;

        for(int i = 0; i < popList.Count; i++)
        {
            // first one = A, second one = B
            popList[i].SetCurrentAns(answerList[i]);
            popList[i].SetPop(true);
        }
    }

    [PunRPC]
    public void HideAllMoes() 
    { 
        for(int i = 0; i < moes.Count; i++)
        {
            moes[i].SetPop(false);
            moes[i].SetHitStatus(false);
        }
    
    }

    public void CheckScore(string _answer) {
        view.RPC("PhotonScore", RpcTarget.All,_answer);
    }

    [PunRPC]
    public void PhotonScore(string _answer) {
        if (manager.CheckAnswer(_answer) 
            && manager.canScore &&!manager.IsCorrect)
        {
            // find the correct answer, and start to the next question.
            manager.IsCorrect = true;
       /*     if (!isScored)
            {*/
                this.score++;
               // isScored = true;
/*                if(!isResetScoreCoroutine)
                {
                    StartCoroutine(ResetScoreCoroutine());
                }*/
            //}
            scoreText.text = "Score: " + score.ToString();
        }
        

    }

    public void ResetMachine()
    {
        view.RPC("PhotonResetMachine", RpcTarget.All);
    }

    [PunRPC]
    public void PhotonResetMachine() {
        hammer.ResetPos();
        isEnabled = false;
        isScored = false;
        isResetScoreCoroutine = false;
        score = 0;
        scoreText.text = "Score: " + score.ToString();
        view.RPC("HideAllMoes", RpcTarget.All);
    }

    public IEnumerator MoeCoroutine() {
        isCoroutine = true;
        while (isEnabled)
        {
            //RandomPickMoes();
            view.RPC("RandomPickMoes", RpcTarget.All);
            yield return new WaitForFixedUpdate();

            //PopMoes();
            view.RPC("PopMoes", RpcTarget.All);

            yield return new WaitForSeconds(3);
            //HideAllMoes();
            view.RPC("HideAllMoes", RpcTarget.All);
        }
        isCoroutine = false;

    }

    IEnumerator ResetScoreCoroutine() {
        isResetScoreCoroutine = true;
        yield return new WaitForFixedUpdate();
        isScored = false;
        isResetScoreCoroutine = false;
    }

}
