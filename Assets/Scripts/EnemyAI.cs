using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform target; // Public variable to store the target's transform
    private NavMeshAgent ai; // Private variable to store the NavMeshAgent component

    void Start()
    {
        // Get the NavMeshAgent component attached to this GameObject
        ai = GetComponent<NavMeshAgent>();

        // Check if the NavMeshAgent component exists.  Important for error handling!
        if (ai == null)
        {
            Debug.LogError("NavMeshAgent component not found on this GameObject!");
            enabled = false; // Disable this script to prevent further errors.  Alternative: Destroy(this);
            return; // Exit Start() early
        }

        // Check if the target is assigned.  Also important for error handling!
        if (target == null)
        {
            Debug.LogError("Target Transform not assigned in the Inspector!");
            enabled = false; // Or Destroy(this);
            return; // Exit Start()
        }
    }

    void Update()
    {
        // Only proceed if both ai and target are valid.  This prevents NullReferenceExceptions
        if (ai != null && target != null)
        {
            // Set the destination of the NavMeshAgent to the target's position
            ai.SetDestination(target.position);
        }
    }
}