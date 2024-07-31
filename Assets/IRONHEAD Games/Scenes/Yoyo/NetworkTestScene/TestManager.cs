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
        public PhotonView view;
        public SpawnManager spawnManager;
        public string roomName = "Test";
        public TextMeshProUGUI roomText;

        // Start is called before the first frame update
        void Start()
        {
            view = GetComponent<PhotonView>();
            spawnManager = GetComponent<SpawnManager>();
        }

        public void Update()
        {
            roomText.text = "Player : " + PhotonNetwork.PlayerList.Length.ToString(); ;

            foreach (Player player in PhotonNetwork.PlayerList)
            {
                roomText.text += "\n" + player.NickName;
               // print(player.NickName);
            }
 

        }

    }
}
