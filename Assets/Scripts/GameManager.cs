using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { private set; get; }

    public GameObject GameOverPanel;
    public Player Player;
    public SceneMove SceneMove;
    public UnityEvent OnGameOver;

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

        RectTransform rect = GameObject.Find("/Canvas").GetComponent<RectTransform>();
        GameResultDisplay result = Instantiate(GameOverPanel, rect).GetComponent<GameResultDisplay>();
        result.Init(Player.Score);

        // 游戏暂停
        Player.PlayerPause();
        // OnGameOver 事件触发
        OnGameOver?.Invoke();
        OnGameOver.RemoveAllListeners();
    }

    // 重置配置
    public void ResetConf(float delay = .3f) {
        StartCoroutine(_ResetConf(delay));
    }

    private IEnumerator _ResetConf(float delay) {
        yield return new WaitForSeconds(delay);
        isGameOver = false;
    }

    // 彩蛋
    public void Bingo() {
        StartCoroutine(_bingo());
    }

    private IEnumerator _bingo() {
        yield return new WaitForSeconds(4f);
        SceneMove.Load("Bingo");
        PlayerPrefs.SetInt("HasBingo", 1);
        PlayerPrefs.Save();
    }
}
