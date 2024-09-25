using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour
{
    public int TeamNum;
    [SerializeField] MultiSpinGame multiSpinGame;
    // Start is called before the first frame update
    void Start()
    {
        TeamNum = multiSpinGame.playerNum;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
