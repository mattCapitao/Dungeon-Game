using System.Collections;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [SerializeField] public AudioSource _startSource, _musicSource, _victorySource, _sfxSource, _hitSource;

    public static SFXManager Instance;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySfx(AudioClip clip)
    {
        _sfxSource.PlayOneShot(clip);
    }

    public void PlayHit(AudioClip clip)
    {
        float hitDelay = 0.2f; //* Time.deltaTime;
        _hitSource.PlayDelayed(hitDelay);
    }

    public void PlayFinal(AudioClip clip)
    {
        _sfxSource.volume = 1f;
        _sfxSource.PlayOneShot(clip);
    }

    public void PlayStart()
    {
        _startSource.volume = .2f;
        _startSource.Play();
    }

    public void StopStart()
    {
        //AudioSource music = _startSource;
        StartCoroutine(FadeMusicCoRoutine(_startSource, 8f));
    }

    public void PlayMusic()
    {
        _musicSource.volume = .2f;
        _musicSource.Play();
    }

    public void PlayVictory()
    {
        _victorySource.volume = .5f;
        _victorySource.Play();
    }

    public void StopVictory()
    {
        StartCoroutine(FadeMusicCoRoutine(_victorySource, 3f));
    }

    public void FadeMusic(float fadeTime)
    {
        StartCoroutine(FadeMusicCoRoutine(_musicSource, fadeTime));
    }

    private static IEnumerator FadeMusicCoRoutine(AudioSource music, float fadeTime)
    {
        float startVolume = music.volume;
        while (music.volume > 0)
        {
            music.volume -= startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }

        music.Stop();
        music.volume = startVolume;
    }

}

