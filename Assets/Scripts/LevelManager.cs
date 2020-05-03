using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelManager : MonoSingleton<LevelManager>
{
    public const float START_DELAY = 3.0f;
    public const float GAME_TIME = 60.0f * 5; // 10.0f;

    public TextMeshProUGUI timer;
    public TextMeshProUGUI score;

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

        if(Mathf.Max(endTime - Time.time, 0) <= 0)
        {
            Gameover();
        }
    }

    void Gameover()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gameoverUI.SetActive(true);
    }

    void Retry(int i)
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneController.LoadScene(i, 1, 2);
    }

}
