using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // :: variables
    public int clipIndex = 0;
    public bool playOnAwake = true;
    [Range(0.0f, 1.0f)] public float fade = 1.0f;
    [Range(0.0f, 1.0f)] public float volume = 0.7f;
    public List<AudioClip> musicClips = new List<AudioClip>();
    public List<AudioClip> soundClips = new List<AudioClip>();
    private int m_musicIndex1 = 0;
    private int m_musicIndex2 = 0;
    private bool m_musicFading = false;
    private AudioSource[] m_soundSources;
    private AudioSource[] m_musicSources = new AudioSource[2];
    
    // :: functions
    void Awake()
    {
        // create audio source
        m_musicSources[0] = gameObject.AddComponent<AudioSource>() as AudioSource;
        m_musicSources[1] = gameObject.AddComponent<AudioSource>() as AudioSource;
        // set default audio values
        for (int i = 0; i < m_musicSources.Length; i++)
        {
            m_musicSources[i].playOnAwake = false;
            m_musicSources[i].volume = 0.0f;
            m_musicSources[i].clip = null;
            m_musicSources[i].loop = true;
            m_musicSources[i].Stop();
        }
        // check for audio play on awake
        if (playOnAwake)
        {
            // play clip index
            PlayNextMusic(clipIndex);
        }
    }
	void Update ()
    {
        // check if fading
        if (m_musicFading)
        {
            float delta = fade * Time.deltaTime;
            // fade audio source
            FadeIn(m_musicSources[m_musicIndex1], delta);
            FadeOut(m_musicSources[m_musicIndex2], delta);
            // check if complete
            if (m_musicSources[m_musicIndex2].volume > 0.0f) return;
            if (m_musicSources[m_musicIndex1].volume < volume) return;
            // set as complete
            m_musicFading = false;
            m_musicSources[m_musicIndex2].Stop();
        }
        else
        {
            // set current volume
            m_musicSources[m_musicIndex1].volume = volume;
        }

    }
    public void FadeIn (AudioSource source, float delta)
    {
        // check audio volume
        if (source.volume < volume)
        {
            // increment volume
            source.volume += delta;
        }
        else
        {
            // set final volume
            source.volume = volume;
        }
    }
    public void FadeOut(AudioSource source, float delta)
    {
        // check audio volume
        if (source.volume > delta)
        {
            // increment volume
            source.volume -= delta;
        }
        else
        {
            // set final volume
            source.volume = 0.0f;
        }
    }
    public void PlayMusic(int index)
    {
        // play clip from list
        PlayMusic(musicClips[index]);
    }
    public void PlayMusic(AudioClip clip)
    {
        // instantiate new object
        GameObject test = new GameObject();
        GameObject instanceOBJ = Instantiate(test, transform);
        // attach audio clip to object
        AudioSource source = instanceOBJ.AddComponent<AudioSource>();
        source.clip = clip;
        source.Play();
        // delete object by audio time
        Destroy(test, clip.length);
        Destroy(instanceOBJ.gameObject, clip.length);
    }
    public void PlayNextMusic(int index)
    {
        //if (clipIndex == index) return;
        // set new values
        clipIndex = index;
        m_musicFading = true;
        m_musicIndex1 = m_musicIndex2;
        m_musicIndex2 = 1 - m_musicIndex1;
        // set new clip to play
        m_musicSources[m_musicIndex1].clip = musicClips[index];
        m_musicSources[m_musicIndex1].Play();
    }

    public void Fade()

    [System.Serializable]
    public class AudioData
    {
        public List<AudioClip> clips = new List<AudioClip>();
        public List<AudioSource> sources = new List<AudioSource>();
    }

    public class AudioFade
    {
        public int index1 = 0;
        public int index2 = 0;
        public bool fading = false;
        public AudioData data = null;

        public bool Update(float volume, float delta)
        {
            if (data.sources[index1].volume < volume)
            {
                data.sources[index1].volume += delta;
                data.sources[index2].volume -= delta;
                return true;
            }
            else
            {
                data.sources[index2].volume = 0;
                data.sources[index1].volume = volume;
                return false;
            }
        }
    }
}