using UnityEngine;

public class CoinSpawnController : MonoBehaviour
{
    public GameObject coinPrefab;
    public Transform coinSpawnPoint;
    public float coinSpawnDelay = 1f;

    void Start()
    {
        // Starte das Coin-Spawnen
        InvokeRepeating("SpawnCoin", coinSpawnDelay, coinSpawnDelay);
    }

    void SpawnCoin()
    {
        // Erstelle eine Münze an der Münz-Spawn-Position
        Instantiate(coinPrefab, coinSpawnPoint.position, coinPrefab.transform.rotation);
    }
}