using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour {

    public GameObject endUI;
    public Text endMessage;

    public static GameManager Instance;
    private EnemySpawner enemySpawner;
    bool isPaused = false;


    void Awake()
    {
        Instance = this;
        enemySpawner = GetComponent<EnemySpawner>();
    }

    public void Win()
    {
        GameObject.Find("Canvas/Wave").GetComponent<Text>().text = "";
        endUI.SetActive(true);
        endMessage.text = "胜 利";
    }
    public void Failed()
    {
        GameObject.Find("Canvas/Wave").GetComponent<Text>().text = "";
        enemySpawner.Stop();
        endUI.SetActive(true);
        endMessage.text = "失 败";
    }

    public void OnButtonRetry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex );
    }
    public void OnButtonMenu()
    {
        SceneManager.LoadScene(0);
    }


    //---------------------------------------------------------曾颉 add   PauseGame-----------------------------------------------------------------

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
           
            //打印isPaused
            Debug.Log(isPaused);
            
            if (isPaused)
            {
                // Pause the game
                endUI.SetActive(true);
                endMessage.text = "暂 停";
                Time.timeScale = 0;
                endUI.GetComponent<Animator>().updateMode = AnimatorUpdateMode.UnscaledTime;
            }
            else
            {
                // Unpause the game
                Time.timeScale = 1;
                endUI.SetActive(false);
                endUI.GetComponent<Animator>().updateMode = AnimatorUpdateMode.Normal;
            }
        }
    }


    //---------------------------------------------------------曾颉 add   PauseGame-----------------------------------------------------------------




}
