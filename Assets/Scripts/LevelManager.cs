using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using TMPro;
using UnityEngine.SceneManagement;

public class LevelManager : MonoSingleton<LevelManager>
{
    public const float START_DELAY = 3.0f;
    public const float GAME_TIME = 5.0f;//60.0f * 5; // 10.0f;

    public TextMeshProUGUI timer;
    public TextMeshProUGUI score;
    public TextMeshProUGUI totalScore;

    float startTime;
    float endTime;
    float timeRemaining;
    bool started = false;
    bool freeroam = false;

    // game over
    public GameObject gameoverUI;

    void Start()
    {
        StartGame();
    }

    void StartGame()
    {
        startTime = Time.time;
        endTime = Time.time + GAME_TIME;
        started = true;
    }

    public void Freeroam()
    {
        freeroam = true;

        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gameoverUI.SetActive(false);
    }

    private void Update()
    {
        if (started)
        {
            score.text = CollectionManager.Instance.PointSum.ToString() + " $";

            if (!freeroam)
            {
                float timeRemaining = Mathf.Floor(Mathf.Max(endTime - Time.time, 0));
                timer.text = "t - " + timeRemaining.ToString();
            }
            else
            {
                timer.text = "";
            }
        }

        if(!freeroam && Mathf.Max(endTime - Time.time, 0) <= 0)
        {
            Gameover();
        }       


    }

    void Gameover()
    {
        totalScore.text = "You got " + score.text;
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gameoverUI.SetActive(true);
    }

    public void GoMainMenu ()
    {
        SceneManager.LoadScene(0);
    }
    
    public void Retry()
    {        
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gameoverUI.SetActive(false);
        SceneManager.LoadScene(1);
       // SceneController.LoadScene(i, 1, 2);
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitApp ()
    {
        Application.Quit();
    }
}
