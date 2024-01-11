using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Chase_Player : MonoBehaviour
{
    [SerializeField]
    private float chaseRange = 200.0f;
    [SerializeField]
    private float attackRange = 3f;
    [SerializeField]
    private float attackCooldown = 2.0f;
    [SerializeField]
    private float nextAttackTime = 0.5f;

    [SerializeField]
    private NavMeshAgent agent;
    private GameObject[] players;
    private Transform target = null;

    private Animator animator;
    private int isWalkingHash;
    private int isPunchingHash;
    private int isIdleHash;

    private EnemyLife enemyLife;

    private bool isAttacking = false;
    [SerializeField]
    private bool forBoss = false;

    private Weapon weapon;
    private IWeapon newBazooka;

    void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        enemyLife = GameObject.FindObjectOfType<EnemyLife>();
        weapon = gameObject.GetComponent<Weapon>();


        isWalkingHash = Animator.StringToHash("isWalking");
        isPunchingHash = Animator.StringToHash("isPunching");
        isIdleHash = Animator.StringToHash("isIdle");
        UpdatePlayers();
        agent.isStopped = false;
    }

    void Update()
    {
        if(!isAttacking){
            if(enemyLife.currentHealth > 0){
                FollowOrAttackPlayer();
            } else {
                agent.isStopped = true;
            }
        }
    }

    Transform FindClosestPlayer()
    {
        Transform closestPlayer = null;
        float closestDistance = Mathf.Infinity;

        if (players != null && players.Length > 0) // Überprüfen Sie, ob Spieler existieren
        {
            foreach (GameObject player in players)
            {
                if (player != null) // Überprüfen Sie, ob der Spieler existiert
                {
                    float distance = Vector3.Distance(transform.position, player.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestPlayer = player.transform;
                    }
                }
            }
        }

        return closestPlayer;
    }

    void Attack()
    {
        agent.isStopped = true;
        if (Time.time >= nextAttackTime)
        {
            animator.SetBool(isPunchingHash, true);
            nextAttackTime = Time.time + attackCooldown;
            StartCoroutine(CompleteAttack());
        }
    }

    private IEnumerator CompleteAttack()
    {
        yield return new WaitForSeconds(attackCooldown);
        if (gameObject.activeInHierarchy)
        {
            agent.isStopped = false;
        }
        isAttacking = false;
    }

    void BossAttack()
    {
        agent.isStopped = true;
        if (Time.time >= nextAttackTime)
        {
            animator.SetBool(isPunchingHash, true);
            nextAttackTime = Time.time + attackCooldown;
            StartCoroutine(CompleteBossAttack());
        }
    }

    private IEnumerator CompleteBossAttack()
    {
        newBazooka = gameObject.GetComponent<Bazooka>();
        newBazooka.SetWeapon("LVL 4 Bazooka", 30f, 20f, 1.3f, 20f, 4f, 6f, 1);
        weapon.AddWeapon(newBazooka);
        weapon.EquipWeapon(newBazooka);
        yield return new WaitForSeconds(1);
        weapon.Shoot();
        if (gameObject.activeInHierarchy)
        {
            agent.isStopped = false;
        }
        isAttacking = false;
    }

    void FollowOrAttackPlayer()
    {   
        UpdateTarget();
        if (target != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            if (distanceToTarget <= attackRange)
            {
                isAttacking = true;
                if (forBoss)
                {
                    BossAttack();
                } else {
                    Attack();
                }

            }
            else if (distanceToTarget >= attackRange && distanceToTarget <= chaseRange) // Check if the enemy is currently not punching
            {
                // Not in attack range
                animator.SetBool(isPunchingHash, false);
                animator.SetBool(isWalkingHash, true);
                agent.isStopped = false;
                agent.SetDestination(target.position);
            }
        }
        else
        {
            // No target
            animator.SetBool(isPunchingHash, false);
            animator.SetBool(isWalkingHash, false);
            UpdatePlayers();
            if (target != null) 
            {
                animator.SetBool(isWalkingHash, true);
                agent.isStopped = false;
                agent.SetDestination(target.position);
            }
        }
    }

    void OnEnable()
    {
        Game_Manager.instance.onPlayerSpawn.AddListener(UpdatePlayers);
        Game_Manager.instance.onPlayerDeath.AddListener(UpdateTarget);
    }

    void OnDisable()
    {
        Game_Manager.instance.onPlayerSpawn.RemoveListener(UpdatePlayers);
        Game_Manager.instance.onPlayerDeath.RemoveListener(UpdateTarget);
    }

    void UpdatePlayers()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        target = FindClosestPlayer();
    }

    void UpdateTarget()
    {
        target = FindClosestPlayer();
    }
    
}