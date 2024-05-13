using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Dan.Main;
using UnityEngine.Events;
using System.Threading;

public class GameStats : MonoBehaviour
{
    public int solvedMinigames;
    public bool gameover;
    public string playername;

    // Leaderboard variables
    [SerializeField]
    private List<TextMeshProUGUI> names;
    [SerializeField]
    private List<TextMeshProUGUI> scores;

    [SerializeField]
    private TMP_InputField inputName;

    private string publicLeaderboardKey =
        "0b73bfbf73558548a81cb7c0c5e396ddcd1df11397e369d61f08ac38208580b6";


    // Start is called before the first frame update
    void Start()
    {
        solvedMinigames = 0;
        playername = "";
        gameover = false;
        GetLeaderboard();
    }

    public UnityEvent<string, int> submitScoreEvent;

    public void SubmitScore()
    { 
        submitScoreEvent.Invoke(playername, solvedMinigames);
    }

    // Get leaderboard
    public void GetLeaderboard()
    {
        LeaderboardCreator.GetLeaderboard(publicLeaderboardKey, ((data) =>
        {
            int loopLength = (data.Length < names.Count) ? data.Length : names.Count;
            for (int i = 0; i < loopLength; i++)
            {
                names[i].text = data[i].Username;
                scores[i].text = data[i].Score.ToString();
            }
        }));
    }

    // Set an entry to the leaderboard
    public void SetLeaderboardEntry(string username, int score)
    {
        LeaderboardCreator.UploadNewEntry(publicLeaderboardKey, username, score,
            ((data) =>
            {
                username.Substring(0, 4);
                GetLeaderboard();
            }));
    }
}
