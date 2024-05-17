using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using Dan.Main;
using UnityEngine.Events;
using System.Threading;
using Microsoft.MixedReality.Toolkit.Experimental.UI;

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
        "4006de3f3a3c1d270b644cbf679d2b0e0bccd685b2043a18ab3df79070186155";


    // Start is called before the first frame update
    void Start()
    {
        solvedMinigames = 0;
        playername = "Player";
        gameover = false;
        GetLeaderboard();
    }

    void Update()
    {
        if (gameover)
        {
            CheckLeaderboard();
        }
    }

    public UnityEvent<string, int> submitScoreEvent;



    public void CheckLeaderboard()
    {
        bool playerScoreIsBetter = false; // Assume the player's score is better until proven otherwise

        // Iterate over the scores from the leaderboard
        for (int i = 0; i < scores.Count; i++)
        {
            int leaderboardScore;
            if (int.TryParse(scores[i].text, out leaderboardScore))
            {
                // Compare the player's score with the current leaderboard score
                if (solvedMinigames > leaderboardScore)
                {
                    playerScoreIsBetter = true; // Player's score is not better than this leaderboard score
                    break; // Exit the loop early if the player's score is not better
                }
            }
            else
            {
                Debug.LogError("Invalid score format in leaderboard."); // Handle invalid score format
            }
        }

        // If player's score is better than any score on the leaderboard, prompt for name input
        if (playerScoreIsBetter)
        {
            inputName.gameObject.SetActive(true);
            
            // Now you can access the NonNativeKeyboard class and its static Instance property
            NonNativeKeyboard keyboardInstance = NonNativeKeyboard.Instance;

            // Example of calling a method on the NonNativeKeyboard instance
            keyboardInstance.PresentKeyboard();

            // Rewrite default name to the input field name
            playername = inputName.text;
        }
    }


    public void SubmitScore()
    { 
        Debug.Log(playername);
        Debug.Log(solvedMinigames);
        submitScoreEvent.Invoke(playername, solvedMinigames);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        Time.timeScale = 1f;
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
