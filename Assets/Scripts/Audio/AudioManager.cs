using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;

using Random = UnityEngine.Random;

//TODO: Fixa strukturen lite, bit aids atm

public class AudioManager : MonoSingleton<AudioManager>
{
    [Expandable, NaughtyAttributes.ReorderableList]
    public AudioLibrary[] libraries;
    public AudioSource prefab;
    public float audioSourceLifeMultiplier = 2.0f;

    [Header("Mixers")]
    public AudioMixerGroup mixerEffects;
    public AudioMixerGroup mixerMusic;
    public AudioMixerGroup mixerAmbience;

    [Header("Master Volume")]
    [Range(0f, 1f)]
    public float volume = 0.5f;

    Dictionary<string, Sound> _soundDictionary;
    Dictionary<string, SoundGroup> _groupDictionary;
    AudioListener _listener;

    private void OnValidate()
    {
        AudioListener.volume = volume;
    }

    private void Awake()
    {
        AudioListener.volume = 1.0f;
        _soundDictionary = new Dictionary<string, Sound>();
        _groupDictionary = new Dictionary<string, SoundGroup>();
        
        //loopa igenom alla libraries
        for (int i = 0; i < libraries.Length; i++)
        { 
            //loopa igenom alla sounds i detta libraryt
            for (int j = 0; j < libraries[i].data.sounds.Count; j++)
            {
                //lägg till den i sounddicten
                _soundDictionary.Add(libraries[i].data.sounds[j].key, 
                    libraries[i].data.sounds[j]);
            }
            //loopa igenom alla soundsGroups i detta libraryt
            for (int j = 0; j < libraries[i].data.groups.Count; j++)
            {
                //lägg till den i sounddicten
                _groupDictionary.Add(libraries[i].data.groups[j].key, 
                    libraries[i].data.groups[j]);
            }
        }
    }
    public AudioSource PlayEffect(string effectName)
    {
        return PlayEffect(effectName, Vector3.zero);
    }

    public AudioSource PlayEffect(string effectName, Vector3 pos)
    {
        //Kolla så vår listener finns

        AudioSource source = PoolManager.Fetch<AudioSource>(prefab.gameObject);
        
       
        float playTime = 0.0f;
        bool looping = false;

        if(source != null && _soundDictionary.TryGetValue(effectName, out Sound value))
        {
            //Här sätter vi positionen med
            source.transform.position = pos;

            source.clip = null;
            source.clip = value.clip;

            source.pitch = value.pitch;
            source.volume = value.volume;
            source.loop = looping = value.loop;
            source.spatialBlend = value.spatialBlend;
            source.playOnAwake = false;

            playTime = source.clip.length * audioSourceLifeMultiplier;

            switch (value.type)
            {
                case SoundType.Effect:
                    source.outputAudioMixerGroup = mixerEffects;
                    break;
                case SoundType.Ambience:
                    source.outputAudioMixerGroup = mixerAmbience;
                    break;
                default:
                    source.outputAudioMixerGroup = mixerMusic;
                    break;
            }

            StartCoroutine(PlaySourceAfterFrame(source));
            //source.gameObject.SetActive(true);
            //source.Play();
        }

        if (!looping)
        {
            Debug.Log("play time");
            PoolManager.Return(prefab.gameObject, source.gameObject, playTime);
        }

        return source;
    }

    IEnumerator PlaySourceAfterFrame (AudioSource source)
    {
        yield return null;
        source.Stop();
        source.Play(0);
    }

    public AudioSource PlayGroup(string key)
    {
        if (_groupDictionary.ContainsKey(key))
        {
            return PlayEffect(_groupDictionary[key].variationKeys[Random.Range(0, _groupDictionary[key].variationKeys.Length)]);
        }

        return null;
    }

    public AudioSource PlayGroup(string key, Vector3 pos)
    {
        if (!_groupDictionary.ContainsKey(key)) 
            return null;
        
        //Leta upp rätt variationkey från groupDicten
        var temp = _groupDictionary[key].variationKeys[Random.Range(0, _groupDictionary[key].variationKeys.Length)];
        return PlayEffect(temp, pos);
    }
}
