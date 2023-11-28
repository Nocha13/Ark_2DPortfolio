using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResult : MonoBehaviour
{
    public GameObject[] Titles;
    
    public void Win()
    {
        Titles[0].SetActive(true);
    }

    public void Lose()
    {
        Titles[1].SetActive(true);
    }
}
