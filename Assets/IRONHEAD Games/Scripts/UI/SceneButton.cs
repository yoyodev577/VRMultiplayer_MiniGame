using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneButton : MonoBehaviour
{   
    public int scene_number;
    public int click; // no one click it yet
    // Start is called before the first frame update
    void Start()
    {
        if (this.gameObject.name == "MiniGame1")
        {
            scene_number = 1;
        }
        else if (this.gameObject.name == "MiniGame2")
        {
            scene_number = 2;
        }
        else if (this.gameObject.name == "MiniGame3")
        {
            scene_number = 3;
        }
        else if (this.gameObject.name == "MiniGame4")
        {
            scene_number = 4;
        }
        else if (this.gameObject.name == "MiniGame5")
        {
            scene_number = 5;
        }
        else if (this.gameObject.name == "MiniGame6")
        {
            scene_number = 6;
        }
        click = 0;
    }
}
