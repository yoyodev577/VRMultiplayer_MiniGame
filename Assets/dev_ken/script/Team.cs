using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour
{
    public int TeamNum;
    public MultiSpinGame multiSpinGame;
    // Start is called before the first frame update
    void Start()
    {
        multiSpinGame = GetComponent<Transform>().parent.parent.parent.GetComponent<MultiSpinGame>();
        TeamNum = multiSpinGame.playerNum;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
