using UnityEngine;

public class Button : MonoBehaviour
{
    public void LoadScene(int i)
    {
        // fade out in 1 sec, wait 2 sec, fade in - in 1 sec
        
        SceneController.LoadScene(i, 1, 2);
    }

    public void QuitGame()
    {
        Debug.Log("QUITTING AWESOME GAME!");
        Application.Quit();
    }

}
