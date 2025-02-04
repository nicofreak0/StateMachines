using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    public Transform patrolPoint;
    public enum EnemyState { Idle, Patrol, Chase, Attack }
    public EnemyState enemyState;

    public Transform target; // Public variable to store the target's transform
    private NavMeshAgent ai; // Private variable to store the NavMeshAgent component
    private Animator anim;
    private float distanceToTarget;
    private Coroutine idleToPatrol;


    void Start()
    {
        enemyState = EnemyState.Idle;

        ai = GetComponent<NavMeshAgent>();
        if (ai == null)
        {
            Debug.LogError("NavMeshAgent component not found on this GameObject!");
            enabled = false;
            return;
        }

        anim = GetComponent<Animator>();
        if (anim == null) { Debug.LogError("Animator component not found!"); enabled = false; return; }


        if (target == null)
        {
            Debug.LogError("Target Transform not assigned in the Inspector!");
            enabled = false;
            return;
        }

        distanceToTarget = Mathf.Abs(Vector3.Distance(target.position, transform.position));
    }

    private IEnumerator SwitchToPatrol()
    {
        yield return new WaitForSeconds(5f);
        enemyState = EnemyState.Patrol;
        idleToPatrol = null;
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

                if (idleToPatrol == null)
                {
                    idleToPatrol = StartCoroutine(SwitchToPatrol());
                }
                break;

            case EnemyState.Patrol:
                float distanceToPatrolPoint = Mathf.Abs(Vector3.Distance(patrolPoint.position, transform.position));

                if (distanceToPatrolPoint > 2f)
                {
                    SwitchState(1);
                    ai.SetDestination(patrolPoint.position);
                }
                else
                {
                    SwitchState(0);
                }

                if (distanceToTarget <= 15f)
                {
                    enemyState = EnemyState.Chase;
                }
                break;

            case EnemyState.Chase:
                SwitchState(2);
                // Original movement code moved here:
                if (ai != null && target != null)
                {
                    ai.SetDestination(target.position);
                }

                if (distanceToTarget <= 5f)
                {
                    enemyState = EnemyState.Attack;
                }
                else if (distanceToTarget > 15f)
                {
                    enemyState = EnemyState.Idle;
                }
                break;

            case EnemyState.Attack:
                SwitchState(3);
                // Implement your attack logic here

                if (distanceToTarget > 5f && distanceToTarget <= 15f)
                {
                    enemyState = EnemyState.Chase;
                }
                else if (distanceToTarget > 15f)
                {
                    enemyState = EnemyState.Idle;
                }
                break;

            default:
                break;
        }
    }
}