using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FoodTerminator : MonoBehaviour
{
    public int MaxFoodCount = 5;    // 组合技最大数量
    public float VomitTime = 2f;    // 呕吐时间

    private Stomach Stomach;
    private CombosManager CombosManager;

    public AudioSource FoodAudio;
    public AudioClip EatClip;
    public AudioClip VomitClip;

    private Queue<Food.FoodType> Foods = new Queue<Food.FoodType>();
    private Animator animator;
    private bool isVomiting;        // 呕吐ing


    private void Start() {
        animator = GetComponent<Animator>();
        Stomach = GetComponent<Stomach>();
        CombosManager = GetComponent<CombosManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        // 正在呕吐或者
        if (isVomiting || collision.tag != "Food")
            return;
        Food food = collision.GetComponent<FoodDisplay>().Food;
        if (food == null)
            return;
        
        // 人物动画 + UI 反馈 + 声音反馈
        EatFood(food);

        // 呕吐 <- 不可吃食物
        if (food.Type == Food.FoodType.Baba) {
            StartCoroutine(Vomit());
        }

        Destroy(collision.gameObject);
    }

    private void EatFood(Food food) {
        // 队列操作
        if (Foods.Count == MaxFoodCount) {
            Foods.Dequeue();
        }
        Foods.Enqueue(food.Type);

        /// 检查是否包含组合技
        if (CombosManager.CheckCombo(Foods)) {
            // 队列清空
            Foods.Clear();

            // 人物动画

        }
        else {
            // 人物动画

            // UI 反馈 -> 加饱腹值
            Stomach.Eat(food.Energy);

            // 声音反馈 -> 播放音效
            if (FoodAudio.clip != EatClip || !FoodAudio.isPlaying) {
                FoodAudio.clip = EatClip;
                FoodAudio.Play();
            }
        }
    }

    private IEnumerator Vomit(float delay=0.3f) {
        isVomiting = true;
        yield return new WaitForSeconds(delay);

        // 动画机 -> start vomit

        // 声音反馈 -> 播放音效
        FoodAudio.clip = VomitClip;
        FoodAudio.Play();
        yield return new WaitForSeconds(VomitTime);
        // 动画机 -> stop vomit

        isVomiting = false;
    }
    
}
