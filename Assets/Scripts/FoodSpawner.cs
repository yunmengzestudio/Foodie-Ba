using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public float SpawnInterval = 3f;
    [Range(0, 0.1f)]
    public float AccelerateRatio = 0.05f;
    [Range(0, 0.1f)]
    public float FallAccelerateRatio = 0.03f;

    public GameObject[] FoodPrefabs;

    private float halfWidth;
    private float timer = 3;
    private float fallAdditionVelocity = 2f;


    private void Start() {
        Vector2 pos = new Vector2(Screen.width, Screen.height);
        pos = Camera.main.ScreenToWorldPoint(pos);
        transform.position = new Vector2(0, pos.y + 1);
        halfWidth = pos.x;
    }

    private void FixedUpdate() {
        if (timer > 0) {
            timer -= Time.fixedDeltaTime;
        }
        else {
            timer = SpawnInterval;
            SpawnInterval -= AccelerateRatio * SpawnInterval;
            AccelerateRatio *= (1 - AccelerateRatio);
            Spawn();

            // 不断增加下落速度
            fallAdditionVelocity += FallAccelerateRatio * fallAdditionVelocity;
            FallAccelerateRatio *= (1 - FallAccelerateRatio);
        }
    }

    private void Spawn() {
        Vector2 pos = new Vector2(Random.Range(-halfWidth, halfWidth), transform.position.y);
        int index = Random.Range(0, FoodPrefabs.Length);

        GameObject go = Instantiate(FoodPrefabs[index], transform);
        go.transform.position = pos;

        // 附加速度
        Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
        float addtionVelocity = Random.Range(fallAdditionVelocity - 1, fallAdditionVelocity + 1);
        rb.velocity = rb.velocity + new Vector2(0, -addtionVelocity);
    }
}
