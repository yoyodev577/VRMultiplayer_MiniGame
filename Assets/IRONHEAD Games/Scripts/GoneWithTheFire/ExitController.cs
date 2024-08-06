using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoneWithTheFire
{
    public class ExitController : MonoBehaviour
    {
        public PhotonView view;
        public AudioSource audioSource;
        public AudioClip winClip;
        public GameObject panel;
        // Start is called before the first frame update

        private void Start()
        {
            view = GetComponent<PhotonView>();
            audioSource = GetComponent<AudioSource>();
            panel.SetActive(false);
        }

        public void EnablePanel() {

            view.RPC("PhotonEnablePanel", RpcTarget.All); 

        }

        [PunRPC]
        public void PhotonEnablePanel() {

            panel.SetActive(true);
        }

        public void EnableSFX() {
            audioSource.PlayOneShot(winClip);
        }
    }
}
