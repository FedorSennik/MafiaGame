using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class PoliceAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    private PistolFire gunScript;

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
    public Transform firePoint;
    public Transform armToRaise;
    public float shootingRange = 10f;
    public float shootingAnimationTime = 0.5f;

    private int currentPatrolPointIndex = 0;
    private float detectionTimer = 0f;
    private bool isChasing = false;
    private bool canShoot = true;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        gunScript = GetComponent<PistolFire>();
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
        if (playerTransform != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            if (distanceToPlayer <= detectionRange)
            {
                isChasing = true;
                ChasePlayer(distanceToPlayer);
            }
            else
            {
                isChasing = false;
                Patrol();
            }
        }
        else
        {
            Patrol();
        }

        UpdateAnimations();
    }

    private void ChasePlayer(float distanceToPlayer)
    {
        agent.speed = chaseSpeed;
        agent.SetDestination(playerTransform.position);

        if (distanceToPlayer <= shootingRange)
        {
            agent.isStopped = true;

            Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0, directionToPlayer.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * agent.angularSpeed);

            if (canShoot)
            {
                StartCoroutine(Shoot());
            }
        }
        else
        {
            agent.isStopped = false;
        }
    }

    private void Patrol()
    {
        agent.speed = patrolSpeed;
        agent.isStopped = false;

        if (patrolPoints.Length == 0) return;

        if (Vector3.Distance(transform.position, agent.destination) < 1f)
        {
            currentPatrolPointIndex = (currentPatrolPointIndex + 1) % patrolPoints.Length;
            SetPatrolDestination();
        }
    }

    private IEnumerator Shoot()
    {
        canShoot = false;
        if (animator != null)
        {
            animator.SetBool("IsShooting", true);
        }

        if (armToRaise != null && playerTransform != null)
        {
            armToRaise.LookAt(playerTransform);
        }

        if (gunScript != null && firePoint != null)
        {
            gunScript.FireAtTarget(firePoint, playerTransform);
        }

        yield return new WaitForSeconds(shootingAnimationTime);

        if (animator != null)
        {
            animator.SetBool("IsShooting", false);
        }

        if (armToRaise != null)
        {
            armToRaise.localRotation = Quaternion.identity;
        }

        yield return new WaitForSeconds(gunScript.fireRate);
        canShoot = true;
    }

    private void UpdateAnimations()
    {
        if (animator != null)
        {
            float speed = agent.velocity.magnitude;
            animator.SetFloat("Speed", speed);
        }
    }

    private void SetPatrolDestination()
    {
        if (patrolPoints.Length > 0)
        {
            agent.SetDestination(patrolPoints[currentPatrolPointIndex].position);
        }
    }
}