using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public int score;
    public static GameController instance;


    private void Awake()
    {
        // Garantir que haja apenas uma instância do GameController.
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Time.timeScale = 1;

        // Verificar se há um EventSystem na cena e criar um se necessário
        if (FindObjectOfType<EventSystem>() == null)
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
        }

        // Carregar pontuação salva
        if (PlayerPrefs.GetInt("score") > 0)
        {
            score = PlayerPrefs.GetInt("score");
            Player.instance.scoreText.text = "x " + score.ToString();
        }
    }


    /*private void Start()
    {
        Time.timeScale = 1;

        // Verificar e destruir EventSystems duplicados ao carregar a cena
        EventSystem[] eventSystems = FindObjectsOfType<EventSystem>();
        Debug.Log("EventSystems encontrados: " + eventSystems.Length);

        // Remover EventSystems duplicados
        if (eventSystems.Length > 1)
        {
            for (int i = 1; i < eventSystems.Length; i++)
            {
                Destroy(eventSystems[i].gameObject);
            }
        }

        // Se não houver EventSystem, crie um novo
        if (eventSystems.Length == 0)
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
        }

        // Carregar o score salvo
        if (PlayerPrefs.GetInt("score") > 0)
        {
            score = PlayerPrefs.GetInt("score");
            Player.instance.scoreText.text = "x " + score.ToString();
        }

    }*/

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

        // Parar todo o áudio do jogo ao morrer
        AudioListener.pause = true;
    }

    /*public void RestartGame()
    {
        Debug.Log("RestartGame chamado"); // Verifique se o botão está chamando este método
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Recarregar a cena atual
    }*/

    public void RestartGame()
    {
        Debug.Log("RestartGame chamado");

        // Resetando o tempo para 1 antes de recarregar a cena
        Time.timeScale = 1;

        // Se houver um GameController persistente, destruir antes de recarregar a cena
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }

        // Reativar o áudio do jogo
        AudioListener.pause = false;

        // Recarregar a cena
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        // Aguarde um frame antes de recriar o EventSystem (evita conflitos)
        StartCoroutine(CreateEventSystem());
    }

    private IEnumerator CreateEventSystem()
    {
        yield return null; // Esperar um frame

        if (FindObjectOfType<EventSystem>() == null)
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
        }
    }

}




/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour
{
    public int score;

    public static GameController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Time.timeScale = 1;

        // Verifica e destrói EventSystems duplicados antes de criar um novo
        EventSystem[] eventSystems = FindObjectsOfType<EventSystem>();
        if (eventSystems.Length > 1)
        {
            for (int i = 1; i < eventSystems.Length; i++)
            {
                Destroy(eventSystems[i].gameObject);
            }
        }
        else if (eventSystems.Length == 0)
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
        }

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
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}/*



/*using System.Collections;
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
    }*

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
}*/
