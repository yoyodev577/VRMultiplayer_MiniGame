using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;


namespace SyncTest
{
    public class SyncTest : MonoBehaviour
    {
        public PhotonView photonView;
        public TextMeshProUGUI playerName_Text;
        public TextMeshProUGUI booleanText;
        public bool isEnabled = false;
        // Start is called before the first frame update
        void Start()
        {
            photonView = GetComponent<PhotonView>();

            if (photonView != null && photonView.Owner.NickName != null)
                playerName_Text.text = photonView.Owner.NickName;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A)) {

                photonView.RPC("SetStatus", RpcTarget.All);
            }
        }

        [PunRPC]
        public void SetStatus()
        {
            isEnabled = !isEnabled;
            booleanText.text = isEnabled.ToString();
        }
    }
}
