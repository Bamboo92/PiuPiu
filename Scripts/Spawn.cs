using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField]
    public bool coop;

    [SerializeField]
    private GameObject playerPrefab1;
    [SerializeField]
    private GameObject playerPrefab2;
    [SerializeField]
    private GameObject enemy;
    [SerializeField]
    private GameObject enemyFast;
    [SerializeField]
    private GameObject enemyBoss;
    [SerializeField]
    private GameObject healPrefab;

    [SerializeField]
    private GameObject player1pos1, player1pos2, player1pos3, player1pos4;
    [SerializeField]
    private GameObject player2pos1, player2pos2, player2pos3, player2pos4;
    [SerializeField]
    private GameObject enemyPos1, enemyPos2;
    [SerializeField]
    private GameObject bossEnemyPos1, bossEnemyPos2;
    [SerializeField]
    private GameObject healPos1, healPos2, healPos3, healPos4;

    private Vector3 randomSpawnPosition1;
    private Vector3 randomSpawnPosition2;
    private Vector3 randomSpawnPositionEnemy;
    private Vector3 randomSpawnPositionBossEnemy;
    private Vector3 randomSpawnPositionHeal;

    public int killCounter1 = 0;
    public int killCounter2 = 0;
    public int killStreakCounter1 = 0;
    public int killStreakCounter2 = 0;
    public int killCounterEnemy = 0;

    private MultipleTargetCam cam;
    private WaveManager waveManager;
    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindObjectOfType<AudioManager>();
    }
    
    void Start()
    {
        spawnPlayer1();
        spawnPlayer2();
        if (coop){
            cam = FindObjectOfType<MultipleTargetCam>();
            waveManager = GameObject.FindObjectOfType<WaveManager>();
            //spawnEnemy();
        }
        //Heals einmalig überall spawnen
        //Instantiate(healPrefab, randomSpawnPositionHeal, Quaternion.identity);
        Instantiate(healPrefab, healPos1.transform.position, Quaternion.identity);
        Instantiate(healPrefab, healPos2.transform.position, Quaternion.identity);
        Instantiate(healPrefab, healPos3.transform.position, Quaternion.identity);
        Instantiate(healPrefab, healPos4.transform.position, Quaternion.identity);
    }
    
    public GameObject spawnPlayer1()
    {
        audioManager.Play("Spawn");
        Vector3[] spawnPoints = new Vector3[] {player1pos1.transform.position, player1pos2.transform.position, player1pos3.transform.position, player1pos4.transform.position};
        randomSpawnPosition1 = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject spawnedPlayer = Instantiate(playerPrefab1, randomSpawnPosition1, Quaternion.identity);
        
        if (Game_Manager.instance != null && Game_Manager.instance.onPlayerSpawn != null)
        {
            Game_Manager.instance.onPlayerSpawn.Invoke();
        }
            
        if (coop){
            if (cam != null)
            {
                cam.AddTarget(GameObject.Find("Player1"));
            }
            if (waveManager != null)
            {
                waveManager.player.Add(spawnedPlayer);
            }
        }
        return spawnedPlayer;
    }

    public GameObject spawnPlayer2()
    {
        audioManager.Play("Spawn");
        Vector3[] spawnPoints = new Vector3[] {player2pos1.transform.position, player2pos2.transform.position, player2pos3.transform.position, player2pos4.transform.position};
        randomSpawnPosition2 = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject spawnedPlayer = Instantiate(playerPrefab2, randomSpawnPosition2, Quaternion.identity);

        if (Game_Manager.instance != null && Game_Manager.instance.onPlayerSpawn != null)
        {
            Game_Manager.instance.onPlayerSpawn.Invoke();
        }

        if (coop){
            if (cam != null)
            {
                cam.AddTarget(GameObject.Find("Player2"));
            }
            if (waveManager != null)
            {
                waveManager.player.Add(spawnedPlayer);
            }
        }
        return spawnedPlayer;
    }

    public GameObject spawnEnemy()
    {
        Vector3[] spawnPoints = new Vector3[] {enemyPos1.transform.position, enemyPos2.transform.position};
        randomSpawnPositionEnemy = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Store the instantiated enemy in a variable.
        GameObject spawnedEnemy = Instantiate(enemy, randomSpawnPositionEnemy, Quaternion.identity);
        
        // Return the newly spawned enemy.
        return spawnedEnemy;
    }

    public GameObject spawnEnemyFast()
    {
        Vector3[] spawnPoints = new Vector3[] {enemyPos1.transform.position, enemyPos2.transform.position};
        randomSpawnPositionEnemy = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Store the instantiated fast enemy in a variable.
        GameObject spawnedFastEnemy = Instantiate(enemyFast, randomSpawnPositionEnemy, Quaternion.identity);

        // Return the newly spawned fast enemy.
        return spawnedFastEnemy;
    }

    public GameObject spawnEnemyBoss()
    {
        Vector3[] spawnPoints = new Vector3[] {bossEnemyPos1.transform.position, bossEnemyPos2.transform.position};
        randomSpawnPositionBossEnemy = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Store the instantiated fast enemy in a variable.
        GameObject spawnedFastBoss = Instantiate(enemyBoss, randomSpawnPositionBossEnemy, Quaternion.identity);

        // Return the newly spawned fast enemy.
        return spawnedFastBoss;
    }

    public void spawnHeal()
    {
        /*if (!GameObject.Find("HealPot(Clone)"))
        {
            
        }*/
        Vector3[] spawnPoints = new Vector3[] {healPos1.transform.position, healPos2.transform.position, healPos3.transform.position, healPos4.transform.position};
        randomSpawnPositionHeal = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(healPrefab, randomSpawnPositionHeal, Quaternion.identity);
    }

    public void Die1()
    {
        spawnHeal();
        
        if (Game_Manager.instance != null && Game_Manager.instance.onPlayerDeath != null)
        {
            Game_Manager.instance.onPlayerDeath.Invoke();
        }

        GameObject playerToDestroy = GameObject.Find("Player1");
        
        if (playerToDestroy != null)
        {
            GameObject parentToDestroy = playerToDestroy.transform.parent.gameObject;
            if (parentToDestroy != null)
            {
                if (coop){
                    if (cam != null)
                    {
                        cam.RemoveTarget(playerToDestroy);
                    }
                }
                DestroyImmediate(parentToDestroy, true);
            }
        }

        killCounter2 += 1;
        if (!coop){
            killStreakCounter2 += 1;
            killStreakCounter1 = 0;
            spawnPlayer1();
        } else {
            Invoke("spawnPlayer1", 5f);
        }
        //sss.SetSplitScreen();
    }

    public void Die2()
    {
        spawnHeal();

        if (Game_Manager.instance != null && Game_Manager.instance.onPlayerDeath != null)
        {
            Game_Manager.instance.onPlayerDeath.Invoke();
        }
        
        GameObject playerToDestroy = GameObject.Find("Player2");

        if (playerToDestroy != null)
        {
            GameObject parentToDestroy = playerToDestroy.transform.parent.gameObject;
            if (parentToDestroy != null)
            {
                if (coop){
                    if (cam != null)
                    {
                        cam.RemoveTarget(playerToDestroy);
                    }
                }
                DestroyImmediate(parentToDestroy, true);
            }
        }

        killCounter1 += 1;
        if (!coop){
            killStreakCounter1 += 1;
            killStreakCounter2 = 0;
            spawnPlayer2();
        } else {
            Invoke("spawnPlayer2", 5f);
        }
        //sss.SetSplitScreen();
    }

    public void DieEnemy(GameObject enemy)
    {
        // Get the top-most parent object
        Transform topParent = enemy.transform;
        while (topParent.parent != null)
        {
            topParent = topParent.parent;
        }

        // Destroy the top-most parent object
        Destroy(topParent.gameObject);

        killCounterEnemy += 1;

        if (Game_Manager.instance != null && Game_Manager.instance.onEnemyDeath != null)
        {
            Game_Manager.instance.onEnemyDeath.Invoke();
        }
    }
}