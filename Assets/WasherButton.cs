using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WasherButton : MonoBehaviour
{
    [SerializeField] EyeWasher Washer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter(Collider c)
    {
        // Debug.Log("---" + collision.gameObject.name);

        if (c.gameObject.tag == "handCol")
        {
            Washer.ToogleWasher(true);
        }
    }

    public void OnTriggerExit(Collider c)
    {
        if (c.gameObject.tag == "handCol")
        {
            Washer.ToogleWasher(false);
        }
    }
}
