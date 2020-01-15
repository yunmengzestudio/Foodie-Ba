using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public float SpawnInterval = 3f;
    public float AccelerateRatio = 0.05f;
    public float MaxGravity = 4f;

    public GameObject FoodPrefab;
    public Food[] Foods;

    private float halfWidth;
    private float timer = 3;


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
        }
    }

    private void Spawn() {
        Vector2 pos = new Vector2(Random.Range(-halfWidth, halfWidth), transform.position.y);
        int index = Random.Range(0, Foods.Length);

        GameObject go = Instantiate(FoodPrefab, transform);
        go.transform.position = pos;
        go.GetComponent<FoodDisplay>().Food = Foods[index];

        go.GetComponent<Rigidbody2D>().gravityScale = Random.Range(1, MaxGravity);
    }
}
