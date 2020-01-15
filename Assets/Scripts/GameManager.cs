﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { private set; get; }

    public GameObject GameOverPanel;
    public Player Player;
    public SceneMove SceneMove;

    [SerializeField]
    private bool isGameOver = false;


    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else {
            Destroy(this);
        }
    }
    
    public void GameOver() {
        if (isGameOver)
            return;
        isGameOver = true;
        RectTransform rect = GameObject.Find("Canvas").GetComponent<RectTransform>();
        GameResultDisplay result = Instantiate(GameOverPanel, rect).GetComponent<GameResultDisplay>();
        result.Init(Player.Score);
    }

    public void LoadScene(string name) {
        SceneMove.Load(name);
        StartCoroutine(ResetConf());
    }

    private IEnumerator ResetConf(float delay = .3f) {
        yield return new WaitForSeconds(delay);
        isGameOver = false;
    }
}
