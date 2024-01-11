using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLife1 : MonoBehaviour, IDamageable
{
    [System.Serializable]
    public class DropItem
    {
        public GameObject item;
        public float dropRate;
    }

    public List<DropItem> dropItems = new List<DropItem>();

    public float maxHealth = 100f;
    public float currentHealth;

    [SerializeField]
    private HealthBar healthBar;

    private int isDyingHash;
    private Animator animator;

    private Spawn spawn;

    [SerializeField]
    private Collider playerCollider;

    [SerializeField]
    private PlayerInput playerInput;

    public bool isDead = false;

    void Start()
    {
        spawn = GameObject.FindObjectOfType<Spawn>();
        StartCoroutine(fillHealth());
        healthBar.SetMaxHealth((int)maxHealth);

        animator = GetComponent<Animator>();
        isDyingHash = Animator.StringToHash("isDying");
    }

    public void TakeDamage(float damage)
    {
        StartCoroutine(VibrateController(0.1f, 0.5f));

        currentHealth -= damage;
        healthBar.SetHealth((int)currentHealth);

        if (currentHealth <= 0)
        {
            isDead = true;
            StopVibration();
            playerCollider.enabled = false;
            handleAnimation();
        }
    }
    
    void handleAnimation()
    {
        bool isDying = animator.GetBool(isDyingHash);
        animator.SetBool(isDyingHash, true);

        StartCoroutine(Destroy());
    }

    public IEnumerator VibrateController(float duration, float intensity)
    {
        // Get all gamepad devices connected
        var gamepads = Gamepad.all;

        // If there is at least one gamepad
        if(gamepads.Count > 0)
        {
            foreach (Gamepad gamepad in gamepads)
            {
                // Vibrate every gamepad found
                gamepad.SetMotorSpeeds(intensity, intensity);
            }

            // Wait for the duration of the vibration
            yield return new WaitForSeconds(duration);


            foreach (Gamepad gamepad in gamepads)
            {
                //Stop Vibration
                gamepad.SetMotorSpeeds(0, 0);
            }

        }
    }
    
    public void StopVibration()
    {
        // Get all gamepad devices connected
        var gamepads = Gamepad.all;

        // If there is at least one gamepad
        if(gamepads.Count > 0){
            foreach (Gamepad gamepad in gamepads)
            {
                //Stop Vibration
                gamepad.SetMotorSpeeds(0, 0);
            }
        }
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(3.7f);
        TryDropItem();
        spawn.Die1();
    }

    public IEnumerator fillHealth()
    {
        while (currentHealth < maxHealth){
            currentHealth += 1f;
            healthBar.SetHealth((int)currentHealth);
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void TryDropItem()
    {
        float noDropChance = 0.1f; // 10% chance of no drop
        float roll = Random.value; // Roll a random number between 0 and 1.

        if (roll < noDropChance)
        {
            return; // No drop in this case
        }

        roll = (roll - noDropChance) / (1 - noDropChance); // Normalize the roll for the remaining drop rates.

        float cumulative = 0f;
        foreach (DropItem dropItem in dropItems)
        {
            cumulative += dropItem.dropRate;
            if (roll <= cumulative)
            {
                Drop(dropItem.item);
                break; // Only drop one item, so break once we've dropped something.
            }
        }
    }

    private void Drop(GameObject itemToDrop)
    {
        // Set the rotation to be 45 degrees around the Y axis.
        Quaternion rotation = Quaternion.Euler(0, 45, 0);
        
        // Adjust the position to be 0.2 units above the current position.
        Vector3 position = new Vector3(transform.position.x, 0.2f, transform.position.z);
        
        // Instantiate the item at the specified position and with the specified rotation.
        Instantiate(itemToDrop, position, rotation);
    }
}