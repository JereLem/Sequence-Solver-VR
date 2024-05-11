using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VrGameManager : MonoBehaviour
{
    public GameObject[] miniGamePrefabs; // Array to hold mini-game prefabs
    public Transform worldCenter; // Center of the cylindrical world
    public float worldRadius = 10f; // Radius of the cylindrical world
    public float gameTime = 300f; // Total duration of the game in seconds
    public AudioClip audioCue; // Audio cue for indicating mini-game locations

    private float gameTimer = 0f;
    private float minigameTimer = 0f;
    private GameObject currentMiniGame;

    // Timer for the game
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI solvedText;

    public GameStats gameStats; // Access Gamestats
    float elapsedTime;
    [SerializeField] public GameObject playerUI;

    private void Start()
    {
        elapsedTime = gameTime;
        StartCoroutine(SpawnMiniGames());

    }

    private void Update()
    {
        elapsedTime -= Time.deltaTime;
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        solvedText.text = string.Format("Solved:{0}", gameStats.solvedMinigames);

        if (elapsedTime <= 0)
        {
            Time.timeScale = 0f;
            timerText.text = "00:00";
            playerUI.SetActive(true);
            gameStats.gameover = true;
            
        }
    }

    private IEnumerator SpawnMiniGames()
    {
        while (gameTimer < gameTime)
        {
            if (currentMiniGame == null) // Check if the previous mini-game is solved or destroyed
            {
                // Calculate random position on the cylindrical world
                Vector3 randomPos = Random.onUnitSphere * worldRadius;
                randomPos.y = 1.2f; // Ensure mini-games spawn at ground level

                // Randomly select a mini-game prefab
                GameObject randomMiniGamePrefab = miniGamePrefabs[Random.Range(0, miniGamePrefabs.Length)];

                // Instantiate the mini-game prefab at the random position
                currentMiniGame = Instantiate(randomMiniGamePrefab, randomPos, Quaternion.identity);

                // Play audio cue indicating the location of the mini-game
                AudioSource.PlayClipAtPoint(audioCue, randomPos);
            }

            // Wait for the specified interval before checking again
            yield return new WaitForSeconds(1f);

            // Update timers
            gameTimer += 1f;
            minigameTimer += 1f;
        }
    }

    public void MiniGameCompleted()
    {
        // Called when a mini-game is completed
        currentMiniGame = null; // Reset currentMiniGame to allow spawning the next mini-game
    }
}
