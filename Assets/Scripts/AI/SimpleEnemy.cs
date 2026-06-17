using UnityEngine;

/// <summary>
/// Simple enemy AI with patrol, chase, and attack behaviors.
/// </summary>
public class SimpleEnemy : MonoBehaviour
{
    [Header("AI Settings")]
    [SerializeField] private float detectionRange = 20f;
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private float chaseSpeed = 5f;
    [SerializeField] private float attackCooldown = 1f;

    [Header("Patrol Settings")]
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float stoppingDistance = 0.5f;

    private Rigidbody rb;
    private Health health;
    private Transform player;
    private int currentPatrolIndex = 0;
    private float lastAttackTime = -1f;
    private bool playerDetected = false;

    private enum EnemyState { Patrol, Chase, Attack, Dead }
    private EnemyState currentState = EnemyState.Patrol;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        health = GetComponent<Health>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (health != null)
        {
            health.OnDeath.AddListener(OnDeath);
        }
    }

    private void Update()
    {
        if (health != null && health.IsDead()) return;

        UpdateState();
        ExecuteState();
    }

    private void UpdateState()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Update player detection
        playerDetected = distanceToPlayer <= detectionRange;

        if (!playerDetected)
        {
            currentState = EnemyState.Patrol;
            return;
        }

        // Player detected, decide between chase or attack
        if (distanceToPlayer <= attackRange)
        {
            currentState = EnemyState.Attack;
        }
        else
        {
            currentState = EnemyState.Chase;
        }
    }

    private void ExecuteState()
    {
        switch (currentState)
        {
            case EnemyState.Patrol:
                Patrol();
                break;
            case EnemyState.Chase:
                Chase();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Dead:
                break;
        }
    }

    private void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        Transform targetPoint = patrolPoints[currentPatrolIndex];
        Vector3 directionToPoint = (targetPoint.position - transform.position).normalized;

        // Move toward patrol point
        rb.velocity = new Vector3(directionToPoint.x * patrolSpeed, rb.velocity.y, directionToPoint.z * patrolSpeed);

        // Rotate to face direction
        if (directionToPoint.magnitude > 0.1f)
        {
            transform.rotation = Quaternion.LookRotation(directionToPoint);
        }

        // Check if reached patrol point
        if (Vector3.Distance(transform.position, targetPoint.position) < stoppingDistance)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }

    private void Chase()
    {
        if (player == null) return;

        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        // Move toward player
        rb.velocity = new Vector3(directionToPlayer.x * chaseSpeed, rb.velocity.y, directionToPlayer.z * chaseSpeed);

        // Rotate to face player
        transform.rotation = Quaternion.LookRotation(directionToPlayer);
    }

    private void Attack()
    {
        if (player == null) return;

        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(directionToPlayer);

        // Stop moving
        rb.velocity = new Vector3(0, rb.velocity.y, 0);

        // Check if can attack
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            Health playerHealth = player.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(10f);
                lastAttackTime = Time.time;
            }
        }
    }

    private void OnDeath()
    {
        currentState = EnemyState.Dead;
    }

    private void OnDrawGizmosSelected()
    {
        // Draw detection range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Draw attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // Draw patrol points
        Gizmos.color = Color.blue;
        if (patrolPoints != null)
        {
            foreach (Transform point in patrolPoints)
            {
                if (point != null)
                    Gizmos.DrawWireSphere(point.position, 0.5f);
            }
        }
    }
}
