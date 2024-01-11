using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootSensor : MonoBehaviour
{
    private WaveManager waveManager;

    [SerializeField]
    private MeshRenderer renderer1;

    public List<(string, int)> possibleLoot = new List<(string, int)> { ("Pistol", 0), ("Uzi", 2), ("Shotgun", 5), ("Bazooka", 8)};

    private Coroutine destroyCoroutine;
    private AudioManager audioManager;

    private void Awake()
    {
        waveManager = GameObject.FindObjectOfType<WaveManager>();
        audioManager = GameObject.FindObjectOfType<AudioManager>();
    }

    private void Start()
    {
        destroyCoroutine = StartCoroutine(DestroyAfterDelay(3.5f));
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            audioManager.Play("Loot");
            // Get the Weapon component from the player who entered the trigger
            Weapon weapon = collision.gameObject.GetComponentInChildren<Weapon>();
            WeaponLVL weaponLVL = collision.gameObject.GetComponentInChildren<WeaponLVL>();
            if (weapon != null)
            {
                DropLoot(weapon, waveManager.waveCount - 1, weaponLVL);
            }
            renderer1.enabled = false;
            StopCoroutine(destroyCoroutine);
            Destroy(gameObject);
        }
    }

    public void DropLoot(Weapon weapon, int playerLevel, WeaponLVL weaponLVL)
    {
        List<(string itemName, int minLevel)> eligibleLoot = new List<(string itemName, int minLevel)>();

        foreach (var loot in possibleLoot)
        {
            if (playerLevel >= loot.Item2)
            {
                eligibleLoot.Add(loot);
            }
        }

        if (eligibleLoot.Count > 0)
        {
            var lootToDrop = eligibleLoot[Random.Range(0, eligibleLoot.Count)];

            switch (lootToDrop.Item1)
            {
                case "Pistol":
                    IWeapon newPistol = weapon.gameObject.GetComponent<Pistol>();
                    if (playerLevel == 1) 
                    {
                        newPistol.SetWeapon("LVL 2 Pistol", 25f, 20f, 3f, 100f, 1f, .1f, 0);
                    }
                    else if (playerLevel >= 2 &&  playerLevel < 4) 
                    {
                        newPistol.SetWeapon("LVL 3 Pistol", 50f, 20f, 3f, 100f, 1f, .1f, 0);
                    }
                    else if (playerLevel >= 4 &&  playerLevel < 6) 
                    {
                        newPistol.SetWeapon("LVL 4 Pistol", 50f, 20f, 4.5f, 100f, 0f, .0f, 0);
                    }
                    else if (playerLevel >= 6 &&  playerLevel < 10) 
                    {
                        newPistol.SetWeapon("LVL 5 Pistol", 50f, 20f, 6f, 100f, 0f, .0f, 0);
                    }
                    else if (playerLevel >= 10) 
                    {
                        newPistol.SetWeapon("LVL 6 Pistol", 100f, 20f, 6f, 100f, 0f, .0f, 0);
                    }
                    else 
                    {
                        Debug.Log("Couldn't SetWeapon()");
                    }
                    weapon.AddWeapon(newPistol);
                    weapon.EquipWeapon(newPistol);
                    break;
                case "Uzi":
                    IWeapon newUzi = weapon.gameObject.GetComponent<Uzi>();
                    if (playerLevel == 2) 
                    {
                        newUzi.SetWeapon("LVL 1 Uzi", 10f, 12f, 10f, 200f, 20f, 1.5f, 0);
                    }
                    else if (playerLevel >= 3 &&  playerLevel < 6) 
                    {
                        newUzi.SetWeapon("LVL 2 Uzi", 20f, 12f, 10f, 200f, 20f, 1.5f, 0);
                    }
                    else if (playerLevel >= 6 &&  playerLevel < 8) 
                    {
                        newUzi.SetWeapon("LVL 3 Uzi", 20f, 12f, 20f, 375f, 10f, 1.0f, 0);
                    }
                    else if (playerLevel >= 8 &&  playerLevel < 11)
                    {
                        newUzi.SetWeapon("LVL 4 Uzi", 40f, 12f, 20f, 375f, 5f, 1.0f, 0);
                    }
                    else if (playerLevel >= 11) 
                    {
                        newUzi.SetWeapon("LVL 5 Uzi", 80f, 15f, 40f, 375f, 3f, 0.5f, 0);
                    }
                    else 
                    {
                        Debug.Log("Couldn't SetWeapon()");
                    }
                    weapon.AddWeapon(newUzi);
                    weapon.EquipWeapon(newUzi);
                    break;
                case "Shotgun":
                    IWeapon newShotgun = weapon.gameObject.GetComponent<Shotgun>();
                    if (playerLevel == 5) 
                    {
                        newShotgun.SetWeapon("LVL 1 Shotgun", 25f, 8f, 2.5f, 200f, 30f, 2f, 3);
                    }
                    else if (playerLevel >= 6 &&  playerLevel < 9) 
                    {
                        newShotgun.SetWeapon("LVL 2 Shotgun", 50f, 8f, 2.5f, 200f, 30f, 2f, 3);
                    }
                    else if (playerLevel >= 9 &&  playerLevel < 11) 
                    {
                        newShotgun.SetWeapon("LVL 3 Shotgun", 50f, 8f, 4f, 375f, 30f, 1f, 6);
                    }
                    else if (playerLevel >= 11 &&  playerLevel < 12) 
                    {
                        newShotgun.SetWeapon("LVL 4 Shotgun", 100f, 8f, 6f, 375f, 30f, 1f, 6);
                    }
                    else if (playerLevel >= 12) 
                    {
                        newShotgun.SetWeapon("LVL 5 Shotgun", 100f, 10f, 7.5f, 375f, 45f, 1f, 10);
                    }
                    else 
                    {
                        Debug.Log("Couldn't SetWeapon()");
                    }
                    weapon.AddWeapon(newShotgun);
                    weapon.EquipWeapon(newShotgun);
                    break;
                case "Bazooka":
                    IWeapon newBazooka = weapon.gameObject.GetComponent<Bazooka>();
                    if (playerLevel == 8) 
                    {
                        newBazooka.SetWeapon("LVL 1 Bazooka", 70f, 200f, 1f, 40f, 0f, 10f, 1);
                    }
                    else if (playerLevel >= 9 &&  playerLevel < 12) 
                    {
                        newBazooka.SetWeapon("LVL 2 Bazooka", 70f, 200f, 1f, 40f, 2f, 7f, 1);
                    }
                    else if (playerLevel >= 12 &&  playerLevel < 14) 
                    {
                        newBazooka.SetWeapon("LVL 3 Bazooka", 70f, 200f, 1f, 140f, 3f, 7f, 1);
                    }
                    else if (playerLevel >= 14 &&  playerLevel < 16) 
                    {
                        newBazooka.SetWeapon("LVL 4 Bazooka", 100f, 200f, 1.3f, 140f, 10f, 7f, 1);
                    }
                    else if (playerLevel >= 16) 
                    {
                        newBazooka.SetWeapon("LVL 5 Bazooka", 100f, 200f, 2f, 140f, 30f, 4.5f, 1);
                    }
                    else 
                    {
                        Debug.Log("Couldn't SetWeapon()");
                    }
                    weapon.AddWeapon(newBazooka);
                    weapon.EquipWeapon(newBazooka);
                    break;
                default:
                    Debug.Log("Unknown weapon." + lootToDrop.Item1 + " " + lootToDrop.Item2);
                    break;
            }
        } else {
            Debug.Log("No eligible loot to drop.");
        }
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}