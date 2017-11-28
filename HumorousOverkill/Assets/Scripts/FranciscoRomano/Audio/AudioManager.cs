using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private class FadeInformation
    {
        // :: variables
        public float speed = 0.1f;
        public AudioSource source = null;
        public static float maximum = 1.0f;
        public static float minimum = 0.0f;
        // :: functions
        public void Update()
        {
            // update fade values
            float amount = source.volume + (speed * Time.deltaTime);
            source.volume = Mathf.Clamp(amount, minimum, maximum);
        }
        public bool IsFadeComplete()
        {
            // check if fade finished
            return source.volume == maximum || source.volume == minimum;
        }
    }
    // :: variables
    [Range(0, 1)] public float volume = 0.5f;
    public List<AudioClip> musicClips = new List<AudioClip>();
    public List<AudioClip> soundClips = new List<AudioClip>();
    private Dictionary<AudioClip, AudioSource> sourceTable = new Dictionary<AudioClip, AudioSource>();
    private Dictionary<AudioClip, FadeInformation> fadeTable = new Dictionary<AudioClip, FadeInformation>();
    // :: functions
    void Awake()
    {
        foreach (AudioClip clip in musicClips)
        {
            AddClip(clip, 0);
        }
    }
    void Update()
    {
        FadeInformation.maximum = volume;
        foreach (AudioClip clip in musicClips)
        {
            if (fadeTable.ContainsKey(clip))
            {
                fadeTable[clip].Update();
                if (fadeTable[clip].IsFadeComplete())
                {
                    Debug.Log("removing fade");
                    fadeTable.Remove(clip);
                }
            }
        }
        foreach (AudioClip clip in soundClips)
        {
            if (fadeTable.ContainsKey(clip))
            {
                fadeTable[clip].Update();
                if (fadeTable[clip].IsFadeComplete())
                {
                    Debug.Log("removing fade");
                    fadeTable.Remove(clip);
                }
            }
        }
    }
    void AddClip(AudioClip clip, float volume)
    {
        if (sourceTable.ContainsKey(clip)) return;
        GameObject obj = Instantiate(new GameObject("manager-audio"), transform);
        AudioSource source = obj.AddComponent<AudioSource>();
        source.playOnAwake = false;
        source.volume = volume;
        source.clip = clip;
        source.Stop();
        sourceTable.Add(clip, source);
    }
    void StopClip(AudioClip clip)
    {
        if (sourceTable.ContainsKey(clip))
        {
            sourceTable[clip].Stop();
        }
    }
    void PlayClip(AudioClip clip, bool repeat)
    {
        if (sourceTable.ContainsKey(clip))
        {
            if (repeat)
            {
                sourceTable[clip].Play();
                sourceTable[clip].loop = true;
            }
            else
            {
                sourceTable[clip].PlayOneShot(clip);
            }
        }
    }
    void FadeClip(AudioSource source, AudioClip clip, float speed)
    {
        Debug.Log(speed < 0 ? "[Fade] adding fadeOut" : "[Fade] adding fadeIn");
        if (!musicClips.Contains(clip) && !soundClips.Contains(clip)) return;
        FadeInformation info = new FadeInformation();
        info.source = source;
        info.speed = 1 / speed;
        if (fadeTable.ContainsKey(clip))
        {
            fadeTable[clip] = info;
        }
        else
        {
            fadeTable.Add(clip, info);
        }
        info.Update();
    }
    public void AddMusic(AudioClip clip) { if (!musicClips.Contains(clip)) { AddClip(clip, 0); musicClips.Add(clip); } }
    public void AddSound(AudioClip clip) { if (!soundClips.Contains(clip)) { AddClip(clip, 1); soundClips.Add(clip); } }
    public void StopMusic(int index) { StopClip(musicClips[index]); }
    public void StopSound(int index) { try { StopClip(soundClips[index]); } catch { } }
    public void StopMusic(AudioClip clip) { StopClip(clip); }
    public void StopSound(AudioClip clip) { StopClip(clip); }
    public void PlayMusic(int index, bool repeat) { PlayClip(musicClips[index], repeat); }
    public void PlaySound(int index, bool repeat) { PlayClip(soundClips[index], repeat); }
    public void PlayMusic(AudioClip clip, bool repeat) { PlayClip(clip, repeat); }
    public void PlaySound(AudioClip clip, bool repeat) { PlayClip(clip, repeat); }
    public void FadeInMusic(int index, float speed) { FadeInMusic(musicClips[index], speed); }
    public void FadeInMusic(AudioClip clip, float speed) { FadeClip(sourceTable[clip], clip, speed); }
    public void FadeOutMusic(int index, float speed) { FadeOutMusic(musicClips[index], speed); }
    public void FadeOutMusic(AudioClip clip, float speed) { FadeClip(sourceTable[clip], clip, -speed); }
}

//// :: variables
//[Range(0.01f, 1.00f)]
//public float fade = 1.0f;
//[Range(0.00f, 1.00f)] public float volume = 0.7f;
//public List<AudioClip> musics = new List<AudioClip>();
//private List<AudioFade> audioFadeIn = new List<AudioFade>();
//private List<AudioFade> audioFadeOut = new List<AudioFade>();
//private List<AudioSource> audioSources = new List<AudioSource>();
//private Dictionary<AudioClip, AudioSource> audioDictionary = new Dictionary<AudioClip,AudioSource>();

//// :: functions
//void Awake()
//{
//    //Add(musics[0]);
//    //FadeIn(musics[0], fade);
//}

//void Start () {
//    EventManager<GameEvent>.InvokeGameState(this, null, null, GetType(), GameEvent._NULL_);
//}

//void Update()
//{
//    foreach (AudioFade fade in audioFadeIn)
//    {
//        if (!fade.Update(Time.deltaTime))
//        {
//            audioFadeIn.Remove(fade);
//            audioSources.Add(fade.source);
//        }
//    }
//    foreach (AudioFade fade in audioFadeOut)
//    {
//        if (!fade.Update(-Time.deltaTime))
//        {
//            fade.source.Stop();
//            audioFadeIn.Remove(fade);
//            audioSources.Add(fade.source);
//        }
//    }
//}
//public void Add(AudioClip clip)
//{
//    if (audioDictionary.ContainsKey(clip)) return;
//    // create objects
//    GameObject gameObj = Instantiate(new GameObject("manager-audio"), transform);
//    AudioSource source = gameObj.AddComponent<AudioSource>();
//    source.playOnAwake = false;
//    source.clip = clip;
//    source.Stop();
//    // update audio manager
//    audioSources.Add(source);
//    audioDictionary.Add(clip, source);
//}
//public void Stop(AudioClip clip)
//{
//    audioDictionary[clip].Stop();
//}
//public void Play(AudioClip clip)
//{
//    audioDictionary[clip].Play();
//    audioDictionary[clip].loop = true;
//}
//public void Play1(AudioClip clip)
//{
//    audioDictionary[clip].Play();
//    audioDictionary[clip].loop = false;
//}
//public void FadeIn(AudioClip clip, float speed)
//{
//    if (audioDictionary.ContainsKey(clip))
//    {
//        // create audio fade
//        AudioFade fade = new AudioFade();
//        fade.source = audioDictionary[clip];
//        fade.source.loop = true;
//        fade.source.Play();
//        fade.speed = speed;
//        // update audio manager
//        audioFadeIn.Add(fade);
//        audioSources.Remove(fade.source);
//    }
//}
//public void FadeOut(AudioClip clip, float speed)
//{
//    if (audioDictionary.ContainsKey(clip))
//    {
//        // create audio fade
//        AudioFade fade = new AudioFade();
//        fade.source = audioDictionary[clip];
//        // update audio manager
//        audioFadeOut.Add(fade);
//        audioSources.Remove(fade.source);
//    }
//}
//[System.Serializable]
//public class AudioFade
//{
//    // :: variables
//    public float speed;
//    public AudioSource source;
//    public static float minimum = 0.0f;
//    public static float maximum = 1.0f;
//    // :: functions
//    public bool Update(float delta)
//    {
//        // update volume
//        source.volume += delta * speed;
//        // calculate clamp
//        float clamp = Mathf.Clamp(source.volume, minimum, maximum);
//        // check if volume is out of range
//        if (clamp != source.volume)
//        {
//            // clamp volume
//            source.volume = clamp;
//            return false;
//        }
//        return true;
//    }
//}