using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class Moe : MonoBehaviour
{
    public MoeManager moeManager;
    public PhotonView view;
    public float minHeight, maxHeight;
    public Vector3 startPos;
    public bool isPop = false;
    public bool isHit = false;
    public float speed = 3f;

    public GameObject panelObj;
    public TextMeshProUGUI textMeshProUGUI;
    public string currentAns = "";
    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
        startPos = transform.localPosition;
        panelObj.SetActive(false);
        moeManager = GetComponentInParent<MoeManager>();
    }

    public void SetPop(bool _pop)
    {
        if(view != null)
        view.RPC("PhotonSetPop", RpcTarget.All,_pop);
    }
    public void SetHitStatus(bool _isHit)
    {
        if (view != null)
            view.RPC("PhotonSetHitStatus", RpcTarget.All, _isHit);
    }
    public void SetCurrentAns(string s)
    {
        if (view != null)
            view.RPC("PhotonSetCurrentAns", RpcTarget.All, s);
    }



    [PunRPC]
    public void PhotonSetPop( bool pop) {

        if (pop)
        {
            view.RPC("Pop", RpcTarget.All);
        }
        else
        {
            view.RPC("Hide", RpcTarget.All);
        }
    }
    [PunRPC]
    public void PhotonSetHitStatus(bool _isHit) {
        isHit = _isHit;
    }
    [PunRPC]
    public void PhotonSetCurrentAns(string s) {
        currentAns = s;
    }

    [PunRPC]
    public void Pop() { 
        Vector3 targetPos =  new Vector3(startPos.x, maxHeight, startPos.z);
        //Debug.Log(targetPos);
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, speed);
        panelObj.SetActive(true);
        textMeshProUGUI.text = currentAns;
    }

    [PunRPC]
    public void Hide() {
        Vector3 targetPos = new Vector3(startPos.x, minHeight, startPos.z);
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, speed);
        panelObj.SetActive(false);
    }
    [PunRPC]
    public void HideNow() {
        Vector3 targetPos = new Vector3(startPos.x, minHeight, startPos.z);
        transform.localPosition = targetPos;
        panelObj.SetActive(false);
    }

    public void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "hammer")
        {
            SetHitStatus(true);
            SetPop(false);
            moeManager.CheckScore(currentAns);
        }
    }
    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "hammer") {
            SetHitStatus(false);
        }
    }
}
