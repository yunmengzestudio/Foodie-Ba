using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="New Food", menuName = "Food")]
public class Food : ScriptableObject
{
    public enum FoodType
    {
        Baba,
        DaSuan,
        XiaoHanBao,
        BaYa,
        NingMeng,
        HuoLongGuo,
        FuRu,
        ChouLuXia,
        ChouDouFu,
        XiangJiao,
        HuangGua,
        JiPiGu
    };

    public FoodType Type;
    public int Energy;      // 能量 | 饱腹值
    public AudioClip Clip;  // 音效
    public Sprite Sprite;

    public float BaseFallSpeed = 5f;
}
