using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausMenuScript : MonoBehaviour
{

    public static bool isPaused = false;
    public GameObject pausmenuUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            isPaused = !isPaused;

            if (isPaused)
            {
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                pausmenuUI.SetActive(true);
            }
            else
            {
                Time.timeScale = 1f;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                pausmenuUI.SetActive(false);
            }            
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pausmenuUI.SetActive(false);
        isPaused = false;
    }
    
    public void LoadMenu(int i)
    {        
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pausmenuUI.SetActive(false);
        isPaused = false;
        SceneController.LoadScene(i, 1, 2);
    }



}
