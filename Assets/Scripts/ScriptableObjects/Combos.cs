using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="New Combos", menuName = "Combos")]
public class Combos : ScriptableObject
{
    public Food.FoodType[] Foods;
    public int Bonus;
    public bool Order;
    public AudioClip Clip;
}
