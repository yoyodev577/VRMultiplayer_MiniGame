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
        view.RPC("PhotonHideAllMoes", RpcTarget.All);
    }


    public void SetEngine(bool _isEnabled) {
        view.RPC("PhotonSetEngine", RpcTarget.All, _isEnabled);
    }



    public void RandomPickMoes() {

        view.RPC("PhotonRandomPickMoes", RpcTarget.All);
    }

    public void PopMoes()
    {
        view.RPC("PhotonPopMoes", RpcTarget.All);
    }

    public void HideAllMoes()
    {
        view.RPC("PhotonHideAllMoes", RpcTarget.All);
    }


    [PunRPC]
    public void PhotonSetEngine(bool _isEnabled)
    {

        isEnabled = _isEnabled;

        if (_isEnabled && !isCoroutine)
        {
            StartCoroutine(MoeCoroutine());
        }
        else if (!_isEnabled)
        {
            if (isCoroutine)
                StopCoroutine(MoeCoroutine());
        }
    }


    [PunRPC]
    public void PhotonRandomPickMoes()
    {
        popList.Clear();
        temp.Clear();

        temp.AddRange(moes);
        Debug.Log("---Random pick moes---");
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
    public void PhotonPopMoes() {
        if (popList.Count == 0) return;

        Debug.Log("---Pop moes---");
        for (int i = 0; i < popList.Count; i++)
        {
            // first one = A, second one = B
            popList[i].SetCurrentAns(answerList[i]);
            popList[i].SetPop(true);
        }
    }

    [PunRPC]
    // lerping
    public void PhotonHideAllMoes() 
    {
        Debug.Log("---Hide all moes---");
        for (int i = 0; i < moes.Count; i++)
        {
            moes[i].SetPop(false);
            moes[i].SetHitStatus(false);
        }
    
    }
    [PunRPC]
    public void PhotonHideAllMoesNow() 
    {
        Debug.Log("---Hide all moes now!---");
        for (int i = 0; i < moes.Count; i++)
        {
            moes[i].HideNow();
        }
    }

    public void CheckScore(string _answer) {

        if (!manager.IsGameStart || manager.IsGameEnd) return;

        view.RPC("PhotonScore", RpcTarget.All,_answer);
    }

    [PunRPC]
    public void PhotonScore(string _answer)
    {
        if (manager.CheckAnswer(_answer))
        {
            if (manager.canScore && !manager.IsCorrect)
            {
                // find the correct answer, and start to the next question.
                manager.IsCorrect = true;
                this.score++;
            }
        }
        else {
            if (manager.canScore) {
                this.score--;
            }
        }
        scoreText.text = "Score: " + score.ToString();

    }

    public void ResetMachine()
    {
        view.RPC("PhotonResetMachine", RpcTarget.All);
    }

    [PunRPC]
    public void PhotonResetMachine() {

        //StopCoroutine(MoeCoroutine());
        hammer.ResetPos();
        isEnabled = false;
        isScored = false;
        isResetScoreCoroutine = false;
        isCoroutine =false;
        score = 0;
        scoreText.text = "Score: " + score.ToString();
        view.RPC("PhotonHideAllMoesNow", RpcTarget.All);
    }

    public IEnumerator MoeCoroutine()
    {
        isCoroutine = true;
        while (isEnabled)
        {


            view.RPC("PopMoes", RpcTarget.All);

            yield return new WaitForSeconds(3);

            view.RPC("HideAllMoes", RpcTarget.All);
            yield return new WaitForSeconds(1);
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
