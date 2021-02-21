using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : Singleton<MusicController>
{
    [SerializeField]
    private AudioClip mainSong;

    [SerializeField]
    private AudioClip bossSong;

    private AudioSource audioSource;

    private void Awake()
    {
        if (FindObjectsOfType<MusicController>().Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
        this.audioSource = GetComponent<AudioSource>();

        if (GameManager.Instance.Level == 10)
        {
            PlayBossSong();
        }
    }

    public void PlayMainSong()
    {
        this.audioSource.clip = this.mainSong;
        this.audioSource.Play();
    }

    public void PlayBossSong()
    {
        this.audioSource.clip = this.bossSong;
        this.audioSource.Play();
    }
}
