using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float baseSpawnRate = 1f;
    public float spawnRateIncreasePerLevel = 0.1f;
    public float maxSpawnRate = 5f;
    public float spawnRadius = 45f;
    public int baseEnemiesPerSpawn = 1;
    public int additionalEnemiesPerLevelThreshold = 1;
    public int maxEnemies = 25;  // Maximum number of enemies allowed in the scene
    public int maxEnemiesIncreasePerLevel = 2;  // How many to increase max enemies per level

    private float nextSpawnTime;
    private PlayerController playerController;
    private List<GameObject> activeEnemies = new List<GameObject>();

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        if (playerController == null)
        {
            Debug.LogError("PlayerController not found in the scene. Please ensure it exists.");
        }
    }

    void Update()
    {
        CleanUpEnemyList();

        if (Time.time >= nextSpawnTime && activeEnemies.Count < GetMaxEnemies())
        {
            SpawnEnemies();
            UpdateNextSpawnTime();
        }
    }

    void SpawnEnemies()
    {
        int enemiesToSpawn = CalculateEnemiesToSpawn();
        int availableSlots = GetMaxEnemies() - activeEnemies.Count;
        enemiesToSpawn = Mathf.Min(enemiesToSpawn, availableSlots);

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Vector2 spawnPos = (Vector2)transform.position + Random.insideUnitCircle.normalized * spawnRadius;
            GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            activeEnemies.Add(enemy);
        }
    }

    void UpdateNextSpawnTime()
    {
        float currentSpawnRate = CalculateSpawnRate();
        nextSpawnTime = Time.time + 1f / currentSpawnRate;
    }

    float CalculateSpawnRate()
    {
        if (playerController == null) return baseSpawnRate;

        float spawnRate = baseSpawnRate + (playerController.level - 1) * spawnRateIncreasePerLevel;
        return Mathf.Min(spawnRate, maxSpawnRate);
    }

    int CalculateEnemiesToSpawn()
    {
        if (playerController == null) return baseEnemiesPerSpawn;

        int additionalEnemies = (playerController.level - 1) / additionalEnemiesPerLevelThreshold;
        return baseEnemiesPerSpawn + additionalEnemies;
    }

    int GetMaxEnemies()
    {
        if (playerController == null) return maxEnemies;

        return maxEnemies + (playerController.level - 1) * maxEnemiesIncreasePerLevel;
    }

    void CleanUpEnemyList()
    {
        activeEnemies.RemoveAll(enemy => enemy == null);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}