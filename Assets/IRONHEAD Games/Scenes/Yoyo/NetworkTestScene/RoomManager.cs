using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SyncTest
{
    public class RoomManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private bool IsJoined = false;
        public Button createBtn, joinBtn;
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

            createBtn.onClick.AddListener(() => OnCreateRoom());
        }

        public void OnCreateRoom()
        {
            if (!IsJoined)
            {
                RoomOptions roomOptions = new RoomOptions();
                roomOptions.MaxPlayers = 8;
                PhotonNetwork.CreateRoom("ABC", roomOptions, null);
                IsJoined = true;
            }
            else
            {
                PhotonNetwork.JoinRoom("ABC");
                IsJoined = true;
            }
        }

        public override void OnJoinedRoom()
        {
            PhotonNetwork.JoinRoom("ABC");
            IsJoined = true;
        }
    }
}
