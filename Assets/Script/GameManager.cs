using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    private void Awake()
    {
        if (instance!= null & instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void RestartScene()
    {
       SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        BoxController.samWeight = 0f;
        BoxController.lockerWeight = 0f;
    }

}
