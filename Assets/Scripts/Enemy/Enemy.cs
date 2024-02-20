using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;

    [SerializeField]
    private float patrolDistance;

    [SerializeField]
    private float hearingTimeOut, watchingTimeOut;

    public bool IsPlayerInSight { get; private set; }
    public bool IsHearingSomething { get; private set; }

    public Vector3 LastSoundPosition { get; private set; }

    public Player Player { get; private set; }

    private float originalSpeed;

    private Animator animator;

    public EnemySounds EnemySounds { get; private set; }

    private void Start()
    {
        Player = FindObjectOfType<Player>();
        agent = GetComponent<NavMeshAgent>();
        originalSpeed = agent.speed;

        animator = GetComponent<Animator>();
        EnemySounds = GetComponent<EnemySounds>();
    }

    public void PlayerInSight(bool value)
    {
        IsPlayerInSight = value;
    }

    public IEnumerator WatchingTimeout()
    {
        yield return new WaitForSeconds(watchingTimeOut);
        IsPlayerInSight = false;
    }

    public void HearingSound(Vector3 position)
    {
        LastSoundPosition = position;
        IsHearingSomething = true;
        StartCoroutine(HearingTimeout());
    }

    private IEnumerator HearingTimeout()
    {
        yield return new WaitForSeconds(hearingTimeOut);
        IsHearingSomething = false;
    }

    public void StartPatrol()
    {
        agent.speed = originalSpeed / 2;
        agent.destination = RandomNavSphere(transform.position, -1);
        animator.SetFloat("MoveSpeed", .2f);
        EnemySounds.PlayCreepyVO();
    }

    public void MoveToSound()
    {
        agent.speed = originalSpeed;
        agent.destination = LastSoundPosition;
        animator.SetFloat("MoveSpeed", .5f);
        EnemySounds.PlayCreepyVO();
    }

    public void ChasePlayer()
    {
        agent.speed = originalSpeed * 2;
        agent.destination = Player.transform.position;
        animator.SetFloat("MoveSpeed", 1f);
        EnemySounds.PlayCreepyVO();
    }

    private Vector3 RandomNavSphere(Vector3 origin, int layermask)
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * patrolDistance;

        randomDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, patrolDistance, layermask);

        return navHit.position;
    }

    public bool IsDestinationReached()
    {
        if (agent.remainingDistance <= 1f) return true;
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player.Death();
        }
    }
}
