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
        WeightController.samWeight = 0f;
        WeightController.lockerWeight = 0f;
    }

}
