using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public float SpawnInterval = 3f;
    [Range(0, 0.1f)]
    public float AccelerateRatio = 0.05f;
    [Range(0, 0.1f)]
    public float FallAccelerateRatio = 0.03f;

    //public GameObject[] FoodPrefabs;

    [SerializeField]
    public FoodInfo[] FoodInfos;

    private float halfWidth;
    private float timer = 3;
    private float fallAdditionVelocity = 0.3f;


    private void Start() {
        Vector2 pos = new Vector2(Screen.width, Screen.height);
        pos = Camera.main.ScreenToWorldPoint(pos);
        transform.position = new Vector2(0, pos.y + 1);
        halfWidth = pos.x;

        // OnGameOver 事件注册 -> 关闭 FoodSpawner
        GameManager.Instance.OnGameOver.AddListener(() => gameObject.SetActive(false));
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

        GameObject go =Instantiate(GetRandomFood());
        go.transform.position = pos;

        // 附加速度
        Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
        float addtionVelocity = Random.Range(fallAdditionVelocity - 1, fallAdditionVelocity + 1);
        rb.velocity = rb.velocity + new Vector2(0, -addtionVelocity);
        rb.AddTorque(Random.Range(-90, 90));

    }

    private GameObject GetRandomFood()
    {
        GameObject targetFood = null;
        int randomValue = Random.Range(0, 100);
        foreach(var info in FoodInfos)
        {
            targetFood = info.FoodPrefab;
            if (randomValue >= info.RangeMin && randomValue <= info.RangeMax)
            {
                break;
            }
        }
        return targetFood;
    }

}

[System.Serializable]
public class FoodInfo
{
    public GameObject FoodPrefab;
    public int RangeMin;
    public int RangeMax;
}
