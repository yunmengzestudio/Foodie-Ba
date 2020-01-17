using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTouchMove : MonoBehaviour
{
    public float Smooth = 0.5f;
    private Animator animator;


    private void Start() {
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate() {
        if (Input.GetMouseButton(0)) {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (pos.y > 0)
                return;

            animator.SetFloat("Speed", pos.x - transform.position.x);
            pos.y = transform.position.y;
            pos.z = transform.position.z;
            transform.position = Vector3.Lerp(transform.position, pos, Smooth);
        }
        else {
            animator.SetFloat("Speed", 0);
        }
    }
}
