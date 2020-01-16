using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CombosManager : MonoBehaviour
{
    public Stomach Stomach;
    public AudioSource CombosAudio;
    public Combos[] CombosList;

    private Dictionary<string, Combos> map = new Dictionary<string, Combos>();


    private void Start() {
        InitMap();
    }

    private void InitMap() {
        foreach (Combos combos in CombosList) {
            map.Add(CreateCombosKey(combos.Foods, combos.Order), combos);
        }
    }

    public bool CheckCombo(Queue<Food.FoodType> foods) {
        List<Food.FoodType> foodList = new List<Food.FoodType>(foods.ToArray());
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
        Stomach.Eat(combos.Bonus);
        CombosAudio.clip = combos.Clip;
        CombosAudio.Play();
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
