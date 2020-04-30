using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO Kolla så den passar alla tests

public class TestSound : MonoBehaviour
{
    public string key = "TEMP";

    [NaughtyAttributes.Button]
    void Run ()
    {
        if (Application.isPlaying)
            AudioManager.Instance.PlayEffect(key);
    }

    [NaughtyAttributes.Button]
    void Run3D ()
    {
        if (Application.isPlaying)
            AudioManager.Instance.PlayEffect(key, transform.position);
    }

    [NaughtyAttributes.Button]
    void RunAsGroup ()
    {
        if (Application.isPlaying)
            AudioManager.Instance.PlayGroup(key, transform.position);
    }

    [NaughtyAttributes.Button]
    void StopAll ()
    {
        if(Application.isPlaying)
        {
            AudioSource[] sources = GameObject.FindObjectsOfType<AudioSource>();
            for (int i = 0; i < sources.Length; i++)
                sources[i].Stop();
        }
    }
}
