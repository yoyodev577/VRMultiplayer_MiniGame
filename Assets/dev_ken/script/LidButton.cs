using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LidButton : MonoBehaviour
{
    [SerializeField] MultiSpinGame multiSpinGame;
    public List<GameObject> colliderList = new List<GameObject>();
    public void OnTriggerEnter(Collider collider)
    {
        if (!colliderList.Contains(collider.gameObject))
        {
            colliderList.Add(collider.gameObject);
        }
        //if hand touch the button
        if (collider.transform.CompareTag("HandBox"))
        {
            //toggle the lid
            multiSpinGame.CheckToggleLid();
        }

    }

    public void OnTriggerExit(Collider collider)
    {
        if (colliderList.Contains(collider.gameObject))
        {
            colliderList.Remove(collider.gameObject);
        }
    }
}
