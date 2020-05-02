using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "New Audio Library")]
public class AudioLibrary : ScriptableObject
{
    public List<Sound> sounds = new List<Sound>();
    public List<SoundGroup> groups = new List<SoundGroup>();
}
