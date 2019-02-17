using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
public class NPC : MonoBehaviour
{
    public enum State
    {
        Idle,
        Moving,
        LoveStruck
    }

    #region Public Variables

    public Transform[] goals;

    public Transform heartParticles;

    #endregion

    #region Private Variables

    [SerializeField]
    private State m_state = State.Idle;

    [SerializeField]
    private float m_maxIdleTime = 5.0f;

    // TJS: keeps track of which goal is the next target
    private int goalIndex = 0;

    private NavMeshAgent m_agent;
    private Rigidbody m_rigidbody;

    public State CurrentState { get => m_state; set => m_state = value; }

    #endregion

    #region Unity Methods

    private void Start()
    {
        Initialize();
        StartCoroutine(NavigateToGoalTarget());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Waypoint")
        {
            if (m_state == State.LoveStruck)
            {
                return;
            }

            if (other.transform == goals[goalIndex])
            {
                IncrementGoalIndex();
                m_state = (State)Random.Range((int)State.Idle, (int)State.Moving + 1);
            }
        }
    }

    #endregion

    #region Private Methods

    private void Initialize()
    {
        m_agent = GetComponent<NavMeshAgent>();
        m_rigidbody = GetComponent<Rigidbody>();

        if (heartParticles == null)
        {
            Debug.LogError($"<color=white>{nameof(NPC)}</color>: Transform: {nameof(heartParticles)} is null. Disabling this monobehaviour.");
            this.enabled = false;
        }
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
            switch (m_state)
            {
                case State.Moving:
                    SetAgentDestinationGoal(goals[goalIndex]);
                    break;
                case State.Idle:
                    yield return new WaitForSeconds(Random.Range(0, m_maxIdleTime));
                    m_state = State.Moving;
                    break;
                case State.LoveStruck:
                    heartParticles.gameObject.SetActive(true);
                    yield break;
                default:
                    yield return new WaitForSeconds(Random.Range(0, m_maxIdleTime));
                    m_state = State.Moving;
                    break;
            }
        }
    }

    private void IncrementGoalIndex()
    {
        goalIndex = (goalIndex >= goals.Length - 1) ? 0 : ++goalIndex;
        Debug.Log($"{nameof(goalIndex)}: {goalIndex}");
    }

    #endregion
}
