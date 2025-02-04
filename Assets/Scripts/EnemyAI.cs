using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform target;
    public Transform patrolPoint;

    private NavMeshAgent ai;
    private Animator anim;
    private float distanceToTarget;
    private Coroutine idleToWalk;

    public enum EnemyState
    {
        Idle,
        Walk,
        Run,
        Attack
    }

    public EnemyState enemyState;
    void Start()
    {
        enemyState = EnemyState.Idle;
        ai = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        distanceToTarget = Mathf.Abs(Vector3.Distance(target.position, transform.position));
    }

    private IEnumerator SwitchToWalk()
    {
        yield return new WaitForSeconds(5);
        enemyState = EnemyState.Walk;
        idleToWalk = null;
    }

    private void SwitchState(int newState)
    {
        if (anim.GetInteger("State") != newState)
        {
            anim.SetInteger("State", newState);
        }
    }

    void Update()
    {
        distanceToTarget = Mathf.Abs(Vector3.Distance(target.position, transform.position));

        switch (enemyState)
        {
            case EnemyState.Idle:
                SwitchState(0);
                ai.SetDestination(transform.position);
                if (idleToWalk == null)
                {
                    idleToWalk = StartCoroutine(SwitchToWalk());
                }
                break;

            case EnemyState.Walk:
                float distanceToPatrolPoint = Mathf.Abs(Vector3.Distance(patrolPoint.position, transform.position));
                if (distanceToPatrolPoint > 2)
                {
                    SwitchState(1);
                    ai.SetDestination(patrolPoint.position);
                }
                else
                {
                    SwitchState(0);
                }

                if (distanceToTarget <= 15)
                {
                    enemyState = EnemyState.Run;
                }
                break;

            case EnemyState.Run:
                SwitchState(2);
                ai.SetDestination(target.position);

                if (distanceToTarget <= 5)
                {
                    enemyState = EnemyState.Attack;
                }
                else if (distanceToTarget > 15)
                {
                    enemyState = EnemyState.Idle;
                }
                break;

            case EnemyState.Attack:
                SwitchState(3);
                if (distanceToTarget > 5 && distanceToTarget <= 15)
                {
                    enemyState = EnemyState.Run;
                }
                else if (distanceToTarget > 15)
                {
                    enemyState = EnemyState.Idle;
                }
                break;

            default:
                break;

        }
    }
}