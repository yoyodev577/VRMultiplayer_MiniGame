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
    public bool isHitCoroutine = false;
    public bool isPopCoroutine = false;
    public bool isHideCoroutine = false;
    public float speed = 3f;

    public GameObject panelObj;
    public TextMeshProUGUI textMeshProUGUI;
    public string currentAns = "";

    public MeshRenderer[] mRs;
    public AudioSource sfxSource;
    public AudioClip hitClip;
    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
        startPos = transform.localPosition;
        panelObj.SetActive(false);
        moeManager = GetComponentInParent<MoeManager>();
        mRs = GetComponentsInChildren<MeshRenderer>();
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

        isPop = pop;
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

        if (isHit)
        {
            if (!isHitCoroutine)
                StartCoroutine(SetHitCoroutine());
        }
    }
    [PunRPC]
    public void PhotonSetCurrentAns(string s) {
        currentAns = s;
    }

    [PunRPC]
    public void Pop()
    {
        panelObj.SetActive(true);
        textMeshProUGUI.text = currentAns;
        if (!isPopCoroutine)
        {
            StartCoroutine(PopCoroutine());
        }
    }

    [PunRPC]
    public void Hide()
    {
        panelObj.SetActive(false);
        textMeshProUGUI.text = "?";
        if (!isHideCoroutine )
        {
            StartCoroutine(HideCoroutine());
        }
    }
    [PunRPC]
    public void HideNow() {
        Debug.Log("--Hide all moes now---");
        StopCoroutine(PopCoroutine());
        StopCoroutine(HideCoroutine());
        isHideCoroutine = false;
        isPopCoroutine = false;

        Vector3 targetPos = new Vector3(startPos.x, minHeight, startPos.z);
        transform.localPosition = targetPos;
        panelObj.SetActive(false);

    }

    [PunRPC]
    public void SwitchColor(bool isRed) {
        if (isRed)
        {
            foreach (MeshRenderer mR in mRs)
            {
                mR.material.color = Color.red;
            }
        }
        else
        {
            foreach (MeshRenderer mR in mRs)
            {
                mR.material.color = Color.white;
            }
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "hammer" && isPop)
        {
            SetHitStatus(true);
            moeManager.CheckScore(currentAns);
        }
    }
    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "hammer") {
            view.RPC("SwitchColor", RpcTarget.All, false);;
        }
    }

    IEnumerator SetHitCoroutine() {
        isHitCoroutine = true;
        while (isHit) {
            view.RPC("SwitchColor", RpcTarget.All, true);
            sfxSource.PlayOneShot(hitClip);
            yield return new WaitForSeconds(0.5f);
            SetPop(false);
            isHit = false;
        }
        isHitCoroutine = false;  
    }

    IEnumerator PopCoroutine()
    {
        isPopCoroutine = true;
        Vector3 targetPos = new Vector3(startPos.x, maxHeight, startPos.z);

        while (Vector3.Distance(targetPos, transform.localPosition) > 0.1f)
        {
            Debug.Log("---Moe is popping :" + gameObject.name);
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, speed * Time.deltaTime);
            yield return null;
        }
        isPopCoroutine = false;

    }

    IEnumerator HideCoroutine()
    {
        isHideCoroutine = true;
        Vector3 targetPos = new Vector3(startPos.x, minHeight, startPos.z);

        while (Vector3.Distance(targetPos, transform.localPosition) > 0.1f)
        {
            Debug.Log("---Moe is hiding :" + gameObject.name );
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, speed * Time.deltaTime);
            yield return null;
        }
        isHideCoroutine = false;

    }
}
