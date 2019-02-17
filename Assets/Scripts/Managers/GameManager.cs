using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Ehhh...it's a game jam
    public static GameManager Instance;

    private const int MAX_MATCH_COUNT = 2;

    public bool IsMatchComplete
    {
        get { return matchCount == MAX_MATCH_COUNT; }
    }

    public int MatchCount
    {
        set
        {
            if(matchCount > MAX_MATCH_COUNT)
            {
                matchCount = MAX_MATCH_COUNT;
            }
            value = matchCount;
        }
    }

    public int matchCount = 0;

    private void Awake()
    {
        Instance = this;
    }
}
