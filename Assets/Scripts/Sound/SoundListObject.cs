using System.Collections.Generic;
using UnityEngine;

public class SoundListObject : MonoBehaviour
{
    public static SoundListObject instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    [SerializeField] public List<AudioClip> _SFXALL;
    [SerializeField] public List<AudioClip> _BGMALL;
    public void OnclickSFX(int sfx_index)
    {
        if (SoundManager.instance != null)
            SoundManager.instance.PlaySound(_SFXALL[sfx_index]);
    }
}
