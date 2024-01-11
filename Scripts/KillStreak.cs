using UnityEngine;
using TMPro;

public class KillStreak : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro killStreakCounterText;
    [SerializeField]
    private TMP_ColorGradient firstGradient, secondGradient, thirdGradient, fourthGradient;
    [SerializeField]
    private bool forPlayer1, forPlayer2;

    private Spawn spawn;

    void Awake()
    {
        spawn = GameObject.FindObjectOfType<Spawn>();

        if (!spawn.coop) {
            killStreakCounterText.colorGradientPreset = firstGradient;
        } else if (spawn.coop) {
            killStreakCounterText.text = "";
        }
    }

    void killStreakCounter()
    {
        if (forPlayer1){
            killStreakCounterText.text = spawn.killStreakCounter1.ToString("");

            if (6 > spawn.killStreakCounter1 && spawn.killStreakCounter1 >= 3){
                killStreakCounterText.colorGradientPreset = secondGradient;
            } else if (9 > spawn.killStreakCounter1 && spawn.killStreakCounter1 >= 6){
                killStreakCounterText.colorGradientPreset = thirdGradient;
            } else if (spawn.killStreakCounter1 >= 9){
                killStreakCounterText.colorGradientPreset = fourthGradient;
            } else {
                killStreakCounterText.colorGradientPreset = firstGradient;
            }
        } else if (forPlayer2){
            killStreakCounterText.text = spawn.killStreakCounter2.ToString("");

            if (6 > spawn.killStreakCounter2 && spawn.killStreakCounter2 >= 3){
                killStreakCounterText.colorGradientPreset = secondGradient;
            } else if (9 > spawn.killStreakCounter2 && spawn.killStreakCounter2 >= 6){
                killStreakCounterText.colorGradientPreset = thirdGradient;
            } else if (spawn.killStreakCounter2 >= 9){
                killStreakCounterText.colorGradientPreset = fourthGradient;
            } else {
                killStreakCounterText.colorGradientPreset = firstGradient;
            }
        }
    }

    void OnEnable()
    {
        if (!spawn.coop){
           Game_Manager.instance.onPlayerSpawn.AddListener(killStreakCounter);
        }
    }

    void OnDisable()
    {
        
    }
}
