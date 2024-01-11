using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLife : MonoBehaviour, IDamageable
{
    [System.Serializable]
    public class DropItem
    {
        public GameObject item;
        public float dropRate;
    }

    public float maxHealth = 100f;
    public float currentHealth;

    private int isDyingHash;
    private Animator animator;

    private Spawn spawn;

    [SerializeField]
    private Collider agentCollider;

    public List<DropItem> dropItems = new List<DropItem>();
    
    void Start()
    {
        spawn = GameObject.FindObjectOfType<Spawn>();

        animator = GetComponent<Animator>();
        isDyingHash = Animator.StringToHash("isDying");
        currentHealth = 100;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            agentCollider.enabled = false;
            handleAnimation();
        }
    }
    
    void handleAnimation()
    {
        bool isDying = animator.GetBool(isDyingHash);
        animator.SetBool(isDyingHash, true);

        StartCoroutine(Destroy());
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(1.8f);
        TryDropItem();
        spawn.DieEnemy(this.gameObject);
    }   

    public void TryDropItem()
    {
        float noDropChance = 0.6f; // 60% chance of no drop
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