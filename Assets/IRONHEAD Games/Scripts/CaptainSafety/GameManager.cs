using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Photon.Pun;
using TMPro;

namespace CaptainSafety
{
    public class GameManager : MonoBehaviour
    {
        private PhotonView view;
        private VideoPlayer videoPlayer;
        private SpawnManager spawnManager;

        [SerializeField] private PlayerGameController[] playerGameControllers;

        public bool isVideoPlayed = false;
        public bool isVideoCoroutine = false;
        public bool isAnimationPlayed = false;
        public bool isAnimationCoroutine = false;
        public bool isReadyTimerCoroutine = false;
        public bool isReadyToStart = false;
        public bool isGameStart = false;
        public bool isGameEnd = false;
        public bool isReset = false;

        public float currentSec = 0;

        public GameObject videoPanel, boardPanel;
        public TMP_Text boardText;

        public AudioSource bgmSource, sfxSource;
        public AudioClip countDownClip, fallClip, fireClip;
        // Start is called before the first frame update
        void Start()
        {
            view = GetComponent<PhotonView>();
            spawnManager = FindObjectOfType<SpawnManager>();
            videoPlayer = FindObjectOfType<VideoPlayer>();
            videoPanel.SetActive(true);
            boardPanel.SetActive(false);
            playerGameControllers = FindObjectsOfType<PlayerGameController>();
            //videoPlayer.Play();

        }

        // Update is called once per frame
        void Update()
        {
           // PhotonUpdate();
            if (PhotonNetwork.IsConnected)
                view.RPC("PhotonUpdate", RpcTarget.All);

        }
        [PunRPC]
        public void EnableCameraVfx() {
            if (playerGameControllers.Length == 0) return;

            for (int i = 0; i < playerGameControllers.Length; i++) {
                playerGameControllers[i].SetCameraEffectObj(true);
            }
        }

        [PunRPC]
        public void PhotonUpdate() {

            if (isVideoPlayed && !videoPlayer.isPlaying)
            {
                if (!isReadyToStart)
                {
                    videoPanel.SetActive(false);
                    boardPanel.SetActive(true);

                    // activate eye hurt effect
                    view.RPC("EnableCameraVfx", RpcTarget.All);

                    isReadyToStart = true;
                    if (!isReadyTimerCoroutine)
                    {
                        StartCoroutine(SetReadyTimerCoroutine(5));
                    }
                }

            }
        }

        public void StartGame()
        {
           //PhotonStartGame();

            if (PhotonNetwork.IsConnected)
                view.RPC("PhotonStartGame", RpcTarget.All);
        }


        [PunRPC]
        public void PhotonStartGame()
        {
            if (!isVideoPlayed)
            {
                if (!isVideoCoroutine)
                    StartCoroutine(VideoCoroutine());

            }

        }



        public void EndGame() {

            if (PhotonNetwork.IsConnected)
                view.RPC("PhotonEndGame", RpcTarget.All);
        }

        [PunRPC]
        public void PhotonEndGame()
        {
            isGameEnd = true;
        }

        public void ResetGame()
        {
            if (PhotonNetwork.IsConnected)
                view.RPC("PhotonResetGame", RpcTarget.All);

        }


        [PunRPC]
        public void PhotonResetGame()
        {
            isAnimationPlayed = false;
            isReadyTimerCoroutine = false;
            isVideoCoroutine = false;
            isReadyToStart = false;
            isVideoPlayed = false;
            isGameStart = false;
            isGameEnd = false;
            currentSec = 0;

            videoPanel.SetActive(true);
            boardPanel.SetActive(false);
        }

        [PunRPC]
        public void UpdateBoardText(string txt)
        {
            boardText.text = txt;
        }

        IEnumerator VideoCoroutine() {
            isVideoCoroutine = true;
            yield return new WaitForSeconds(1);
            videoPlayer.Play();
            yield return new WaitUntil(()=>!videoPlayer.isPlaying);
            isVideoPlayed = true;
            isVideoCoroutine = false;
               
        }

        IEnumerator SetReadyTimerCoroutine(float seconds)
        {
            isReadyTimerCoroutine = true;
            currentSec = seconds;
            view.RPC("UpdateBoardText", RpcTarget.All,currentSec.ToString());


            while (currentSec >= 0)
            {
                sfxSource.PlayOneShot(countDownClip);
                view.RPC("UpdateBoardText", RpcTarget.All, currentSec.ToString());
                yield return new WaitForSeconds(1f);
                currentSec -= 1;
            }

            if (currentSec <= 0)
            {
                sfxSource.Stop();
                isReadyToStart = true;
                isGameStart = true;
                view.RPC("UpdateBoardText", RpcTarget.All, "Game Starts");
            }

            isReadyTimerCoroutine = false;
            yield return null;
        }
    }
}
