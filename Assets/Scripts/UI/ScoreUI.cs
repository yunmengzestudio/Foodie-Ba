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
            if (score < Score)
                score += speed * Time.deltaTime;
            else
                score -= speed * Time.deltaTime;

            if (Mathf.Abs(score - Score) < speed * Time.deltaTime)
                score = Score;

            ScoreText.text = ((int)score).ToString();
        }
    }

    public void SetScore(int score) {
        Score = score;
        speed = Mathf.Abs(Score - this.score) / SmoothTime;
    }
}
