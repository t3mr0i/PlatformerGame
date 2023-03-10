using UnityEngine;
using System.Collections.Generic;

public class ObstacleSpawnController : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;
    public Transform obstacleSpawnPoint;
    public float obstacleSpawnDelay = 2f;
    public float spawnRange = 5f;
    public int maxObstacles = 5;

    private List<GameObject> obstacles = new List<GameObject>();

    void Start()
    {
        // Starte das Obstacle-Spawnen
        InvokeRepeating("SpawnObstacle", obstacleSpawnDelay, obstacleSpawnDelay);
    }

    void SpawnObstacle()
    {
        // Überprüfe, ob die maximale Anzahl an Hindernissen erreicht wurde
        if (obstacles.Count >= maxObstacles)
        {
            return;
        }

        // Wähle ein zufälliges Hindernis und eine zufällige Spawn-Position
        GameObject obstaclePrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
        Vector3 spawnPosition = new Vector3(obstacleSpawnPoint.position.x + Random.Range(-spawnRange, spawnRange), obstacleSpawnPoint.position.y, obstacleSpawnPoint.position.z);

        // Erstelle das Hindernis und füge es der Liste hinzu
        GameObject obstacle = Instantiate(obstaclePrefab, spawnPosition, obstaclePrefab.transform.rotation);
        obstacles.Add(obstacle);

        // Entferne Hindernisse, die unterhalb der Plattform liegen
        if (obstacle.transform.position.y < -5f)
        {
            obstacles.Remove(obstacle);
            Destroy(obstacle);
        }
    }

    public void RemoveObstacle(GameObject obstacle)
    {
        // Entferne das Hindernis aus der Liste
        obstacles.Remove(obstacle);
    }

    public void ClearObstacles()
    {
        // Zerstöre alle Hindernisse und leere die Liste
        foreach (GameObject obstacle in obstacles)
        {
            Destroy(obstacle);
        }
        obstacles.Clear();
    }
}