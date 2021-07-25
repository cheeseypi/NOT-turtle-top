using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreTrack : MonoBehaviour
{
    public int Score { get => _score; }
    public string ScorePrefix = "Score";
    public TextMeshProUGUI ScoreText;

    public GameObject EnemyTop;

    private int _score = 0;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void IncreaseScore()
    {
        _score += 1;
        ScoreText.text = ScorePrefix + (ScorePrefix?.Length > 0 ? ": " : "") + Score;
        Instantiate(EnemyTop, new Vector3(0, 0, 0), Quaternion.Euler(0,0,0));
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
