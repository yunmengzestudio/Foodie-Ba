using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class CombosManager : MonoBehaviour
{
    public Stomach Stomach;
    public BulletCommenter BulletCommenter;
    public AudioSource CombosAudio;
    public Combos[] CombosList;

    private Dictionary<string, Combos> map = new Dictionary<string, Combos>();
    private List<AudioSource> otherAudios;


    private void Start() {
        InitMap();
        otherAudios = new List<AudioSource>(GetComponentsInChildren<AudioSource>());
        otherAudios.Remove(CombosAudio);
    }

    private void InitMap() {
        foreach (Combos combos in CombosList) {
            map.Add(CreateCombosKey(combos.Foods, combos.Order), combos);
        }
    }

    public bool CheckCombo(Queue<Food> foods) {
        List<Food.FoodType> foodList = new List<Food.FoodType>();
        foreach(Food food in foods) {
            foodList.Add(food.Type);
        }
        return CheckCombo(foodList);
    }

    public bool CheckCombo(List<Food.FoodType> foods) {
        int len = foods.Count - 1;

        for (int i = 0; i < len; i++) {
            string unorderedKey = CreateCombosKey(foods.ToArray(), false);
            string orderedKey = CreateCombosKey(foods.ToArray(), true);

            if (map.ContainsKey(orderedKey) && map[orderedKey].Order == true) {
                CombosEffect(map[orderedKey]);
                return true;
            }
            else if (map.ContainsKey(unorderedKey) && map[unorderedKey].Order == false) {
                CombosEffect(map[unorderedKey]);
                return true;
            }
            else {
                foods.RemoveAt(0);
            }
        }
        return false;
    }

    private void CombosEffect(Combos combos) {
        // 加饱腹值 得分
        Stomach.Eat(combos.Bonus);
        // 发送弹幕
        BulletCommenter.Shoot(combos.BulletComment);
        // 播放音频
        CombosAudio.clip = combos.Clip;
        CombosAudio.Play();
        StartCoroutine(TurnDownOtherAudios());
    }

    private IEnumerator TurnDownOtherAudios() {
        foreach (AudioSource audio in otherAudios) {
            audio.volume /= 4;
        }
        yield return new WaitForSeconds(.1f);
        yield return new WaitForSeconds(CombosAudio.clip.length - 0.1f);
        foreach (AudioSource audio in otherAudios) {
            audio.DOFade(audio.volume * 4, 0.1f);
        }
    }

    private string CreateCombosKey(Food.FoodType[] foods, bool order = false) {
        if (!order) {
            Array.Sort(foods);
        }

        string key = "";
        foreach (Food.FoodType type in foods) {
            key += type.ToString();
        }
        return key;
    }
}
