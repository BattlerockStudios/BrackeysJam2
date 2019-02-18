using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{   

    #region Private Constants

    private const int MAX_MATCH_COUNT = 2;

    #endregion

    #region Private Variables

    private int m_matchCount = 0;

    private bool m_readyToStart = false;

    #endregion

    #region Public Properties

    public bool IsMatchComplete
    {
        get { return m_matchCount == MAX_MATCH_COUNT; }
    }

    public int MatchCount
    {
        get => m_matchCount;

        set
        {
            if(value >= MAX_MATCH_COUNT)
            {
                value = MAX_MATCH_COUNT;
                blindfoldImage.gameObject.SetActive(false);
                m_readyToStart = false;
            }
            m_matchCount = value;

            Debug.Log($"<color=white>{nameof(GameManager)}</color>: <color=red>{nameof(m_matchCount)}</color>: {m_matchCount}");
        }
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

    #region Unity Methods

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(WaitForInputToConfirmPlayerIsReady());
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
