using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Audio Library")]
public class AudioLibrary : ScriptableObject
{
    [System.Serializable]
    public struct AudioLibraryData
    {
        public Sound[] sounds;
        public SoundGroup[] groups;
    }

    public AudioLibraryData data;
}
