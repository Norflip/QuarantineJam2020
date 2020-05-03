using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Audio Library")]
public class AudioLibrary : ScriptableObject
{
    [System.Serializable]
    public struct AudioLibraryData
    {
        [NaughtyAttributes.ReorderableList]
        public List<Sound> sounds;

        [NaughtyAttributes.ReorderableList]
        public List<SoundGroup> groups;
    }

    public AudioLibraryData data;
}
