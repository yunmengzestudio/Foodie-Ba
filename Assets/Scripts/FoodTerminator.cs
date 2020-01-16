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
    private PlayerTouchMove PlayerMove;

    public AudioSource FoodAudio;
    public AudioSource EatVomitAudio;
    public AudioClip EatClip;
    public AudioClip VomitClip;

    private Queue<Food.FoodType> Foods = new Queue<Food.FoodType>();
    private Animator animator;
    private bool isVomiting;        // 呕吐ing


    private void Start() {
        animator = GetComponent<Animator>();
        Stomach = GetComponent<Stomach>();
        CombosManager = GetComponent<CombosManager>();
        PlayerMove = GetComponent<PlayerTouchMove>();
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

        // Audio 报食物名
        FoodAudio.clip = food.Clip;
        FoodAudio.Play();

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
            if (EatVomitAudio.clip != EatClip || !EatVomitAudio.isPlaying) {
                EatVomitAudio.clip = EatClip;
                EatVomitAudio.Play();
            }
        }
    }

    private IEnumerator Vomit(float delay=0.3f) {
        isVomiting = true;
        PlayerMove.enabled = false;
        yield return new WaitForSeconds(delay);

        // 动画机 -> start vomit

        // 声音反馈 -> 播放音效
        EatVomitAudio.clip = VomitClip;
        EatVomitAudio.Play();
        yield return new WaitForSeconds(VomitTime);
        // 动画机 -> stop vomit

        PlayerMove.enabled = true;
        isVomiting = false;
    }
    
}
