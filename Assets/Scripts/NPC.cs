using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NPC : MonoBehaviour, IArrowTarget
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

    public AudioSource sfxSource;

    public AudioClip hitClip;

    #endregion

    #region Private Variables

    [SerializeField]
    private State m_state = State.Idle;

    [SerializeField]
    private float m_maxIdleTime = 5.0f;

    // TJS: keeps track of which goal is the next target
    private int goalIndex = 0;

    private NavMeshAgent m_agent;
    private NPC m_loveInterest;

    public State CurrentState
    {
        get => m_state;
        set => m_state = value;
    }

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
            if (CurrentState == State.LoveStruck)
            {
                return;
            }

            if (other.transform == goals[goalIndex])
            {
                IncrementGoalIndex();
                CurrentState = (State)UnityEngine.Random.Range((int)State.Idle, (int)State.Moving + 1);
            }
        }
    }

    public void FallInLoveWith(NPC otherNPC)
    {
        m_loveInterest = otherNPC;
        CurrentState = NPC.State.LoveStruck;
        heartParticles.gameObject.SetActive(true);
    }

    #endregion

    #region Private Methods

    private void Initialize()
    {
        m_agent = GetComponent<NavMeshAgent>();

        if (heartParticles == null)
        {
            Debug.LogError($"<color=white>{nameof(NPC)}</color>: Transform: {nameof(heartParticles)} is null. Disabling this monobehaviour.");
            this.enabled = false;
        }
    }

    private void SetAgentDestinationGoal(Transform goalTransform)
    {
        NavMesh.SamplePosition(goalTransform.position, out NavMeshHit hit, float.MaxValue, NavMesh.AllAreas);
        m_agent.destination = hit.position;
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
            switch (CurrentState)
            {
                case State.Moving:
                    SetAgentDestinationGoal(goals[goalIndex]);
                    break;
                case State.Idle:
                    yield return new WaitForSeconds(UnityEngine.Random.Range(0, m_maxIdleTime));
                    CurrentState = State.Moving;
                    break;
                case State.LoveStruck:
                    SetAgentDestinationGoal(m_loveInterest.transform);
                    break;
                default:
                    yield return new WaitForSeconds(UnityEngine.Random.Range(0, m_maxIdleTime));
                    CurrentState = State.Moving;
                    break;
            }
            yield return null;
        }
    }

    private void IncrementGoalIndex()
    {
        goalIndex = (goalIndex >= goals.Length - 1) ? 0 : ++goalIndex;
        Debug.Log($"{nameof(goalIndex)}: {goalIndex}");
    }

    void IArrowTarget.OnHitByArrow(Arrow arrow)
    {
        if (CurrentState != State.LoveStruck)
        {
            GameManager.Instance.SetHitTarget(this);
            sfxSource.PlayOneShot(hitClip);
        }
    }

    #endregion
}
