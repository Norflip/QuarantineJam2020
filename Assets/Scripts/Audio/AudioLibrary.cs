using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Audio Library")]
public class AudioLibrary : ScriptableObject
{
    [NaughtyAttributes.ReorderableList]
    public List<Sound> sounds = new List<Sound>();

    [NaughtyAttributes.ReorderableList]
    public List<SoundGroup> groups = new List<SoundGroup>();
}
