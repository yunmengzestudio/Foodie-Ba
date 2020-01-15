using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
public class FoodDisplay : MonoBehaviour
{
    private Food food;
    public Food Food
    {
        get { return food; }
        set
        {
            food = value;
            GetComponent<SpriteRenderer>().sprite = Food.Sprite;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Ground") {
            Destroy(gameObject);
        }
    }
}
