using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    public enum State { Searching, Chasing, Menacing }
    private State currentState = State.Searching;

    public Transform player;
    public float detectionRange = 20f;
    public float teleportRadius = 100f;
    public float chaseDuration = 10f;
    public float menacingDuration = 5f;
    public float teleportCooldown = 5f;
    public float aggressionIncrease = 0.1f;
    public float maxAggression = 1.0f;

    private float detectionAngle = 90f;
    private NavMeshAgent agent;
    private float aggressionLevel = 0f;
    private int canTeleport = 0;
    private bool isChasing = false;

    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource roarSound;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(SearchRoutine());
    }

    void Update()
    {
        if (currentState == State.Searching)
        {
            LookForPlayer();
        }
        else if (currentState == State.Chasing)
        {
            agent.SetDestination(player.position);
        }

        if (agent.velocity.magnitude > 0.1f)
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }

    void LookForPlayer()
    {
        for (int i = -1; i <= 1; i++) // Looks left, center, and right
        {
            Vector3 direction = Quaternion.Euler(0, i * detectionAngle / 2, 0) * transform.forward;
            if (Physics.SphereCast(transform.position, 2f, direction, out RaycastHit hit, detectionRange))
            {
                if (hit.transform == player)
                {
                    if (Random.value < aggressionLevel)
                    {
                        StartCoroutine(ChasePlayer());
                    }
                    else
                    {
                        StartCoroutine(MenacePlayer());
                    }
                    return;
                }
            }
        }
    }

    IEnumerator SearchRoutine()
    {
        while (currentState == State.Searching)
        {
            agent.SetDestination(GetRandomNavMeshPosition());
            yield return new WaitForSeconds(5f);

            if (canTeleport > Random.Range(4,7))
            {
                canTeleport = 0;
                TeleportNearPlayer();
                yield return new WaitForSeconds(teleportCooldown);
            }
            else
            {
                canTeleport++;
            }
        }
    }

    IEnumerator ChasePlayer()
    {
        currentState = State.Chasing;
        isChasing = true;
        agent.SetDestination(player.position);
        roarSound.Play(); // Play roar sound when mad
        animator.SetTrigger("roar"); // Play roaring animation
        yield return new WaitForSeconds(chaseDuration);

        isChasing = false;
        TeleportRandomly();
        currentState = State.Searching;
    }

    IEnumerator MenacePlayer()
    {
        currentState = State.Menacing;
        agent.isStopped = true;
        transform.LookAt(player);
        aggressionLevel = Mathf.Clamp(aggressionLevel + aggressionIncrease, 0f, maxAggression);

        yield return new WaitForSeconds(menacingDuration);

        if (Vector3.Distance(transform.position, player.position) < 3f)
        {
            StartCoroutine(ChasePlayer());
        }
        else
        {
            TeleportRandomly();
            currentState = State.Searching;
        }
    }

    void TeleportNearPlayer()
    {
        Vector3 teleportPos = player.position + new Vector3(Random.Range(-teleportRadius, teleportRadius), 0, Random.Range(-teleportRadius, teleportRadius));
        transform.position = GetRandomNavMeshPosition(teleportPos);
    }

    void TeleportRandomly()
    {
        transform.position = GetRandomNavMeshPosition();
    }

    Vector3 GetRandomNavMeshPosition(Vector3? nearPosition = null)
    {
        Vector3 randomPoint = nearPosition ?? new Vector3(Random.Range(-teleportRadius, teleportRadius), 0, Random.Range(-teleportRadius, teleportRadius));
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 10f, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return transform.position;
    }
}
