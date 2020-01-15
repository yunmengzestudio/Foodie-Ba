using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int Score { private set; get; } = 0;

    private void Start() {
        GameManager.Instance.Player = this;
    }

    public void AddScore(int score) {
        if (score > 0)
            Score += score;
    }
}
