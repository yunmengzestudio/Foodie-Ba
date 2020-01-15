using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTouchMove : MonoBehaviour
{
    private void FixedUpdate() {
        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);
            Vector3 pos = Camera.main.ScreenToWorldPoint(touch.position);

            if (pos.y > 0)
                return;

            pos.y = transform.position.y;
            pos.z = transform.position.z;
            transform.position = pos;
        }
    }
}
