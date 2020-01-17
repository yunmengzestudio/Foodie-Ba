using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CombosPanel : MonoBehaviour
{
    public Image[] Images;
    
    public void Show(Food[] foods) {
        for(int i = 0; i < foods.Length; i++) {
            Images[i].sprite = foods[i].Sprite;
            Images[i].color = Color.white;
        }
        for (int i = foods.Length; i < Images.Length; i++) {
            Images[i].color = Color.clear;
        }
    }
}
