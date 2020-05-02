using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneController : MonoSingleton <SceneController>
{
    public Image fader;
    private static SceneController instance;

    void Awake()
    {
        
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            fader.rectTransform.sizeDelta = new Vector2(Screen.width * 20, Screen.height * 20);
            fader.gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }

   
    public static void LoadScene(int index, float fadeDuration = 1, float waitTime = 0)
    {
        instance.StartCoroutine(instance.FadeScene(index, fadeDuration, waitTime));
    }

    private IEnumerator FadeScene(int index, float fadeDuration, float waitTime)
    {
        fader.gameObject.SetActive(true);

        for (float t = 0; t < 1; t+= Time.deltaTime / fadeDuration)
        {
            fader.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, t));
            yield return null;
        }

        SceneManager.LoadScene(index);

        yield return new WaitForSeconds(waitTime);


        for (float t = 0; t < 1; t += Time.deltaTime / fadeDuration)
        {
            fader.color = new Color(0, 0, 0, Mathf.Lerp(1, 0, t));
            yield return null;
        }

        fader.gameObject.SetActive(false);
    }

}
