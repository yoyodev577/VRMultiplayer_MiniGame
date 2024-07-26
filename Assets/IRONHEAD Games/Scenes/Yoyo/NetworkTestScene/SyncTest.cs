using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;


namespace SyncTest
{
    public class SyncTest : MonoBehaviour
    {
       
        public string playerName;
        public PhotonView photonView;
        public TextMeshProUGUI playerName_Text;
        public TextMeshProUGUI booleanText;
        public bool isEnabled = false;
        public Material blueColor, orangeColor;
        public MeshRenderer mR;

        // Start is called before the first frame update
        void Start()
        {
            photonView = GetComponent<PhotonView>();
            mR = GetComponent<MeshRenderer>();

            if (photonView != null && photonView.Owner.NickName != null)
            {
                playerName = photonView.Owner.NickName;
                if (photonView.Owner.IsMasterClient)
                {
                    playerName_Text.text = "Host";
                }
                else {
                    playerName_Text.text = "Client";
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (photonView.IsMine)
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    //photonView.RPC("SetStatus", RpcTarget.AllBuffered);
                    photonView.RPC("SetBlueColor", RpcTarget.All);
                }
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    photonView.RPC("SetOrangeColor", RpcTarget.All);

                }
            }

        }

        [PunRPC]
        public void SetStatus()
        {
            isEnabled = !isEnabled;
            booleanText.text = isEnabled.ToString();
        }

        [PunRPC]
        public void SetBlueColor() {
            mR.material = blueColor;
        }

        [PunRPC]
        public void SetOrangeColor()
        {
            mR.material = orangeColor;
        }
    }
}
