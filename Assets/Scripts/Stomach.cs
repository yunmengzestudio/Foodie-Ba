using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

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
    

    private float energy;
    private bool hasStarved = false;


    private void Start() {
        energy = MaxEnergy / 2;
        canvasRect = GameObject.Find("Canvas").GetComponent<RectTransform>();
    }

    private void Update() {
        energy -= Time.deltaTime * HungrySpeed;
        HungrySpeed += Time.deltaTime * 0.1f;
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
}
