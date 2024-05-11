using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStats : MonoBehaviour
{
    public int solvedMinigames;
    public bool gameover;
    public string name;

    // Start is called before the first frame update
    void Start()
    {
        solvedMinigames = 0;
        name = "";
        gameover = false;
    }

}
