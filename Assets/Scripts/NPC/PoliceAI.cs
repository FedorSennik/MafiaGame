using UnityEngine;
using UnityEngine.AI;

public class PoliceAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    private GunFire gunScript;

    private Transform playerTransform;
    private PlayerStats playerStats;

    [Header("Налаштування Патрулювання")]
    public Transform[] patrolPoints;
    public float patrolSpeed = 3.5f;

    [Header("Налаштування Переслідування")]
    public float chaseSpeed = 6f;
    public float detectionRange = 15f;
    public float graceTime = 3f;
    public float losePlayerRange = 25f;

    [Header("Налаштування Атаки")]
    public float shootingRange = 10f;

    private int currentPatrolPointIndex = 0;
    private float detectionTimer = 0f;
    private bool isChasing = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        gunScript = GetComponent<GunFire>();
        animator = GetComponent<Animator>();

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
            playerStats = playerObject.GetComponent<PlayerStats>();
        }

        if (patrolPoints.Length > 0)
        {
            SetPatrolDestination();
        }
    }

    private void Update()
    {
        if (playerStats == null || !playerStats.IsAlive)
        {
            agent.isStopped = true;
            UpdateAnimations(0, false);
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        if (isChasing)
        {
            HandleChaseBehavior(distanceToPlayer);
        }
        else
        {
            HandlePatrolBehavior(distanceToPlayer);
        }

        UpdateAnimations(agent.velocity.magnitude, isChasing);
    }

    private void HandlePatrolBehavior(float distanceToPlayer)
    {
        agent.speed = patrolSpeed;

        if (distanceToPlayer <= detectionRange)
        {
            detectionTimer += Time.deltaTime;
            if (detectionTimer >= graceTime)
            {
                isChasing = true;
                Debug.Log("Гравець виявлений! Починається переслідування.");
            }
        }
        else
        {
            detectionTimer = 0;
        }

        if (patrolPoints.Length > 0 && !agent.pathPending && agent.remainingDistance < 1f)
        {
            currentPatrolPointIndex = (currentPatrolPointIndex + 1) % patrolPoints.Length;
            SetPatrolDestination();
        }
    }

    private void HandleChaseBehavior(float distanceToPlayer)
    {
        if (distanceToPlayer > losePlayerRange)
        {
            isChasing = false;
            Debug.Log("Гравець втрачений. Повернення до патрулювання.");
            detectionTimer = 0;

            FindClosestPatrolPoint();
            SetPatrolDestination();
            return;
        }

        agent.speed = chaseSpeed;
        agent.SetDestination(playerTransform.position);

        if (distanceToPlayer <= shootingRange)
        {
            if (gunScript != null)
            {
                gunScript.Fire();
            }
        }
    }

    private void UpdateAnimations(float speed, bool isChasing)
    {
        if (animator != null)
        {
            animator.SetFloat("Speed", speed);
            animator.SetBool("IsChasing", isChasing);
        }
    }

    private void SetPatrolDestination()
    {
        if (patrolPoints.Length > 0)
        {
            agent.SetDestination(patrolPoints[currentPatrolPointIndex].position);
        }
    }

    private void FindClosestPatrolPoint()
    {
        float minDistance = float.MaxValue;
        int closestPointIndex = 0;

        for (int i = 0; i < patrolPoints.Length; i++)
        {
            if (patrolPoints[i] == null) continue;

            float distance = Vector3.Distance(transform.position, patrolPoints[i].position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestPointIndex = i;
            }
        }
        currentPatrolPointIndex = closestPointIndex;
    }
}