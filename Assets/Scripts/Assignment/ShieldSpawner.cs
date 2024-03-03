using UnityEngine;

public class ShieldSpawner : MonoBehaviour
{
    [SerializeField] private GameObject shieldPrefab; // Assign this in the Inspector
    [SerializeField] private float spawnRadius = 5f; // The radius within which the shield can spawn
    [SerializeField] private Vector2 spawnTimeRange = new Vector2(5f, 10f); // Min and max time in seconds between spawns

    private float timeToNextSpawn;
    private float hight = 0.33f;

    private void Start()
    {
        // Initialize the spawn timer the first time the object is spawned
        ResetSpawnTimer();
    }

    private void Update()
    {
        timeToNextSpawn -= Time.deltaTime;

        if (timeToNextSpawn <= 0)
        {
            SpawnShield();
            ResetSpawnTimer();
        }
    }

    private void ResetSpawnTimer()
    {
        // Reset the timer to a new value within the specified range
        timeToNextSpawn = Random.Range(spawnTimeRange.x, spawnTimeRange.y);
    }

    private void SpawnShield()
    {
        // Calculate a random position within the specified radius on the plane
        Vector3 spawnPosition = Random.insideUnitSphere * spawnRadius;
        spawnPosition.y = hight; // Adjust for the correct height if your game plane is not at y=0

        // Spawn the shield at the calculated position
        Instantiate(shieldPrefab, spawnPosition, Quaternion.identity);
    }
}
