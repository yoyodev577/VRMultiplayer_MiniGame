using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using System.Linq;
using HTC.UnityPlugin.Vive.VIUExample;
using Unity.Collections.LowLevel.Unsafe;

namespace AngryMouse
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        public PhotonView view;
        public List<MoeManager> moeManagers;

        //Questions
        public List<Question> questions;
        [SerializeField] private int currentIndex = 0;
        public Question currentQuestion;
        public string answer = "";

        [SerializeField] private List<PlayerButton> _playerButtons;
        [SerializeField] private List<Hammer> _hammers;
        [SerializeField] private TableButton _resetButton;
        public bool isPlayersReady = false;
        public bool IsReadyToStart = false;
        public bool isReadyTimerEnd = false;
        public bool IsGameStart = false;
        public bool IsGameEnd = false;
        public bool isLastQuestion = false;
        public bool canScore = false;
        public bool IsCorrect = false;
        public bool IsReset = false;
        public bool IsResetCoroutine = false;

        public float currentSec = 0f;
        public float timerSec = 3f;

        public bool IsReadyTimerCoroutine = false;
        public bool IsQuestionCoroutine = false;
        public IEnumerator questionCoroutine;

        public TMP_Text board;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _audioClip, _nextQuestionClip;

        // Start is called before the first frame update
        void Start()
        {
            instance = this;
            view = GetComponent<PhotonView>();
            _playerButtons = FindObjectsOfType<PlayerButton>().ToList();
            _hammers = FindObjectsOfType<Hammer>().ToList();
            moeManagers = FindObjectsOfType<MoeManager>().ToList();
            InitQuestions();
            Init();
        }
        void Init() {
            if (PhotonNetwork.IsConnected)
                view.RPC("UpdateBoardText", RpcTarget.All, "Press Ready to start the game.");
        }

        public void Update()
        {
            if (PhotonNetwork.IsConnected)
                view.RPC("PhotonUpdate", RpcTarget.All);
        }

        [PunRPC]
        public void PhotonUpdate()
        {
            if (IsReset) return;

            // when players get ready, the timer starts.
            if (isPlayersReady && IsReadyToStart && !IsReadyTimerCoroutine)
            {
                StartCoroutine(SetReadyTimerCoroutine(timerSec));
            }

            // when timer count down as 0
            if (!IsGameStart && isReadyTimerEnd&& !IsGameEnd)
            {
                StartGame();
            }

            if (IsGameStart && currentIndex == questions.Count && IsCorrect) {
                isLastQuestion = true;
            }

            //end the game
            if (IsGameStart && isLastQuestion && !IsGameEnd)
            {
                EndGame();
            }

        }

        public void InitQuestions()
        {
            questions.Add(new Question(
            "What are not major disinfectants for lab disinfection?\n"+ 
            "A. Hypochlorites\n" + 
            "B. Formaldehyde\n" + 
            "C. Xylene\n" +
            "D. Ebola virus disease\n" +
            "E. Pink Eye"
            , "C"));


            questions.Add(new Question(
                "Which one is not belong to common behavior that can be exposed to bloodborne pathogens?\n"
                + "A. Splashes to blood\n" 
                + "B. Contact of eyes\n"
                + "C. Bites and knife wounds\n" 
                + "D. Shake hands\n"
                + "E. Pink Eye"
                , "D"));


            questions.Add(new Question("Which communicable disease is NOT found to be common?\n" +
                    "A. Cold\n" +
                    "B. Hepatitis A\n" +
                    "C. Chickenpox\n" +
                    "D. Ebola virus disease\n" +
                    "E. Pink Eye"
                    , "D"));
        }

        [PunRPC]
        public void SetQuestion()
        {
            if (!IsGameStart || IsGameEnd) return;
            string text = "";
            if(currentIndex < questions.Count)
            {
                Debug.Log("--Question:" + currentIndex);
                currentQuestion = questions[currentIndex];

                answer = currentQuestion.answerText;
                text = "<b>Question " + (currentIndex + 1) + "</b>:\n" + questions[currentIndex].questionText;


                view.RPC("UpdateBoardText", RpcTarget.All, text);
            }

        }

        [PunRPC]
        public void UpdateBoardText(string _text) {
            board.text = _text;
        }

        public void PlayerReady()
        {
            if (PhotonNetwork.IsConnected)
                view.RPC("PhotonPlayerReady", RpcTarget.All);

        }

        [PunRPC]
        public void PhotonPlayerReady()
        {
            Debug.Log("---Waiting For Players Ready---");
            foreach (PlayerButton button in _playerButtons)
            {
                if (button.isPressed)
                {
                    isPlayersReady = true;
                }
            }
        }
        public void ReadyToStart()
        {
            if (PhotonNetwork.IsConnected)
                view.RPC("PhotonReadyToStart", RpcTarget.All);

        }

        [PunRPC]
        public void PhotonReadyToStart()
        {
            Debug.Log("---Game Ready To Start---");
            if (isPlayersReady && !IsReadyToStart)
            {
                if (_playerButtons[0].isPressed && _playerButtons[1].isPressed)
                {
                    IsReadyToStart = true;
                }
            }
        }


        public void StartGame()
        {

            if (PhotonNetwork.IsConnected)
                view.RPC("PhotonStartGame", RpcTarget.All);

        }

        [PunRPC]
        public void PhotonStartGame()
        {

            if (IsGameEnd) return;

            IsGameStart = true;
            Debug.Log("---Game Start---");

            //first time
            foreach (MoeManager m in moeManagers)
            {

                m.RandomPickMoes();
                m.PopMoes();
            }
            view.RPC("SetQuestion", RpcTarget.All);


            if (!IsQuestionCoroutine && questionCoroutine==null)
            {
                questionCoroutine = SetQuestionBoardCoroutine();
                StartCoroutine(questionCoroutine);
            }

        }


        public void EndGame() {
            if (PhotonNetwork.IsConnected)
                view.RPC("PhotonEndGame", RpcTarget.All);
        }

        [PunRPC]
        public void PhotonEndGame()
        {
            IsGameEnd = true; 
            Debug.Log("---Game End---");
            if (questionCoroutine!=null)
            {
                Debug.Log("---Stop Questions--");
                StopCoroutine(questionCoroutine);
                questionCoroutine = null;
            }
            ShowResult();
  

        }

        public void ResetGame()
        {
            if (PhotonNetwork.IsConnected)
                view.RPC("PhotonResetGame", RpcTarget.All);
        }

        [PunRPC]
        public void PhotonResetGame() {
            Debug.Log("---Game Reset---");

            IsReset = true;

            foreach (MoeManager m in moeManagers)
            {
                Debug.Log("---Reset moes---");
                m.ResetMachine();
            }

            foreach(Hammer hammer in _hammers) {
                Debug.Log("---Reset Hammer--");
                hammer.ResetPos();
            }

            questionCoroutine = null;
            currentIndex = 0;
            answer = "";
            currentSec = 0;
            currentQuestion = null;
            isPlayersReady = false;
            IsReadyToStart = false;
            isReadyTimerEnd = false;
            isLastQuestion = false;
            IsGameStart = false;
            IsGameEnd = false;

            canScore = false;
            IsCorrect = false;
            IsQuestionCoroutine = false;
            IsReadyTimerCoroutine = false;


            if (!IsResetCoroutine)
                StartCoroutine(ResetCoroutine());

            view.RPC("UpdateBoardText", RpcTarget.All, "Press Ready to start the game.");

        }
        public bool CheckAnswer(string _text) { 
            if(_text ==answer)
                return true;
            else
                return false;
        }

        public void ShowResult() {
            string text = "";
            text = "The game has ended";

            Debug.Log("---Show game result---");
            if (moeManagers[0].score > moeManagers[1].score)
            {
                text = "The game has ended.\nPlayer :" + moeManagers[0].playerNum + " wins";
            }
            else if (moeManagers[0].score < moeManagers[1].score)
            {
                text = "The game has ended.\nPlayer :" + moeManagers[1].playerNum + " wins";
            }
            else
            {
                text = "The game has ended";
            }

            view.RPC("UpdateBoardText", RpcTarget.All, text);
        }


        IEnumerator SetQuestionBoardCoroutine()
        {

            IsQuestionCoroutine =true;
            currentIndex = 0;
            canScore = true;

        
            while (IsGameStart && !IsGameEnd )
            {
    
                if (IsCorrect) {
                    Debug.Log("---Question:" + currentIndex + " correct");

                    canScore = false;


                    foreach (MoeManager m in moeManagers)
                    {
                        m.HideMoes();
                    }
                    //set question
 
                    if (currentIndex < questions.Count)
                    {
                        view.RPC("UpdateBoardText", RpcTarget.All, "Next Question");
                        _audioSource.PlayOneShot(_nextQuestionClip);
                        yield return new WaitForSeconds(0.3f);
                        currentIndex++;
                        foreach (MoeManager m in moeManagers)
                        {
                            m.RandomPickMoes();
                            yield return new WaitForSeconds(0.5f);
                            m.PopMoes();
                        }
                    }

                    IsCorrect = false;

                }
                yield return new WaitForFixedUpdate();
                view.RPC("SetQuestion", RpcTarget.All);

                //set and pop random moes for it.

                canScore = true;

            }


            IsQuestionCoroutine = false;
        }

        IEnumerator SetReadyTimerCoroutine(float seconds)
        {
            IsReadyTimerCoroutine = true;
            currentSec = seconds;
            view.RPC("UpdateBoardText",RpcTarget.All, currentSec.ToString());
            while (currentSec >= 0)
            {
                _audioSource.PlayOneShot(_audioClip);
                view.RPC("UpdateBoardText", RpcTarget.All, currentSec.ToString());
                yield return new WaitForSeconds(1f);
                currentSec -= 1;

            }

            if (currentSec <= 0)
            {
                _audioSource.Stop();
                isReadyTimerEnd = true;
                IsReadyToStart = false;
                view.RPC("UpdateBoardText", RpcTarget.All, "Game Starts");
            }
            yield return null;
            IsReadyTimerCoroutine = false;
        }

        IEnumerator ResetCoroutine()
        {
            IsResetCoroutine = true;
            yield return new WaitForSeconds(2f);
            _resetButton.ResetButton();
            IsReset = false;
            IsResetCoroutine = false;
        }
    }
}
