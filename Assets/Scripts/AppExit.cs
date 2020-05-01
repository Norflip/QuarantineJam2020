using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppExit : MonoBehaviour
{
    public KeyCode exitKey = KeyCode.Escape;

    void Update()
    {
        if(Input.GetKey(exitKey))
        {
#if UNITY_EDITOR
            Debug.Break();
#elif UNITY_STANDALONE_WIN
            Application.Quit();
#endif
        }
    }
}
