﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int Score { private set; get; } = 0;
    public ScoreUI ScoreUI;

    private void Start() {
        GameManager.Instance.Player = this;
    }

    public void AddScore(int score) {
        Score += score;
        ScoreUI.SetScore(Score);
    }

    public void PlayerPause() {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
