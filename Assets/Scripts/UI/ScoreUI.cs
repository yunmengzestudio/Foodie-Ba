using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreUI : MonoBehaviour
{
    public Text ScoreText;
    public float SmoothTime = .5f;
    private float speed = 100;

    private int Score = 0;
    private float score = 0;


    private void Update() {
        if ((int)score != Score) {
            score += speed * Time.deltaTime;
            if (score >= Score) {
                score = Score;
            }
            ScoreText.text = ((int)score).ToString();
        }
    }

    public void SetScore(int score) {
        Score = score;
        speed = (Score - this.score) / SmoothTime;
    }
}
