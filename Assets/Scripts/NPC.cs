using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
public class NPC : MonoBehaviour
{
    public Transform[] goals;

    // TJS: keeps track of which goal is the next target
    private int goalIndex = 0;

    private NavMeshAgent m_agent;
    private Rigidbody m_rigidbody;

    private void Start()
    {
        Initialize();
        StartCoroutine(NavigateToGoalTarget());
    }

    private void Initialize()
    {
        m_agent = GetComponent<NavMeshAgent>();
        m_rigidbody = GetComponent<Rigidbody>();
    }

    private void SetAgentDestinationGoal(Transform goalTransform)
    {
        m_agent.destination = goalTransform.position;
    }

    private IEnumerator NavigateToGoalTarget()
    {
        if (goals == null || goals.Length <= 0)
        {
            Debug.LogError($"<color=white>{nameof(NPC)}</color>: Array <color=red>{nameof(goals)}</color> is null or does not have any values. Please assign a Transform value in the inspector.");
            yield break;
        }

        while (true)
        {
            yield return null;
            SetAgentDestinationGoal(goals[goalIndex]);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Waypoint")
        {
            if (other.transform == goals[goalIndex])
            {
                IncrementGoalIndex();
            }
        }
    }

    private void IncrementGoalIndex()
    {
        goalIndex = (goalIndex >= goals.Length - 1) ? 0 : ++goalIndex;
        Debug.Log($"{nameof(goalIndex)}: {goalIndex}");
    }
}
