using UnityEngine;

public class Button : MonoBehaviour
{
    
    public void LoadScene(int i)
    {
        if (Time.timeScale == 0f)
            Time.timeScale = 1f;
        SceneController.LoadScene(i, 1, 2);
    }

    public void QuitGame()
    {
        Debug.Log("QUITTING AWESOME GAME!");
        Application.Quit();
    }

}
