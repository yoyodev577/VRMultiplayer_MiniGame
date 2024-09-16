using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicOwnership : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision c)
    {
        Debug.Log("coltest"+c.transform.name);
        if (c.transform.CompareTag("handCol"))
        {
            Debug.Log("suc");
            GetComponent<NetworkChangeOwnership>().SetOwnership();
        }
    }
}
