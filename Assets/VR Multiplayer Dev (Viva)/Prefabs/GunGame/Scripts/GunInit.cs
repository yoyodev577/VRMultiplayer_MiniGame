using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunInit : MonoBehaviour
{
    //Vector3 _initpos = new Vector3(-3.1099999f, 0.707000017f, 4.54400015f);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init()
    {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    }

    public void Reset()
    {
        //transform.position = _initpos;
        //transform.eulerAngles = Vector3.zero;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }
}
