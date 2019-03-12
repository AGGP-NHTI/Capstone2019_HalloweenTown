using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEBUG_MirrorScoreboard : MonoBehaviour
{
    public List<int> PlayerScores;

    private void Start()
    {
        PlayerScores = new List<int>();
    }

    private void Update()
    {
        PlayerScores.Clear();
        if(Candy.Scoreboard != null)
        {
            foreach(int score in Candy.Scoreboard.Values)
            {
                PlayerScores.Add(score);
            }
        }
    }
}
