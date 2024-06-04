using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : SingletonMonoBehavior<SoundManager>
{
    [SerializeField] 
    private AudioSource[] soundEffects;
    [SerializeField] 
    private AudioSource[] backgroundMusic;
    [SerializeField] 
    private float minimumDistanceOfSoundEffects;

    public bool playBackgroundMusic;

    private bool canPlaySoundEffects;
    private int backgroundMusicIndex;

    protected override void Awake()
    {
        base.Awake();
        Invoke(nameof(AllowSoundEffects), 1f);
    }

    private void Update()
    {
        if (!playBackgroundMusic)
        {
            StopAllBackgroundMusic();
        }
        else
        {
            if (!backgroundMusic[backgroundMusicIndex].isPlaying)
            {
                PlayBackgroundMusic(backgroundMusicIndex);
            }
        }
    }

    public void PlaySoundEffects(int _soundEffectsIndex, Transform _source, bool changePitch)
    {
        Debug.Log("Play Sounds: " + _soundEffectsIndex);
        if (canPlaySoundEffects == false)
        {
            return;
        }

        if (_source != null && Vector2.Distance(PlayerManager.Instance.player.transform.position, _source.position) > minimumDistanceOfSoundEffects)
        {
            return;
        }
        
        if (_soundEffectsIndex < soundEffects.Length)
        {
            if (changePitch)
            {
                soundEffects[_soundEffectsIndex].pitch = Random.Range(.9f, 1.1f);
            }
            soundEffects[_soundEffectsIndex].Play();
        }
    }

    public void StopSoundEffects(int _soundEffectsIndex)
    {
        soundEffects[_soundEffectsIndex].Stop();
    }

    public void PlayBackgroundMusic(int _backgroundMusicIndex)
    {
        backgroundMusicIndex = _backgroundMusicIndex;
        
        StopAllBackgroundMusic();
        
        backgroundMusic[backgroundMusicIndex].Play();
    }

    public void StopAllBackgroundMusic()
    {
        for (int i = 0; i < backgroundMusic.Length; i++)
        {
            backgroundMusic[i].Stop();
        }
    }

    public void PlayRandomBackgroundMusic()
    {
        backgroundMusicIndex = Random.Range(0, backgroundMusic.Length);
        PlayBackgroundMusic(backgroundMusicIndex);
    }

    private void AllowSoundEffects()
    {
        canPlaySoundEffects = true;
    }

    public void StopSoundEffectsWithDelay(int index)
    {
        StartCoroutine(DecreaseVolume(soundEffects[index]));
    }

    private IEnumerator DecreaseVolume(AudioSource audio)
    {
        float defaultVolume = audio.volume;

        while (audio.volume > .1f)
        {
            audio.volume -= audio.volume * .2f;
            yield return new WaitForSeconds(.25f);

            if (audio.volume <= .1f)
            {
                audio.Stop();
                audio.volume = defaultVolume;
                break;
            }
        }
    }

    public float GetLengthSoundEffects(int _soundEffectsIndex)
    {
        return soundEffects[_soundEffectsIndex].clip.length;
    }
}
