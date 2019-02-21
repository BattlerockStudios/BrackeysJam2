using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    #region Private Constants

    private const int MAX_COUPLE_COUNT = 2;

    #endregion

    #region Private Variables

    [SerializeField]
    private AudioClip m_loveClip = null;

    [SerializeField]
    private AudioSource m_audioSource = null;

    [SerializeField]
    private AudioClip m_victoryClip = null;

    [SerializeField]
    private TMPro.TextMeshProUGUI m_gameData = null;

    private bool m_readyToStart = false;
    private readonly List<NPC> m_hitTargets = new List<NPC>();
    private readonly List<Tuple<NPC, NPC>> m_couples = new List<Tuple<NPC, NPC>>();

    #endregion

    #region Public Properties

    public bool IsMatchComplete
    {
        get { return m_couples.Count == MAX_COUPLE_COUNT; }
    }

    public bool ReadyToStart { get => m_readyToStart; set => m_readyToStart = value; }

    #endregion

    #region Public Variables

    // Ehhh...it's a game jam
    public static GameManager Instance;

    // This can be purely a black image texture
    // or we can use something else to limit the player's view
    public Image blindfoldImage;

    #endregion

    #region Public Methods

    public void SetBlindfoldOpacity(float opacity)
    {
        blindfoldImage.color = Color.Lerp(Color.black, Color.clear, opacity);
    }

    public void SetHitTarget(NPC npc)
    {
        if(m_hitTargets.Contains(npc))
        {
            return;
        }

        m_hitTargets.Add(npc);
        if (m_hitTargets.Count == 2)
        {
            m_hitTargets[0].FallInLoveWith(m_hitTargets[1]);
            m_hitTargets[1].FallInLoveWith(m_hitTargets[0]);

            m_couples.Add(new Tuple<NPC, NPC>(m_hitTargets[0], m_hitTargets[1]));
            m_audioSource.PlayOneShot(m_loveClip);

            m_hitTargets.Clear();
        }

        if(IsMatchComplete)
        {
            blindfoldImage.gameObject.SetActive(false);
            m_readyToStart = false;

            m_audioSource.PlayOneShot(m_victoryClip);
        }

        UpdateGameText();
    }

    #endregion

    #region Unity Methods

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateGameText();
        StartCoroutine(WaitForInputToConfirmPlayerIsReady());
    }

    private void UpdateGameText()
    {
        m_gameData.text = $"Oh no! Cupid is blind! Click the mouse to fire arrows. {m_couples.Count} out of {MAX_COUPLE_COUNT} matches made.";
    }

    #endregion

    #region Private Methods

    private IEnumerator WaitForInputToConfirmPlayerIsReady()
    {
        while (Input.anyKey == false)
        {
            yield return null;
        }

        m_readyToStart = true;

        blindfoldImage.gameObject.SetActive(true);
    }

    #endregion
}
