using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    [Header("Components")]
    [SerializeField] public SoundInstance soundInstancePrefabs;
    [SerializeField] public Transform soundParent;
    [Header("Setting")]
    [SerializeField] public int MaxSoundInstance;
    [SerializeField] private SoundInstance BGMsoundInstance;
    [Range(0.1f, 1f)]
    [SerializeField] public float volumeMultipler = 0.5f;


    public void PlaySound(AudioClip sound, float volume = 0.5f, bool isLoop = false)
    {
        if (soundParent.childCount >= MaxSoundInstance) return;

        SoundInstance _instance = Instantiate(soundInstancePrefabs, soundParent);

        _instance.InitInsance(sound, DataSoundHolder.SFX_Volume * volumeMultipler, isLoop);
    }

    public void PlayBGM(AudioClip sound, float volume = 0.5f)
    {
        if (soundParent.childCount >= MaxSoundInstance) return;
        if (BGMsoundInstance != null)
        {
            GetBGMInstance().Stop();
            Destroy(BGMsoundInstance.gameObject);
        }
        SoundInstance _instance = Instantiate(soundInstancePrefabs, soundParent);
        _instance.InitInsance(sound, DataSoundHolder.BGM_Volume * volumeMultipler, true);
        BGMsoundInstance = _instance;
    }
    public void StopBGM()
    {
        GetBGMInstance().Stop();
        Destroy(BGMsoundInstance.gameObject);
    }

    public AudioSource GetBGMInstance()
    {
        return BGMsoundInstance.audioSource;
    }

    public bool isPlayingBgm()
    {
        return BGMsoundInstance != null;
    }
}
