using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameResultDisplay : MonoBehaviour
{
    public Text Title;
    public Text Score;
    public Button ReplayBtn;
    public Button HomeBtn;

    public static int[] Scores = new int[] { 1000, 2000, 5000 };
    public static string[] Titles = new string[] { "小吃播", "当红吃播", "全国美食家" };


    public void Init(int score) {
        int index = 0;
        if (score >= Scores[0]) {
            while (index < Scores.Length) {
                if (score < Scores[index++]) {
                    index--;
                    break;
                }
            }
        }
        index = Mathf.Min(Scores.Length - 1, index);

        Score.text = score.ToString();
        Title.text = Titles[index];

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
