using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Game : MonoBehaviour
{
    public int lives, score, highscore;
    [SerializeField] private Player player;
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private Spawner[] spawners;
    [SerializeField] private int level;

    [SerializeField] private TextMeshProUGUI scoreText, livesText, bestText;

    private void Start()
    {
        highscore = PlayerPrefs.GetInt("Highscore", 0);
        Load();
        bestText.text = "Best: " + highscore;
        UpdateHUD();
    }

    public void LoseLife()
    {
        // Respawn if player still has lives left
        if (lives > 0)
            StartCoroutine(Respawn());
        // End game if player has no lives
        else
            EndGame();
    }

    // Logic for respawning when player dies
    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(2f); // Wait two seconds before respawn
        lives--; // Subtract 1 from player lives
        Instantiate(player.gameObject, playerSpawnPoint.position, Quaternion.identity); // Spawn the player at the player spawn point
        UpdateHUD();
    }

    public void AddPoints(int points)
    {
        score += points;
        UpdateHUD();
        // is the level complete?
        CheckForLevelCompletion();
    }

    public void AddLife()
    {
        lives++;
        UpdateHUD();
    }

    private void CheckForLevelCompletion()
    {
        if (!FindObjectOfType<Enemy>())
        {
            foreach (Spawner spawner in spawners)
                if (!spawner.completed)
                    return;
            CompleteLevel();
        }
    }

    private void CompleteLevel()
    {
        level++;
        Save();
        if (level <= SceneManager.sceneCountInBuildSettings-1)
        {
            SceneManager.LoadScene(level);
        }
        else
        {
            Debug.Log("You beat the game! Congratulations!");
            EndGame();
        }
    }

    private void Save()
    {
        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.SetInt("Lives", lives);
        PlayerPrefs.SetInt("Level", level);
    }

    private void Load()
    {
        score = PlayerPrefs.GetInt("Score", 0);
        lives = PlayerPrefs.GetInt("Lives", 3);
        level = PlayerPrefs.GetInt("Level", 0);
    }

    private void StartNewGame()
    {
        level = 0;
        SceneManager.LoadScene(level);
        PlayerPrefs.DeleteKey("Score");
        PlayerPrefs.DeleteKey("Lives");
        PlayerPrefs.DeleteKey("Level");
    }

    private void EndGame()
    {
        if (score > highscore)
            PlayerPrefs.SetInt("HighScore", score);
        StartNewGame();
    }

    private void UpdateHUD()
    {
        scoreText.text = "Score: " + score;
        livesText.text = "Lives: " + lives;
    }
}
