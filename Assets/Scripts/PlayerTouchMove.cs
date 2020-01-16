using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTouchMove : MonoBehaviour
{
    public float Smooth = 0.5f;

    private void FixedUpdate() {
        if (Input.GetMouseButton(0)) {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (pos.y > 0)
                return;

            pos.y = transform.position.y;
            pos.z = transform.position.z;
            transform.position = Vector3.Lerp(transform.position, pos, Smooth);
        }
    }
}
