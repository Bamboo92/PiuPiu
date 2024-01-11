using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public float damage;
    private float range;
    private float fireRate; // The rate at which the bazooka can fire
    public float rocketSpeed; // The speed of the rocket
    public float homingSensitivity; // The sensitivity of the rocket's homing capability
    public float explosionRadius;
    private int numOfBullets;

    //private Bazooka bazooka;
    private float detectionRange = 50;
    private float homingDelay = .3f; // Delay before starting homing behavior
    private float timeSinceLaunch;

    [SerializeField]
    private LayerMask enemyLayer;
    [SerializeField]
    private LayerMask playerLayer;
    [SerializeField]
    private LayerMask environmentLayer;

    [SerializeField]
    private GameObject impactEffect;

    private Transform target;
    private bool homingEnabled;

    private float acceleration = 100f; // Acceleration value, you can tweak this
    private float currentSpeed = 10f; // Initial speed of the rocket
    private float maxSpeed; // Max speed that the rocket can reach

    private AudioManager audioManager;
    private Spawn spawn;

    void Awake()
    {
        audioManager = GameObject.FindObjectOfType<AudioManager>();
        spawn = GameObject.FindObjectOfType<Spawn>();
    }

    void Start()
    {
        //bazooka = GameObject.FindObjectOfType<Bazooka>();
        timeSinceLaunch = Time.time;
        maxSpeed = rocketSpeed; // Set the max speed to the speed specified in Bazooka script
        //Debug.Log(damage + " " + range + " " + fireRate + " " + rocketSpeed + " " + homingSensitivity + " " + explosionRadius + " " + numOfBullets);
    }

    void Update()
    {
        if (Time.time - timeSinceLaunch > homingDelay)
        {
            homingEnabled = true;
        }

        // Increase the current speed until it reaches max speed
        if (currentSpeed < maxSpeed)
        {
            currentSpeed += acceleration * Time.deltaTime;
        }
        else
        {
            currentSpeed = maxSpeed;
        }

        if (homingEnabled)
        {
            // Find the closest enemy
            float closestDistance = Mathf.Infinity;
            if (spawn.coop)
            {
                Collider[] enemies = Physics.OverlapSphere(transform.position, detectionRange, enemyLayer);
                foreach (Collider enemy in enemies)
                {
                    float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                    if (distanceToEnemy < closestDistance)
                    {
                        closestDistance = distanceToEnemy;
                        target = enemy.transform;
                    }
                }
            } else {
                Collider[] players = Physics.OverlapSphere(transform.position, detectionRange, playerLayer);
                foreach (Collider player in players)
                {
                    float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
                    if (distanceToPlayer < closestDistance)
                    {
                        closestDistance = distanceToPlayer;
                        target = player.transform;
                    }
                }
            }


            if (target != null)
            {
                // Homing functionality
                Vector3 direction = (target.position - transform.position).normalized;
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, homingSensitivity * Time.deltaTime, 0.0f);
                transform.rotation = Quaternion.LookRotation(newDirection);
                transform.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World);
            }
            else
            {
                // If no target, just go straight
                transform.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World);
            }
        }
        else
        {
            // If homing not enabled, just go straight
            transform.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World);
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        // Only explode if the rocket hits an enemy, wall, or ground
        if (spawn.coop)
        {
            if ((enemyLayer == (enemyLayer | (1 << collision.gameObject.layer))) || (environmentLayer == (environmentLayer | (1 << collision.gameObject.layer))))
            {
                
                audioManager.Play("Explosion");

                RaycastHit hit;
                // Perform a raycast
                if (Physics.Raycast(transform.position, transform.forward, out hit))
                {
                    Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                    Explode();
                }
            }
        } else {
            if ((playerLayer == (playerLayer | (1 << collision.gameObject.layer))) || (environmentLayer == (environmentLayer | (1 << collision.gameObject.layer))))
            {
                
                audioManager.Play("Explosion");

                RaycastHit hit;
                // Perform a raycast
                if (Physics.Raycast(transform.position, transform.forward, out hit))
                {
                    Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                    Explode();
                }
            }
        }

    }

    void Explode()
    {
        // Get all colliders within the explosion radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider hit in colliders)
        {
            // Check if the object hit has the IDamageable interface
            IDamageable damageable = hit.GetComponent<IDamageable>();
            if (damageable != null)
            {
                // Calculate the distance to the hit object
                float distanceToHit = Vector3.Distance(transform.position, hit.transform.position);

                // Calculate the damage reduction as a proportion of the distance to the explosion radius
                float damageReduction = distanceToHit / explosionRadius;

                // Ensure the damage reduction is no more than 1
                damageReduction = Mathf.Min(damageReduction, 0.7f);

                // Apply damage to the object with reduction
                float reducedDamage = damage * (1f - damageReduction);
                damageable.TakeDamage(reducedDamage);
            }
        }

        // Destroy the rocket and create explosion effect
        Instantiate(impactEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    /*
    void OnDrawGizmos()
    {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, bazooka.explosionRadius);
    }*/
    public void SetRocket(float damage, float range, float fireRate, float rocketSpeed, float homingSensitivity, float explosionRadius, int numOfBullets)
    {
        this.damage = damage;
        this.range = range;
        this.fireRate = fireRate;
        this.rocketSpeed = rocketSpeed;
        this.homingSensitivity = homingSensitivity;
        this.explosionRadius = explosionRadius;
        this.numOfBullets = numOfBullets;
    }
}