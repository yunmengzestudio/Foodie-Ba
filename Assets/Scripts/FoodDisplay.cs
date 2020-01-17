using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
public class FoodDisplay : MonoBehaviour
{
    public Food food;
    public Food Food
    {
        get { return food; }
        set
        {
            food = value;
            GetComponent<SpriteRenderer>().sprite = food.Sprite;
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.velocity = rb.velocity + new Vector2(0, -Food.BaseFallSpeed);
            rb.AddTorque(Random.Range(-90, 90));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Ground") {
            Destroy(gameObject);
        }
    }
}
