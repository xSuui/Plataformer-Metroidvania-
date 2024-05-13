using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public int score;
    public Text scoreText;

    public GameObject gameOverPanel;

    public static GameController instance;

    private void Awake()
    {
        instance = this;

        Time.timeScale = 1;

        if (PlayerPrefs.GetInt("score") > 0)
        {
            score = PlayerPrefs.GetInt("score");
            scoreText.text = "x " + score.ToString();
        }
    }

    public void GetCoin()
    {
        score++;
        scoreText.text = "x " + score.ToString();

        PlayerPrefs.SetInt("score", score);
    }

    public void NextLvl()
    {
        SceneManager.LoadScene(1);
    }

    public void ShowGameOver()
    {
        Time.timeScale = 0;
        gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
