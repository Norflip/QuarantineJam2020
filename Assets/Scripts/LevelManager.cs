using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelManager : MonoSingleton<LevelManager>
{
    public const float START_DELAY = 3.0f;
    public const float GAME_TIME = 60.0f * 5;

    public TextMeshProUGUI timer;
    public TextMeshProUGUI score;

    float startTime;
    float endTime;
    bool started = false;

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

    private void Update()
    {
        if (started)
        {
            score.text = CollectionManager.Instance.sum.ToString() + " $";
            float timeRemaining = Mathf.Floor(Mathf.Max(endTime - Time.time, 0));
            timer.text = "t - " + timeRemaining.ToString();
        }
    }
}
