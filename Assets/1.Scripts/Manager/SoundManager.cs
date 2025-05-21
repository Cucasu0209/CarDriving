//using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.SceneManagement;


public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            SetBGMusicState();
            SetSFXState();
            SetHapticState();
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

    }

    #region LOAD BG MUSIC
    private void Start()
    {
        SwitchToMainMenuBGM();

        //SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDestroy()
    {
        //SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Trung Comment
        //switch (scene.name)
        //{
        //    case GameConfig.HOME_SCENE:
        //        SwitchToMainMenuBGM();
        //        break;
        //    case GameConfig.GAMEPLAY_SCENE:
        //        SwitchToChillBGM();
        //        break;
        //}
    }
    #endregion LOAD BG MUSIC

    #region Background Music
    [SerializeField] private AudioSource BGAudioSource;
    [SerializeField] private AudioClip Background_MainMenu;
    [SerializeField] private AudioClip Background_Gameplay;

    private const string BGMusicSetting = "BGMusicSetting";
    public float BGMusicVolume { get; private set; } = 1;
    private void SetBGMusicState()
    {
        BGAudioSource.Play();
        if (PlayerPrefs.HasKey(BGMusicSetting) == false)
        {
            PlayerPrefs.SetInt(BGMusicSetting, 1);
        }
        BGMusicVolume = PlayerPrefs.GetFloat(BGMusicSetting, 1);
        BGAudioSource.volume = BGMusicVolume;
    }


    /// <summary>
    /// Bật tắt background music
    /// </summary>
    public void ChangeBGVolume(float newVolume)
    {
        BGMusicVolume = newVolume;
        BGAudioSource.volume = BGMusicVolume;
        PlayerPrefs.SetFloat(BGMusicSetting, BGMusicVolume);
    }

    public void SwitchToMainMenuBGM()
    {
        BGAudioSource.clip = Background_MainMenu;
        BGAudioSource.mute = false;
        BGAudioSource.Play();
    }

    public void SwitchToGameplay()
    {
        BGAudioSource.clip = Background_Gameplay;
        BGAudioSource.mute = false;
        BGAudioSource.Play();
    }
    #endregion Background Music

    #region SOUND EFFECT
    [SerializeField] private AudioSource SFXAudioSource;
    private const string SFXSetting = "SFXSetting";
    public float SFXVolume { get; private set; } = 1;

    private void SetSFXState()
    {
        if (PlayerPrefs.HasKey(SFXSetting) == false)
        {
            PlayerPrefs.SetInt(SFXSetting, 1);
        }
        SFXVolume = PlayerPrefs.GetFloat(SFXSetting, 1);
        SFXAudioSource.volume = SFXVolume;
    }
    /// <summary>
    /// Bật tắt sound effect
    /// </summary>
    public void ChangeSFXVolume(float newVolume)
    {
        SFXVolume = newVolume;
        SFXAudioSource.volume = SFXVolume;

        PlayerPrefs.SetFloat(SFXSetting, SFXVolume);
    }


    private Dictionary<string, Vector2> SoundDelayTimeDict = new Dictionary<string, Vector2>();
    public void PlayEffect(AudioClip clip, float pitch = 1, float volume = 1, float delayTime = -1, string soundName = "")
    {
        if (clip == null)
        {
            Debug.LogError("null sound");
            return;
        }
        SFXAudioSource.pitch = pitch;
        if (delayTime < 0)
        {
            SFXAudioSource.PlayOneShot(clip, volume);
        }
        else
        {
            if (soundName == "")
            {
                Debug.LogError("Thiếu sound name cho sound cần delay");
            }
            if (SoundDelayTimeDict.ContainsKey(soundName) == false)
            {
                SoundDelayTimeDict.Add(soundName, Vector2.zero);
            }
            if (Time.realtimeSinceStartup >= SoundDelayTimeDict[soundName].x + SoundDelayTimeDict[soundName].y)
            {
                SoundDelayTimeDict[soundName] = new Vector2(Time.realtimeSinceStartup, delayTime);
                SFXAudioSource.PlayOneShot(clip, volume);
            }
        }

    }

    public void PlayEffectWithDelay(AudioClip clip, Action callbackAction = null, float delayTime = 0.25f, float pitch = 1, float volume = 1)
    {
        if (callbackAction != null)
        {
            DOVirtual.DelayedCall(delayTime, () => callbackAction());
        }
        PlayEffect(clip, pitch, volume);
    }
    #endregion SOUND EFFECT

    #region MUTUAL SOUND
    [SerializeField] private AudioClip ButtonSound;
    [SerializeField] private AudioClip OpenPopupSound;
    [SerializeField] private AudioClip ClosePopupSound;


    public void PlayButtonSound()
    {
        PlayEffect(ButtonSound);
    }

    public void PlayOpenPopupSound()
    {
        PlayEffect(OpenPopupSound);
    }
    public void PlayClosePopupSound()
    {
        PlayEffect(ClosePopupSound);
    }
    #endregion MUTUAL SOUND

    #region LOOP SOUND
    private List<AudioSource> CurrentLoopSounds = new List<AudioSource>();
    public void PlayLoop(AudioClip clip, float pitch = 1, float volume = 1)
    {
        if (clip == null)
        {
            Debug.LogError("null sound");
            return;
        }

        for (int i = 0; i < CurrentLoopSounds.Count; i++)
        {
            if (CurrentLoopSounds[i].loop == true && CurrentLoopSounds[i].clip == clip)
            {
                return;
            }
        }
        AudioSource newSource = PoolingSystem.Spawn(SFXAudioSource.gameObject, transform.position, Quaternion.identity).GetComponent<AudioSource>();
        newSource.transform.SetParent(transform);

        CurrentLoopSounds.Add(newSource);
        newSource.pitch = pitch;
        newSource.volume = volume;
        newSource.clip = clip;
        newSource.loop = true;
        newSource.Play();
    }
    public void StopLoopSound(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogError("null sound");
            return;
        }

        for (int i = 0; i < CurrentLoopSounds.Count; i++)
        {
            if (CurrentLoopSounds[i].loop == true && CurrentLoopSounds[i].clip == clip)
            {
                CurrentLoopSounds[i].Stop();
                PoolingSystem.Despawn(CurrentLoopSounds[i].gameObject);
                CurrentLoopSounds.RemoveAt(i);
                return;
            }
        }
    }
    #endregion

    #region Haptic
    private const string HapticSetting = "HapticSetting";
    public int HapticMode { get; private set; } = 0;
    private void SetHapticState()
    {
        if (PlayerPrefs.HasKey(HapticSetting) == false)
        {
            PlayerPrefs.SetInt(HapticSetting, 1);
        }
        HapticMode = PlayerPrefs.GetInt(HapticSetting, 0);
    }
    /// <summary>
    /// Bật tắt rung
    /// </summary>
    public void SetHaptic(int value)
    {
        HapticMode = value;

        PlayerPrefs.SetInt(HapticSetting, HapticMode);
    }
    // private float HapticDelay = 0.25f;
    // private float HapticLastTime = 0;
    public void Vibrate()
    {
        //Trung Comment
        //HapticMode = PlayerPrefs.GetInt(HapticSetting, 0);
        //switch (HapticMode)
        //{
        //    case 0: // OFF
        //        break;
        //    case 1: // LIGHT
        //        if (Time.realtimeSinceStartup >= HapticLastTime + HapticDelay)
        //        {
        //            HapticLastTime = Time.realtimeSinceStartup;
        //            HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
        //        }
        //        break;
        //    case 2: // MEDIUM
        //        if (Time.realtimeSinceStartup >= HapticLastTime + HapticDelay)
        //        {
        //            HapticLastTime = Time.realtimeSinceStartup;
        //            HapticPatterns.PlayPreset(HapticPatterns.PresetType.MediumImpact);
        //        }
        //        break;
        //    case 3: // HEAVY
        //        if (Time.realtimeSinceStartup >= HapticLastTime + HapticDelay)
        //        {
        //            HapticLastTime = Time.realtimeSinceStartup;
        //            HapticPatterns.PlayPreset(HapticPatterns.PresetType.HeavyImpact);
        //        }
        //        break;
        //    default:
        //        break;
        //}
    }
    #endregion Haptic
}
