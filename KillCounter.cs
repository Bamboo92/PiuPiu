using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class KillCounter : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI killCounterPlayer1, killCounterPlayer2;
    [SerializeField]
    private TextMeshProUGUI killCounterCoop;

    [SerializeField]
    private TextMeshProUGUI waveCooldown;
    [SerializeField]
    private TextMeshProUGUI waveCounter;
    private int realWaveCount;

    private Spawn spawn;
    private WaveManager waveManager;

    void Awake()
    {
        spawn = GameObject.FindObjectOfType<Spawn>();

        if (spawn.coop){
            waveManager = GameObject.FindObjectOfType<WaveManager>();
            killCounterEnemy();
        } else if (!spawn.coop) {
            killCounterPlayer();
        }
    }

    void killCounterPlayer()
    {
        killCounterPlayer1.text = spawn.killCounter1.ToString("");
        killCounterPlayer2.text = spawn.killCounter2.ToString("");
    }

    void killCounterEnemy()
    {
        killCounterCoop.text = spawn.killCounterEnemy.ToString("");
    }

    private IEnumerator setWaveCooldown()
    {
        waveCooldown.text = waveManager.waveCooldown.ToString("F0");
        yield return new WaitForSeconds(4);
        waveCooldown.text = "";
    }

    void setWaveCounter()
    {
        realWaveCount = waveManager.waveCount - 1;
        waveCounter.text = realWaveCount.ToString("");
    }

    void OnEnable()
    {
        if (spawn.coop){
            Game_Manager.instance.onWaveCooldown.AddListener(StartWaveCooldown);
            Game_Manager.instance.onWaveCooldown.AddListener(setWaveCounter);
            Game_Manager.instance.onEnemyDeath.AddListener(killCounterEnemy);
        } else if (!spawn.coop) {
            Game_Manager.instance.onPlayerSpawn.AddListener(killCounterPlayer);
        }
    }

    void OnDisable()
    {
        
    }

    void StartWaveCooldown()
    {
        StartCoroutine(setWaveCooldown());
    }
}
