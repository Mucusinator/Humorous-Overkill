using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // variables [public]
    public int clipIndex = 0;
    public bool playOnAwake = true;
    [Range(0.0f, 1.0f)]public float fade = 1.0f;
    [Range(0.0f, 1.0f)] public float volume = 0.7f;
    public List<AudioClip> audioClips = new List<AudioClip>();
    // variables [private]
    private int m_audioIndex1 = 0;
    private int m_audioIndex2 = 0;
    private bool m_audioFading = false;
    private AudioSource[] m_audioSources = new AudioSource[2];
    
    void Awake()
    {
        // create audio source
        m_audioSources[0] = gameObject.AddComponent<AudioSource>() as AudioSource;
        m_audioSources[1] = gameObject.AddComponent<AudioSource>() as AudioSource;
        // set default audio values
        for (int i = 0; i < m_audioSources.Length; i++)
        {
            m_audioSources[0].playOnAwake = false;
            m_audioSources[0].volume = 0.0f;
            m_audioSources[0].clip = null;
            m_audioSources[0].loop = true;
            m_audioSources[0].Stop();
        }
        // check for audio play on awake
        if (playOnAwake)
        {
            // play clip index
            PlayNextClip(clipIndex);
        }

    }
	
	void Update ()
    {
        // check if fading
        if (m_audioFading)
        {
            float delta = fade * Time.deltaTime;
            // fade audio source
            FadeIn(m_audioSources[m_audioIndex1], delta);
            FadeOut(m_audioSources[m_audioIndex2], delta);
            // check if complete
            if (m_audioSources[m_audioIndex2].volume > 0.0f) return;
            if (m_audioSources[m_audioIndex1].volume < volume) return;
            // set as complete
            m_audioFading = false;
            m_audioSources[m_audioIndex2].Stop();
        }
        else
        {
            // set current volume
            m_audioSources[m_audioIndex1].volume = volume;
        }

    }

    void FadeIn (AudioSource source, float delta)
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

    void FadeOut(AudioSource source, float delta)
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

    public void PlayClip(int index)
    {
        // play clip from list
        PlayClip(audioClips[index]);
    }

    public void PlayClip(AudioClip clip)
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

    public void PlayNextClip(int index)
    {
        //if (clipIndex == index) return;
        // set new values
        clipIndex = index;
        m_audioFading = true;
        m_audioIndex1 = m_audioIndex2;
        m_audioIndex2 = 1 - m_audioIndex1;
        // set new clip to play
        m_audioSources[m_audioIndex1].clip = audioClips[index];
        m_audioSources[m_audioIndex1].Play();
    }
}