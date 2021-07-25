using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FinalScore : MonoBehaviour
{
    private int _score;
    // Start is called before the first frame update
    void Start()
    {
        var scoreboard = GameObject.FindGameObjectWithTag("Scoreboard");
        _score = scoreboard.GetComponent<ScoreTrack>().Score;
        gameObject.GetComponent<TextMeshProUGUI>().text = "Final Score: " + _score;
        Destroy(scoreboard);
    }
}
