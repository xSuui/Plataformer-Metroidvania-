using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public int score;

    public static GameController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this; // Corrigido! Agora 'instance' recebe a referência correta.
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject); // Agora ele destrói apenas este objeto, não a instância existente.
        }
    }


    /*private void Awake()
    {
        //instance = this;

        if (instance == null)
        {
            instance = null;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(instance.gameObject);
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }*/

    private void Start()
    {
        Time.timeScale = 1;

        if (PlayerPrefs.GetInt("score") > 0)
        {
            score = PlayerPrefs.GetInt("score");
            Player.instance.scoreText.text = "x " + score.ToString();
        }
    }

    public void GetCoin()
    {
        score++;
        Player.instance.scoreText.text = "x " + score.ToString();

        PlayerPrefs.SetInt("score", score);
    }


    public void ShowGameOver()
    {
        Time.timeScale = 0;
        Player.instance.gameOver.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
