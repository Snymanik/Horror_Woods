using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class MonsterAI : MonoBehaviour
{
    public enum State { Searching, Chasing, Menacing }
    private State currentState = State.Searching;

    public Transform player;
    public float detectionRange = 20f;
    public float teleportRadius = 100;
    public float chaseDuration = 10f;
    public float menacingDuration = 5f;
    public float teleportCooldown = 5f;
    public float aggressionIncrease = 0.1f;
    public float maxAggression = 1.0f;

    private float detectionAngle = 120f;
    private NavMeshAgent agent;
    private float aggressionLevel = 0f;
    private int canTeleport = 0;
    private bool isChasing = false;

    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource roarSound;


    private float viewRadius = 30;
    private float viewAngle = 45;
    [SerializeField] LayerMask playerMask, obstacleMask;


    void Start()
    {
        GetComponent<Animator>().applyRootMotion = false;
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(SearchRoutine());
    }

    void Update()
    {
        
        if (currentState == State.Chasing)
        {
            agent.SetDestination(player.position);
        }

        if (agent.velocity.magnitude > 0.1f)
        {
           // animator.SetBool("isMoving", true);
        }
        else
        {
            //animator.SetBool("isMoving", false);
        }


        CheckRange();
        
    }

    private void CheckRange()
    {
        if (Mathf.Pow(Mathf.Pow(Mathf.Abs(transform.position.x - player.position.x), 2) + Mathf.Pow(Mathf.Abs(transform.position.z - player.position.z), 2), 0.5f) < 20 && !isChasing)
        {
            StopCoroutine(SearchRoutine());
            StartCoroutine(ChasePlayer());
            //Debug.Log("ITS CLOSE");
        }
        
    }

    void LookForPlayer()
    {
        if (currentState == State.Searching) 
            {
            Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, playerMask);

            foreach (var target in targetsInViewRadius)
            {
                Vector3 dirToTarget = (target.transform.position - transform.position).normalized;
                float angleToTarget = Vector3.Angle(transform.forward, dirToTarget);

                if (angleToTarget < viewAngle / 2f)
                {
                    float dstToTarget = Vector3.Distance(transform.position, target.transform.position);

                    if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                    {
                        Debug.Log("omg it works");
                        StopCoroutine(SearchRoutine());
                        if (Random.Range(1, 10) < aggressionLevel)
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
            
        

    }

    IEnumerator SearchRoutine()
    {
        while (currentState == State.Searching)
        {

            agent.SetDestination(GetRandomNavMeshPosition(transform.position)); // check
            LookForPlayer();
            yield return new WaitForSeconds(5f);

            if (canTeleport > Random.Range(5,13))
            {
                canTeleport = 0;
                TeleportNearPlayer();
                yield return new WaitForSeconds(teleportCooldown);
                agent.SetDestination(GetRandomNavMeshPosition(transform.position));
            }
            else
            {
                yield return new WaitForSeconds(1f);
                canTeleport++;
                LookForPlayer();
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
        yield return new WaitForSeconds(teleportCooldown);

        currentState = State.Searching;
        StartCoroutine(SearchRoutine());
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
            StartCoroutine(SearchRoutine());
        }
    }

    void TeleportNearPlayer()
    {
        
        transform.position = GetRandomNavMeshPosition(player.position);
        agent.ResetPath();
    }

    void TeleportRandomly()
    {
        //Vector3 RandomlyPlayer = new Vector3(Random.Range(-teleportRadius, teleportRadius), 1, Random.Range(-teleportRadius, teleportRadius)) + player.position;
        transform.position = GetRandomNavMeshPosition(player.position);
        agent.ResetPath();
    }

    Vector3 GetRandomNavMeshPosition(Vector3 nearPosition)
    {
        Vector3 randomPoint =  new Vector3(Random.Range(-teleportRadius, teleportRadius) + nearPosition.x, 1 + nearPosition.y, Random.Range(-teleportRadius, teleportRadius) + nearPosition.z);
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 10f, NavMesh.AllAreas))
        {
            if (Vector3.Distance(transform.position, hit.position) > 5f) // Only if not too close
                return hit.position;
            
        }
        return transform.position;
    }
}
