using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class GameController : MonoBehaviour
{
    public GameObject platformPrefab;
    public GameObject coinPrefab;
    public GameObject[] obstaclePrefabs;
    public GameObject[] characterPrefabs;
    public GameObject[] outfitPrefabs;
    public GameObject characterObject;
    public GameObject storeObject;
    public Text scoreText;
    public Text highScoreText;
    public Text greenCoinText;
    public Text gameOverText;
    public Button restartButton;
    public Button customizeButton;
    public Button storeButton;
    public Button buyButton;
    public GameObject greenCoinEffectPrefab;
    public GameObject[] bonusEffectPrefabs;
    public Transform[] spawnPoints;
    public Transform coinSpawnPoint;
    public Transform obstacleSpawnPoint;
    public Transform characterSpawnPoint;
    public Transform outfitSpawnPoint;
    public float coinSpawnDelay = 1f;
    public float obstacleSpawnDelay = 2f;
    public float spawnRange = 5f;
    public int maxObstacles = 5;
    public int maxLevels = 10;
    public int maxLives = 3;
    public int greenCoinValue = 10;
    public int bonusValue = 5;
    public AudioClip coinSound;
    public AudioClip bonusSound;
    public AudioClip levelUpSound;
    public AudioClip gameOverSound;

    private int score = 0;
    private int highScore = 0;
    private int greenCoins = 0;
    private int bonusCount = 0;
    private int currentLevelIndex = 0;
    private int currentLives = 3;
    private bool isGameOver = false;
    private bool isCustomizing = false;
    private LevelManager levelManager;
    private CharacterCustomizationUI customizationUI;
    private List<GameObject> obstacles = new List<GameObject>();
    private DatabaseReference databaseReference;
    private StoreController storeController;

    void Start()
    {
        // Initialisiere Firebase
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://your-project-id.firebaseio.com/");
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

        // Erstelle das Level-Manager-Skript
        levelManager = gameObject.AddComponent<LevelManager>();
        levelManager.maxLevels = maxLevels;
        levelManager.levelText = scoreText;

        // Erstelle das UI-Skript für die Charakteranpassung
        customizationUI = gameObject.AddComponent<CharacterCustomizationUI>();
        customizationUI.characterObject = characterObject;
        customizationUI.outfits = outfitPrefabs;

        // Erstelle den Store-Controller
        storeController = new StoreController(storeObject, buyButton, greenCoinText, greenCoins, outfitPrefabs, customizationUI);

        // Setze den Highscore-Text
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = "High Score: " + highScore;

        // Starte das Spiel
        StartGame();
    }

    void StartGame()
    {
        // Initialisiere die Variablen
        score = 0;
        greenCoins = 0;
        bonusCount = 0;
        currentLevelIndex = 0;
        currentLives = maxLives;
        isGameOver = false;
        isCustomizing = false;

        // Aktualisiere die UI-Elemente
        UpdateScoreText();
        UpdateGreenCoinText();

        // Erstelle die Plattformen
        CreatePlatforms();

        // Starte
// Starte das Coin-Spawnen
    InvokeRepeating("SpawnCoin", coinSpawnDelay, coinSpawnDelay);

    // Starte das Obstacle-Spawnen
    InvokeRepeating("SpawnObstacle", obstacleSpawnDelay, obstacleSpawnDelay);
}

void Update()
{
    // Überprüfe, ob das Spiel vorbei ist
    if (!isGameOver)
    {
        // Überprüfe, ob der Charakter auf dem Boden steht
        if (characterObject.transform.position.y < -5f)
        {
            // Verringere die Anzahl der Leben und aktualisiere die UI
            currentLives--;
            UpdateGreenCoinText();

            // Überprüfe, ob keine Leben mehr übrig sind
            if (currentLives <= 0)
            {
                // Beende das Spiel
                EndGame();
            }
            else
            {
                // Setze den Charakter zurück
                characterObject.transform.position = characterSpawnPoint.position;
                characterObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
    }
}

void EndGame()
{
    // Beende das Spiel
    isGameOver = true;

    // Aktualisiere den Highscore
    if (score > highScore)
    {
        highScore = score;
        PlayerPrefs.SetInt("HighScore", highScore);
    }

    // Zeige die Gameover-Texte und Buttons an
    gameOverText.gameObject.SetActive(true);
    restartButton.gameObject.SetActive(true);
    customizeButton.gameObject.SetActive(true);
    storeButton.gameObject.SetActive(true);

    // Speichere die Punktzahl in Firebase
    string userId = PlayerPrefs.GetString("UserId");
    if (!string.IsNullOrEmpty(userId))
    {
        databaseReference.Child("users").Child(userId).Child("score").SetValueAsync(score);
        databaseReference.Child("users").Child(userId).Child("greenCoins").SetValueAsync(greenCoins);
    }
}

void CreatePlatforms()
{
    // Erstelle die Plattformen
    for (int i = 0; i < spawnPoints.Length; i++)
    {
        Vector3 spawnPosition = spawnPoints[i].position;
        spawnPosition.x += Random.Range(-spawnRange, spawnRange);
        spawnPosition.z += Random.Range(-spawnRange, spawnRange);
        Instantiate(platformPrefab, spawnPosition, platformPrefab.transform.rotation);
    }
}

void SpawnCoin()
{
    // Erstelle eine Münze an der Münz-Spawn-Position
    Instantiate(coinPrefab, coinSpawnPoint.position, coinPrefab.transform.rotation);
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
    Vector3 spawnPosition = obstacleSpawnPoint.position;
    spawnPosition.x += Random.Range(-spawnRange, spawnRange);
    spawnPosition.z += Random.Range(-spawnRange, spawnRange);

    // Erstelle das Hindernis an der Spawn-Position
    GameObject obstacleObject = Instantiate(obstaclePrefab, spawnPosition, obstaclePrefab.transform.rotation);

    // Füge das Hindernis der Liste hinzu
    obstacles.Add(obstacleObject);

    // Lösche das Hindernis nach einer gewissen Zeit wieder aus der Liste
    Destroy(obstacleObject, 10f);
}

public void AddScore(int points)
{
    // Füge die Punkte zum Score
    
    score += points;
    UpdateScoreText();

    // Überprüfe, ob ein Level aufgestiegen wurde
    if (levelManager.TryAdvanceLevel(score))
    {
        // Spiele den Level-Up-Sound ab und zeige den Effekt an
        AudioSource.PlayClipAtPoint(levelUpSound, Camera.main.transform.position);
        levelManager.ShowLevelUpEffect();
    }

    // Überprüfe, ob ein Bonus gewährt wird
    if (Random.Range(0f, 1f) <= 0.1f)
    {
        // Füge den Bonus-Punkte hinzu und aktualisiere die UI
        bonusCount += bonusValue;
        UpdateGreenCoinText();

        // Spiele den Bonus-Sound ab und zeige den Effekt an
        AudioSource.PlayClipAtPoint(bonusSound, Camera.main.transform.position);
        Instantiate(bonusEffectPrefabs[Random.Range(0, bonusEffectPrefabs.Length)], characterObject.transform.position, Quaternion.identity);
    }
}

public void CollectCoin()
{
    // Füge die Münz-Punkte hinzu und aktualisiere die UI
    greenCoins += greenCoinValue;
    UpdateGreenCoinText();

    // Spiele den Münz-Sound ab und zeige den Effekt an
    AudioSource.PlayClipAtPoint(coinSound, Camera.main.transform.position);
    GameObject effectObject = Instantiate(greenCoinEffectPrefab, characterObject.transform.position, Quaternion.identity);
    Destroy(effectObject, 2f);
}

void UpdateScoreText()
{
    // Aktualisiere den Score-Text
    scoreText.text = "Score: " + score;
}

void UpdateGreenCoinText()
{
    // Aktualisiere den Green-Coin-Text
    greenCoinText.text = "Green Coins: " + greenCoins + "\nBonus: " + bonusCount;
}
}