using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoneWithTheFire
{
    public class ExitDetector : MonoBehaviour
    {
        public bool isInExit = false;

        private void OnTriggerEnter(Collider other)
        {
            if(other != null && other.gameObject.name=="XR Origin")
            {
                isInExit = true;
            }
            
        }
        private void OnTriggerExit(Collider other)
        {
            if (other != null && other.gameObject.name == "XR Origin")
            {
                isInExit = false;
            }
        }
    }
}
