using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FoodTerminator : MonoBehaviour
{
    public int MaxFoodCount = 5;    // 组合技最大数量
    public float VomitTime = 3f;    // 呕吐时间

    private Stomach Stomach;
    private CombosManager CombosManager;
    private PlayerTouchMove PlayerMove;

    public CombosPanel CombosPanel;
    public AudioSource FoodAudio;
    public AudioSource EatVomitAudio;
    public AudioClip EatClip;
    public AudioClip VomitClip;

    private Queue<Food> Foods = new Queue<Food>();
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
        animator.SetTrigger("Eat");

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
        Foods.Enqueue(food);
        CombosPanel.Show(Foods.ToArray());

        // Audio 报食物名
        FoodAudio.clip = food.Clip;
        FoodAudio.Play();

        /// 检查是否包含组合技
        if (CombosManager.CheckCombo(Foods)) {
            // 队列清空
            Foods.Clear();
            CombosPanel.Show(Foods.ToArray());

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
        // 动画机 -> start vomit
        animator.SetBool("IsVomiting", true);

        isVomiting = true;
        PlayerMove.enabled = false;
        yield return new WaitForSeconds(delay);

        // 声音反馈 -> 播放音效
        EatVomitAudio.clip = VomitClip;
        EatVomitAudio.Play();
        yield return new WaitForSeconds(VomitTime);
        
        // 动画机 -> stop vomit
        animator.SetBool("IsVomiting", false);

        PlayerMove.enabled = true;
        isVomiting = false;
    }

    private void Update()
    {
        //测试用
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    Stomach.Eat(500);
        //}
    }

}
