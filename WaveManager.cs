using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
    [SerializeField]
    public int waveCount = 1;         // Current wave.
    [SerializeField]
    private int baseEnemyCount = 5;    // Base number of enemies per wave.
    [SerializeField]
    private float spawnDelay = 1f;   // Delay between each enemy spawn.

    [SerializeField]
    private float waveTimer = 5;  // Time between waves.
    public float waveCooldown;       // Time until next wave.

    private Spawn spawn;
    private Game_Manager game_Manager;
    private AudioManager audioManager;

    // List to keep track of all the enemies.
    public List<GameObject> enemies = new List<GameObject>();
    public List<GameObject> player = new List<GameObject>();

    private int numOfBullets;
    
    private bool gameOverHasStarted = false;

    private float bossSpawnRate = 0.0f;
    
    void Awake()
    {
        audioManager = GameObject.FindObjectOfType<AudioManager>();
        spawn = GameObject.FindObjectOfType<Spawn>();
        game_Manager = GameObject.FindObjectOfType<Game_Manager>();
    }

    private void Start()
    {
        
        waveCooldown = waveTimer;     // Initialize wave cooldown to the wave timer.

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject foundPlayer in players)
        {
            player.Add(foundPlayer);
        }
    }


    private void LateUpdate()
    {
        // Clean up the list by removing null references.
        enemies.RemoveAll(item => item == null);
        player.RemoveAll(item => item == null);
        
        if (enemies.Count == 0){
            WaveBreak();
            if (Game_Manager.instance != null && Game_Manager.instance.onPlayerSpawn != null)
            {
                Game_Manager.instance.onWaveCooldown.Invoke();
            }
        }

        if (player.Count == 0 && !gameOverHasStarted){
            gameOverHasStarted = true;
            StartCoroutine(HandleGameOver());
        }
    }

    private void WaveBreak()
    {
        waveCooldown -= Time.deltaTime;

        if(waveCooldown <= 0)
        {
            StartCoroutine(SpawnWave());
            waveCooldown = waveTimer;
            waveCount++;
        }
    }

    private IEnumerator SpawnWave()
    {
        audioManager.Play("Wave");
        int enemyCount = baseEnemyCount * waveCount;   // Compute enemy count for this wave.

        for (int i = 0; i < enemyCount; i++)
        {
            // Spawn the enemy.
            GameObject newEnemy = spawn.spawnEnemy();

            // Add the new enemy to the list.
            enemies.Add(newEnemy);

            // If wave is 3 or more, spawn a new type of enemy as well.
            if(waveCount >= 4)
            {
                GameObject newEnemyFast = spawn.spawnEnemyFast();
                enemies.Add(newEnemyFast);
            }
            // If wave is 6 or more, possibly spawn a boss enemy.
            if(waveCount >= 6)
            {
                // Generate a random float between 0.0 and 1.0.
                float random = UnityEngine.Random.Range(0.0f, 1.0f);

                // If the random number is less than or equal to the boss spawn rate, spawn a boss.
                if(random <= bossSpawnRate)
                {
                    GameObject newEnemyBoss = spawn.spawnEnemyBoss();
                    enemies.Add(newEnemyBoss);
                }
            }

            yield return new WaitForSeconds(spawnDelay); // Wait for the delay before spawning the next enemy.
        }
        
        // Increase the boss spawn rate by some amount after each wave.
        bossSpawnRate += 0.01f;
    }

    private IEnumerator HandleGameOver()
    {
        audioManager.Play("Game Over");
        // Wait for the length of the "Game Over" sound clip
        yield return new WaitForSeconds(0.3f);

        game_Manager.GameOver();
    }
}