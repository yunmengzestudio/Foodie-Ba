using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;


/// <summary>
/// 管理两个 UI: 饱腹值 Slider 和得分文字效果显示
/// </summary>
public class Stomach : MonoBehaviour
{
    public Slider Slider;
    public int MaxEnergy = 1000;
    public float HungrySpeed = 0.1f;

    [Header("Score Effect")]
    public GameObject EnergyTextPrefab;
    public float UpDistance = 5f;
    public float Duration = 0.35f;
    public Ease UpEase = Ease.OutElastic;
    public Ease FadeEase = Ease.OutElastic;

    public int MinFontSize = 50;
    public int MaxFontSize = 300;
    public int MinScoreBound = 40;  // 若小于等于 MinScoreBound 则取最小字号
    public int MaxScoreBound = 200;  // 若大于等于 MaxScoreBound 则取最大字号


    private float energy;
    private bool hasStarved = false;


    private void Start() {
        energy = MaxEnergy / 2;
        canvasRect = GameObject.Find("/Canvas").GetComponent<RectTransform>();
    }

    private void Update() {
        energy -= Time.deltaTime * HungrySpeed;
        HungrySpeed += Time.deltaTime * 1f;
    }

    private void FixedUpdate() {
        Slider.value = energy / MaxEnergy;
        if (energy <= 0 && !hasStarved) {
            hasStarved = true;
            GameManager.Instance.GameOver();
        }
    }

    public void Eat(int energy) {
        GameManager.Instance.Player.AddScore(energy);
        EnergyTextEffect(energy);
        this.energy = Mathf.Max(0, Mathf.Min(this.energy + energy, MaxEnergy));
    }


    private void EnergyTextEffect(int energy) {
        GameObject go = Instantiate(EnergyTextPrefab, canvasRect);

        Text text = go.GetComponent<Text>();
        text.fontSize = GetFontSize(energy);
        Debug.Log(text.fontSize);
        text.text = (energy > 0 ? "+" : "") + energy.ToString();
        text.DOFade(0, Duration).SetEase(FadeEase);

        RectTransform rect = go.GetComponent<RectTransform>();
        rect.anchoredPosition = World2UI(transform.position);
        rect.DOMoveY(rect.position.y + UpDistance, Duration)
            .SetEase(UpEase)
            .OnComplete(() => {
                Destroy(go);
            });
    }

    private RectTransform canvasRect;
    public Vector2 World2UI(Vector3 wpos) {
        Vector2 retPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect, 
            Camera.main.WorldToScreenPoint(wpos), 
            null, 
            out retPos);
        return retPos;
    }

    private int GetFontSize(int energy) {
        energy = Mathf.Abs(energy);
        if (energy <= MinScoreBound) {
            return MinFontSize;
        }
        else if (energy >= MaxScoreBound) {
            return MaxFontSize;
        }
        else {
            float ratio = (float)(energy - MinScoreBound) / (MaxScoreBound - MinScoreBound); 
            return (int)(MinFontSize + ratio * (MaxFontSize - MinFontSize));
        }
    }
}
