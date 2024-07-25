using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using TMPro;


namespace SyncTest
{
    public class TestManager : MonoBehaviourPunCallbacks
    {
        public string roomName = "Test";
        public TextMeshProUGUI roomText;
        [SerializeField] private bool isJoined = false;

        [SerializeField] private List<RoomInfo> currentRoomList;
        // Start is called before the first frame update
        void Start()
        {
            PhotonNetwork.AutomaticallySyncScene = true;

            if (!PhotonNetwork.IsConnectedAndReady)
            {
                PhotonNetwork.ConnectUsingSettings();
            }
            else
            {
                PhotonNetwork.JoinLobby();
            }

        }

        private void Update()
        {

           // roomText.text = "Room :"+ PhotonNetwork.CountOfRooms.ToString();
        }

        public void CreateRoom() {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 8;
            PhotonNetwork.CreateRoom(roomName, roomOptions, null);
            Debug.Log("CreateRoom");
        }

        public void JoinRoom()
        {
            PhotonNetwork.JoinRandomRoom();
            isJoined = true;
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            Debug.LogErrorFormat("Room creation failed with error code {0} and error message {1}", returnCode, message);
        }

        public override void OnJoinedRoom()
        {
            PhotonNetwork.LoadLevel("PhotonNetworkTest_Game");
            Debug.Log("Join Room ");
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            CreateRoom();
        }

    }
}
