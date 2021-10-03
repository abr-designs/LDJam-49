using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class AudioController : MonoBehaviour
{
    public enum EFFECT
    {
        STEP = 0,
        CLIMB,
        STARTER,
        
        ALARM,
        EXPLOSION,
        
        BREAKER_ON,
        BREAKER_OFF,
        
        BUTTON_PRESS,
    }
    
    [Serializable]
    public struct EffectProfile
    {
        public string name;
        public AudioClip clip;
        [Range(0f,1f)]
        public float volume;
        public Vector2 pitchRange;
    }

    //====================================================================================================================//

    public static AudioController Instance { get; private set; }

    [SerializeField]
    private AudioMixer masterMixer;

    [SerializeField]
    private AudioSource musicSource;
    [SerializeField]
    private AudioSource sfxSource;
    
    
        
    [SerializeField]
    private EffectProfile[] _effectProfiles;

    //====================================================================================================================//

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    //====================================================================================================================//
    
    public void PlaySoundEffect(EFFECT effect)
    {
        var index = (int) effect;

        var pitchRange = _effectProfiles[index].pitchRange;

        //masterMixer.SetFloat("EffectPitch", Random.Range(pitchRange.x, pitchRange.y));
        sfxSource.PlayOneShot(_effectProfiles[index].clip, _effectProfiles[index].volume);
    }

    //====================================================================================================================//
    
    /// <summary>
    /// Sets the master volume
    /// </summary>
    /// <param name="volume"></param>
    public void SetMasterVolume(float volume)
    {
        SetVolume("MasterVolume", volume);
    }
    public void SetMusicVolume(float volume)
    {
        SetVolume("MusicVolume", volume);
    }
    public void SetEffectsVolume(float volume)
    {
        SetVolume("SFXVolume", volume);
    }

    private void SetVolume(string parameterName, float volume)
    {
        volume = Mathf.Clamp01(volume);
        masterMixer.SetFloat(parameterName, Mathf.Log10(volume) * 40);
    }
}
