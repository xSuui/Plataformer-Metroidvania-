using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public int score;
    public Text scoreText;

    public static GameController instance;

    private void Awake()
    {
        instance = this;

        if(PlayerPrefs.GetInt("score") > 0)
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
}
