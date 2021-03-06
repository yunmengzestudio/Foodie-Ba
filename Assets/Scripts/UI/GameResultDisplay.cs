﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameResultDisplay : MonoBehaviour
{
    public Text Title;
    public Text Score;
    public Button ReplayBtn;
    public Button HomeBtn;
    public AudioSource Audio;

    public static int[] Scores = new int[] { 1000, 3000, 6000 };
    public static string[] Titles = new string[] { "小吃播", "当红吃播", "全国美食家" };
    public AudioClip[] Clips;


    public void Init(int score) {
        int index = 0;

        for(int i=0; i < Scores.Length; i++)
        {
            if (score > Scores[i])
            {
                index = i;
            }
            else
            {
                break;
            }
        }

        Score.text = score.ToString();
        Title.text = Titles[index];
        Audio.clip = Clips[index];
        Audio.Play();

        ReplayBtn.onClick.AddListener(() => {
            GameManager.Instance.SceneMove.Load("Game");
        });
        HomeBtn.onClick.AddListener(() => {
            GameManager.Instance.SceneMove.Load("Home");
        });

        // 拿到最高等级进入彩蛋
        if (index == Scores.Length - 1 && !PlayerPrefs.HasKey("HasBingo")) {
            GameManager.Instance.Bingo();
            ReplayBtn.enabled = false;
            HomeBtn.enabled = false;
        }
    }
}
