using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using System.Linq;
using HTC.UnityPlugin.Vive.VIUExample;

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
        public bool isPlayersReady = false;
        public bool IsReadyToStart = false;
        public bool IsGameStart = false;
        public bool IsGameEnd = false;
        public bool IsCorrect = false;
        public bool IsReset = false;
        public bool IsResetCoroutine = false;

        public float currentSec = 0f;
        public float timerSec = 3f;

        public bool IsReadyTimerCoroutine = false;
        public bool IsQuestionCoroutine = false;

        public TMP_Text board;
        private AudioSource _audioSource;
        [SerializeField] private AudioClip _audioClip;

        // Start is called before the first frame update
        void Start()
        {
            instance = this;
            view = GetComponent<PhotonView>();
            _playerButtons = FindObjectsOfType<PlayerButton>().ToList();
            moeManagers = FindObjectsOfType<MoeManager>().ToList();
            _audioSource = GetComponent<AudioSource>();
            InitQuestions();
            Init();
        }
        void Init() {
            if (PhotonNetwork.IsConnected)
                view.RPC("UpdateBoardText", RpcTarget.AllBuffered, "Press Ready to start the game.");
        }

        // Update is called once per frame
        void Update()
        {
            if (IsCorrect)
            {
                if (!IsQuestionCoroutine)
                {
                    StartCoroutine(SetQuestionBoardCoroutine());
                    IsCorrect = false;
                }
            }

            // when players get ready, the timer starts.
            if (isPlayersReady && IsReadyToStart && !IsReadyTimerCoroutine)
            {
                StartCoroutine(SetReadyTimerCoroutine(timerSec));
            }
            // start the game after the count down.
            if (IsGameStart && !IsGameEnd)
            {
                StartGame();
            }

            //end the game when it is the last question.
            if (IsGameStart && IsGameEnd)
            {
                EndGame();
            }

        }

        public void InitQuestions()
        {
            questions.Add(new Question(
            "What are not major disinfectants for lab disinfection?\n"
            + "A. Hypochlorites\n" + "B. Formaldehyde\n" + "C. Xylene"
            , "C"));


            questions.Add(new Question(
                "Which one is not belong to common behavior that can be exposed to bloodborne pathogens?\n"
                + "A. Splashes to blood\n" + "B. Contact of eyes\n"
                + "C. Bites and knife wounds\n" + "D. Shake hands"
                , "D"));


            questions.Add(new Question("Which communicable disease is NOT found to be common?\n" +
                    "A. Cold\n" +
                    "B. Hepatitis A\n" +
                    "C. Chickenpox\n" +
                    "D. Ebola virus disease\n" +
                    "E. Pink Eye"
                    , "D"));
        }

        private void ShowQuestion()
        {
            currentQuestion = questions[currentIndex];
            string text = "Question " + currentIndex + ":\n" + questions[currentIndex].questionText;
            view.RPC("UpdateBoardText", RpcTarget.AllBuffered, text);
        }

        [PunRPC]
        public void UpdateBoardText(string _text) {
            board.text = _text;
        }

        public void PlayerReady()
        {
            if (PhotonNetwork.IsConnected)
                view.RPC("PhotonPlayerReady", RpcTarget.AllBuffered);

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
                view.RPC("PhotonReadyToStart", RpcTarget.AllBuffered);

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
                view.RPC("PhotonStartGame", RpcTarget.AllBuffered);

        }

        [PunRPC]
        public void PhotonStartGame()
        {
            Debug.Log("---Game Start---");
            foreach (MoeManager m in moeManagers)
            {
                m.PhotonSetEngine(true);
            }
            answer = currentQuestion.answerText;
            ShowQuestion();
            
        }


        public void EndGame() {
            if (PhotonNetwork.IsConnected)
                view.RPC("PhotonEndGame", RpcTarget.AllBuffered);
        }

        [PunRPC]
        public void PhotonEndGame() {
            Debug.Log("---Game End---");
            foreach (MoeManager m in moeManagers)
            {
                m.PhotonSetEngine(false);
            }
        }

        public void ResetGame()
        {
            if (PhotonNetwork.IsConnected)
                view.RPC("PhotonResetGame", RpcTarget.AllBuffered);
        }

        [PunRPC]
        public void PhotonResetGame() {
            Debug.Log("---Game Reset---");
            currentIndex = 0;
            currentQuestion = null;
            isPlayersReady = false;
            IsReadyToStart = false;
            IsGameStart = false;
            IsGameEnd = false;
            IsReset = true;


            if (!IsResetCoroutine)
                StartCoroutine(ResetCoroutine());

            view.RPC("UpdateBoardText", RpcTarget.AllBuffered, "Press Ready to start the game.");
        }
        public bool CheckAnswer(string _text) { 
            if(_text ==answer)
                return true;
            else
                return false;
        }

        public void ShowResult() {
            string text = "";
            
            if (moeManagers[0].score > moeManagers[1].score)
            {
                text = "The game has ended.\nPlayer :" + moeManagers[0].playerNum + " wins";
            }
            else if (moeManagers[0].score < moeManagers[1].score)
            {
                text = "The game has ended.\nPlayer :" + moeManagers[1].playerNum + " wins";
            }
            else {
                text = "The game has ended";
            }

            view.RPC("UpdateBoardText", RpcTarget.AllBuffered, text);
        }


        IEnumerator SetQuestionBoardCoroutine()
        {
            IsQuestionCoroutine =true;
            yield return new WaitForSeconds(3f);        
            if (currentIndex > questions.Count)
            {
                IsGameEnd = true;
                ShowResult();
            }
            else
            {
                currentIndex += 1;
                ShowQuestion();
            }
            IsQuestionCoroutine = false;
        }

        IEnumerator SetReadyTimerCoroutine(float seconds)
        {
            IsReadyTimerCoroutine = true;
            currentSec = seconds;
            view.RPC("UpdateBoardText",RpcTarget.AllBuffered, currentSec.ToString());
            while (currentSec >= 0)
            {
                _audioSource.PlayOneShot(_audioClip);
                view.RPC("UpdateBoardText", RpcTarget.AllBuffered, currentSec.ToString());
                yield return new WaitForSeconds(1f);
                currentSec -= 1;

            }

            if (currentSec <= 0)
            {
                _audioSource.Stop();
                IsReadyToStart = false;
                IsGameStart = true;
                view.RPC("UpdateBoardText", RpcTarget.AllBuffered, "Game Starts");
            }
            yield return null;
            IsReadyTimerCoroutine = false;
        }

        IEnumerator ResetCoroutine()
        {
            IsResetCoroutine = true;
            yield return new WaitForSeconds(2f);
            IsReset = false;
            IsResetCoroutine = false;
        }
    }
}
