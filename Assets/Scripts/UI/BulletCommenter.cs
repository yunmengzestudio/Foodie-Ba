using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class BulletCommenter : MonoBehaviour
{
    public GameObject Prefab;
    public float MinSpeed = 200f;
    public float MaxSpeed = 400;
    public float LowYRatio = 0.3f;
    public float HighYRatio = 0.7f;

    private float singleLetterLength = 80;
    private float StartX;
    private float LowY;
    private float HighY;


    private void Start() {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            GetComponent<RectTransform>(),
            new Vector2(Screen.width, Screen.height),
            null,
            out pos);

        StartX = pos.x;
        LowY = pos.y * 2 * LowYRatio - pos.y;
        HighY = pos.y - (1 - HighYRatio) * pos.y * 2;
    }

    public void Shoot(string text) {
        float y = Random.Range(LowY, HighY);

        GameObject go = Instantiate(Prefab, transform);
        RectTransform rectTransform = go.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector3(StartX, y, 0);

        Text Text = go.GetComponent<Text>();
        Text.text = text;
        Text.color = Random.ColorHSV();

        float targetX = -StartX - text.Length * singleLetterLength;
        float duration = (StartX - targetX) / Random.Range(MinSpeed, MaxSpeed);
        rectTransform.DOAnchorPosX(targetX, duration)
            .SetEase(Ease.Linear)
            .OnComplete(() => {
                Destroy(go);
            });
    }
}
