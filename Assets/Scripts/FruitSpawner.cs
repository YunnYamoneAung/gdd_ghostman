using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    public GameObject fruitPrefab;
    public Transform spawnPoint;
    public float fruitLifetime = 10f;

    public void SpawnFruit()
    {
        if (fruitPrefab != null && spawnPoint != null)
        {
            GameObject fruit = Instantiate(fruitPrefab, spawnPoint.position, Quaternion.identity);

            // Auto-destroy after lifetime
            Destroy(fruit, fruitLifetime);
        }
    }
}
