using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    public static SoundManager Instance { get; set; }
    [SerializeField]
    private AudioSource sfxPlayer, musicPlayer,lavaSource,ambiencePlayer;
    [SerializeField]
    public SoundPrefab soundPrefab;
	// Use this for initialization
	void Awake () {
        if (Instance == null)
        {
            Instance = this;
            sfxPlayer = transform.GetChild(0).GetComponent<AudioSource>();
            musicPlayer = transform.GetChild(1).GetComponent<AudioSource>();
            lavaSource = transform.GetChild(2).GetComponent<AudioSource>();
            ambiencePlayer = transform.GetChild(3).GetComponent<AudioSource>();

        }



    }
    public void PlayClickSFX()
    {
        SoundManager.Instance.PlaySFX(SFX.clickSFX, 1f);
    }
    public void PlayLavaSource()
    {
        if(SettingPanel.Instance.isSFXEnabled)
        lavaSource.Play();
    }
    public void StopLava()
    {
        if (lavaSource!=null &&lavaSource.isPlaying)
        {
            lavaSource.Stop();
        }
    }
    public void PlaySFX(SFX name, float vol = 1, float time = 0)
    {

        PlaySFX(soundPrefab.GetClip(name), vol,time);
    }
    public void PlayAmbience(AudioClip clip,float vol = 1)
    {
        if (!SettingPanel.Instance.isSFXEnabled ||clip==null) return;
        if (ambiencePlayer.clip == clip) return;
        ambiencePlayer.volume = vol;
        ambiencePlayer.clip = clip;
        ambiencePlayer.Play();
    }
    public void StopAmbience()
    {
        if (ambiencePlayer.isPlaying)
        {
            ambiencePlayer.Stop();
        }
    }

    public void StopFadeAmbience()
    {
        StopAllCoroutines();
        StartCoroutine(DoFadeOutAmbience());
    }
    IEnumerator DoFadeOutAmbience()
    {
        float defaultVol = ambiencePlayer.volume;
        while (defaultVol > 0)
        {
            defaultVol -= 1 * Time.deltaTime;
            ambiencePlayer.volume = defaultVol;
            yield return null;
        }
        ambiencePlayer.Stop();
    }
  
    public void PlayMusic(SFX name,float vol=0.5f)
    {
        if (musicPlayer.isPlaying)
        {
            if (PlayerPrefs.GetInt("Music", 1) == 1)
            {
                musicPlayer.UnPause();
                ChangeMusic(name,vol);
            }
        }
        else
        {
            PlayMusic(soundPrefab.GetClip(name), vol);
        }
        
    }
    public void PlaySFX(AudioClip clip,float vol, float time = 0)
    {
        if (clip == null) return;
        if (SettingPanel.Instance.isSFXEnabled)
        {
            if (time == 0)
            {
                sfxPlayer.PlayOneShot(clip, vol);
            }
            else
            {
                StartCoroutine(DoPlayDelay(clip, vol, time));
            }
        }
    }
    IEnumerator DoPlayDelay(AudioClip clip, float vol, float time = 0)
    {
        yield return new WaitForSeconds(time);
        sfxPlayer.PlayOneShot(clip, vol + 0.15f);

    }
    public void ChangeMusic(AudioClip newClip, float vol = 0.5f, bool noFade = false, bool useTime = false)
    {
        if (PlayerPrefs.GetInt("MusicOn", 1) == 1)
        {

            if (musicPlayer.clip.name != newClip.name)
            {
                if (noFade)
                {
                    float currentTime = 0;
                    if (useTime)
                    {
                        currentTime = musicPlayer.time;
                        musicPlayer.clip = newClip;
                        musicPlayer.volume = vol;
                        musicPlayer.time = currentTime;

                        musicPlayer.Play();

                    }
                    else
                    {
                        musicPlayer.clip = newClip;
                        musicPlayer.volume = vol;
                        musicPlayer.Play();
                    }

                }
                else
                {
                    StartCoroutine(FadeChangeMusic(newClip, vol));
                }
            }
            else
            {
                musicPlayer.volume = vol;
                if (!musicPlayer.isPlaying)
                {
                    musicPlayer.Play();
                }

            }
        }
    }
    public void ChangeMusic(SFX name,float vol=0.5f,bool noFade=false,bool useTime=false)
    {
        if (PlayerPrefs.GetInt("MusicOn", 1) == 1)
        {
            AudioClip newClip = soundPrefab.GetClip(name);
            if (name.Equals(SFX.theme))
            {
                int index = ((GameController.Instance.currentLevel - 1) / 8) % soundPrefab.gameClips.Length;
                newClip=soundPrefab.gameClips[index];
            }
            if (musicPlayer.clip.name != newClip.name)
            {
                if (noFade)
                {
                    float currentTime = 0;
                    if (useTime)
                    {
                        currentTime = musicPlayer.time;
                        musicPlayer.clip = newClip;
                        musicPlayer.volume = vol;
                        musicPlayer.time = currentTime;

                        musicPlayer.Play();

                    }
                    else
                    {
                        musicPlayer.clip = newClip;
                        musicPlayer.volume = vol;
                        musicPlayer.Play();
                    }
                   
                }
                else
                {
                    StartCoroutine(FadeChangeMusic(newClip, vol));
                }
            }
            else
            {
                musicPlayer.volume = vol;
                if (!musicPlayer.isPlaying)
                {
                    musicPlayer.Play();
                }
                
            }
        }
    }
    bool isChanging = false;
    IEnumerator FadeChangeMusic(AudioClip clip,float vol)
    {
        Debug.Log("ISISIS:" + isChanging);
        while (isChanging)
        {
            yield return null;
        }
        isChanging = true;
        float currentVol = musicPlayer.volume;
        while (musicPlayer.volume > 0)
        {
            musicPlayer.volume -= currentVol*2* Time.deltaTime;
            yield return null;
        }
        musicPlayer.clip = clip;
        musicPlayer.Play();
        while (musicPlayer.volume <vol)
        {
            musicPlayer.volume += vol*2 * Time.deltaTime;
            yield return null;
        }

        musicPlayer.volume = vol;
        isChanging = false;
    }
    public void PlayMusic(AudioClip clip,float vol)
    {
        if (clip == null) return;

        if (musicPlayer.isPlaying)
        {
            return;
        }
        if (PlayerPrefs.GetInt("MusicOn", 1)==1)
        {

            musicPlayer.clip=clip;
            musicPlayer.volume = vol;
            musicPlayer.Play();
        }
        else
        {
            musicPlayer.clip = clip;
            musicPlayer.volume = vol;
            musicPlayer.Play();
            musicPlayer.Pause();
        }
    }
    public bool IsMusicPlaying()
    {
        return musicPlayer.isPlaying;
    }
    public void StopSFX()
    {
        if (sfxPlayer.isPlaying)
        {
            sfxPlayer.Stop();
        }
        StopLava();
    }
    public void StopMusic()
    {
        if (musicPlayer.isPlaying)
        {
            musicPlayer.volume=0.1f;
        }
    }
    public void PauseMusic()
    {
        if (musicPlayer.isPlaying)
        {
            musicPlayer.Pause();
        }
    }
    public float ChangeVolumn(float vol)
    {
        float current = musicPlayer.volume;
        musicPlayer.volume=vol;
        return current;
    }
    public void ContinueMusic()
    {
        if (musicPlayer.isPlaying)
        {
            musicPlayer.volume = 0.5f;
        }
        else
        {
            musicPlayer.UnPause();
        }
    }
}

