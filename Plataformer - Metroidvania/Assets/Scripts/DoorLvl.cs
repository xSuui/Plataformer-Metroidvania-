using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorLvl : MonoBehaviour
{
    public int lvlIndex;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("player"))
        {
            SceneManager.LoadScene(lvlIndex);
        }
    }
}