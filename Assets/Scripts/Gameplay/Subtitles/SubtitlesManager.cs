using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;
using System.Linq;

public class SubtitlesManager : MonoSingleton<SubtitlesManager>
{

    public SubtitlesClip clip;
    public TextMeshProUGUI displayText;

    Coroutine coroutine;

    private void Awake()
    {
        Play(clip);
    }

    public void Play (SubtitlesClip clip)
    {
        coroutine = StartCoroutine(RunSubtitles(clip));
    }

    IEnumerator RunSubtitles (SubtitlesClip clip)
    {
        yield return new WaitForSeconds(clip.leftOffset);

        for (int i = 0; i < clip.rows.Count; i++)
        {
            yield return new WaitForSeconds(clip.rows[i].leftOffset);
            string str = clip.rows[i].textLine;

            if(clip.rows[i].showCompleteRow)
            {
                displayText.text = str;
                yield return new WaitForSeconds(clip.rows[i].playTime);
            }
            else
            {
                int startChar = str.IndexOf(">");
                int endChar = str.LastIndexOf("<");

                if (startChar == -1)
                    startChar = 1;

                if (endChar == -1)
                    endChar = str.Length;

                float timePerChar = clip.rows[i].playTime / (endChar - startChar);

                for (int j = startChar; j <= endChar; j++)
                {
                    displayText.text = str.Substring(0, j);
                    yield return new WaitForSeconds(timePerChar);
                }
            }

            yield return new WaitForSeconds(clip.rows[i].rightOffset);
        }

        displayText.text = "";
        yield return new WaitForSeconds(clip.rightOffset);

    }
}
