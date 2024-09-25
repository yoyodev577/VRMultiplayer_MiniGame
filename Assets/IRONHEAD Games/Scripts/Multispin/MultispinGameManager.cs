using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using MultiplayerKitForHVR.General;

public class MultispinGameManager : MonoBehaviour
{
    public static MultispinGameManager instance;
    private PhotonView _view;

    [SerializeField] private List<PlayerButton> _playerButtons;
    [SerializeField] private TableButton resetButton;
    [SerializeField] private List<MultiSpin> _multiSpins;
    private List<MultiSpinGame> _multiSpinsGame;

    [SerializeField] 
    private GameState _gameState = GameState.Default;
    public bool isPlayersReady = false;
    public bool IsReadyToStart = false;
    public bool IsGameStart = false;
    public bool IsGameEnd = false;
    public bool IsReset = false;
    public bool IsResetCoroutine = false;

    public float currentSec = 0f;
    public float timerSec = 3f;
    public bool IsReadyTimerCoroutine = false;

    public TMP_Text uiBoard;
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioClip;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
        _view = GetComponent<PhotonView>();
        _playerButtons = FindObjectsOfType<PlayerButton>().ToList();
        _multiSpins = FindObjectsOfType<MultiSpin>().ToList();
        _multiSpinsGame = FindObjectsOfType<MultiSpinGame>().ToList();
        _audioSource = GetComponent<AudioSource>();
        InitGame();
    }

    // Update is called once per frame
    void Update()
    {
        // when players get ready, the timer starts.
        if (isPlayersReady && IsReadyToStart && !IsReadyTimerCoroutine)
        {
            StartCoroutine(SetReadyTimerCoroutine(timerSec));
        }

        // start the game after the count down.
        if (!IsGameStart && IsReadyToStart && !IsGameEnd)
        {
            StartGame();
        }

        //end the game when both players have the results.
        if (IsGameStart && !IsGameEnd) {

            if (_multiSpins[0].hasResult && _multiSpins[1].hasResult)
            {
                EndGame();
            }
        }

        //reset the game state
        if(IsGameStart && IsReset)
        {
            if (PhotonNetwork.IsConnected)
                _view.RPC("PhotonResetGame", RpcTarget.All);
        }

    }

    void InitGame() {

        _gameState = GameState.Default;
        _view.RPC("UpdateBoardText", RpcTarget.All, "Press Ready to start the game.");
    }

    public void WaitForPlayersReady()
    {
        if (PhotonNetwork.IsConnected)
            _view.RPC("PhotonWaitForPlayersReady", RpcTarget.All);
    }

    [PunRPC]
    public void PhotonWaitForPlayersReady() {
        Debug.Log("---Waiting For Players Ready---");
        foreach (PlayerButton button in _playerButtons)
        {
            if (button.isPressed)
            {
                isPlayersReady = true;
                _gameState = GameState.PlayersReady;
            }
            else
            {
                isPlayersReady = false;
                _gameState = GameState.Default;
            }
        }
    }

    public void ReadyToStart()
    {
        if (PhotonNetwork.IsConnected)
            _view.RPC("PhotonReadyToStart", RpcTarget.All);
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
                _gameState = GameState.ReadyToStart;
                

            }
        }
    }

    public void StartGame()
    {
        if (PhotonNetwork.IsConnected)
            _view.RPC("PhotonStartGame", RpcTarget.All);

    }

    [PunRPC]
    public void PhotonStartGame()
    {
        Debug.Log("---Start the game---");

        _gameState = GameState.StartGame;
        IsGameStart = true;
        var sockets = FindObjectsOfType<SocketNetworkBehaviour>();
        foreach (var s in sockets)
        {
            s.GrabInitialize();
        }
    }

    public void EndGame()
    {
        if (PhotonNetwork.IsConnected)
            _view.RPC("PhotonEndGame", RpcTarget.All);
    }

    [PunRPC]
    public void PhotonEndGame() {


        _gameState = GameState.EndGame;
        IsGameEnd = true;
        ShowResult();
    }



    public void ResetGame()
    {
        _gameState = GameState.ResetGame;
        IsReset = true;
        if (PhotonNetwork.IsConnected)
            _view.RPC("PhotonResetGame", RpcTarget.All);
    }

    [PunRPC]
    public void PhotonResetGame()
    {
        Debug.Log("---Game Reset---");
        if(!IsResetCoroutine)
            StartCoroutine(ResetCoroutine());
    }

    public void ShowResult() {
        string text = "";

        if (_multiSpins[0].isBalanced && !_multiSpins[1].isBalanced)
        {
            text = "The game has ended.\nPlayer :" + _multiSpins[0].playerNum + " wins";

        }
        else if (!_multiSpins[0].isBalanced && _multiSpins[1].isBalanced) {

            text = "The game has ended.\nPlayer :" + _multiSpins[1].playerNum + " wins";
        }
        else if (_multiSpins[0].isBalanced && _multiSpins[1].isBalanced)
        {
            text = "The game has ended.Both players win!";
        }
        else if (!_multiSpins[0].isBalanced && !_multiSpins[1].isBalanced)
        {
            text = "The game has ended.Both players lose :(!";
        }

        _view.RPC("UpdateBoardText", RpcTarget.All,text);
    }

    [PunRPC]
    public void UpdateBoardText(string text)
    {
        uiBoard.text = text;

    }

    IEnumerator SetReadyTimerCoroutine(float seconds)
    {
        IsReadyTimerCoroutine = true;
        currentSec = seconds;
        _view.RPC("UpdateBoardText", RpcTarget.All, currentSec.ToString());
        while (currentSec >= 0)
        {
            _audioSource.PlayOneShot(_audioClip);
            _view.RPC("UpdateBoardText", RpcTarget.All, currentSec.ToString());
            yield return new WaitForSeconds(1f);
            currentSec -= 1;

        }

        if (currentSec <= 0)
        {
            _audioSource.Stop();
            IsReadyToStart = false;
            IsGameStart = true;
            _view.RPC("UpdateBoardText", RpcTarget.All, "Game Starts");
        }
        yield return null;
        IsReadyTimerCoroutine = false;
    }

    IEnumerator ResetCoroutine()
    {
        IsResetCoroutine = true;
        Debug.Log("---Reset Coroutine---");
        isPlayersReady = false;
        IsReadyToStart = false;
        IsGameStart = false;
        IsGameEnd = false;
        IsReadyTimerCoroutine = false;

        resetButton.ResetButton();
        foreach(PlayerButton button in _playerButtons)
        {
            button.ResetButton();
        }
        /*
        foreach(MultiSpin m in _multiSpins)
        {
            m.ResetMultispin();
        }
        */
        foreach (MultiSpinGame m in _multiSpinsGame)
        {
            m.ResetGame();
        }
        
        //InitGame();
        yield return new WaitForSeconds(2f);
        IsReset = false;
        IsResetCoroutine = false;
        InitGame();
    }

}
