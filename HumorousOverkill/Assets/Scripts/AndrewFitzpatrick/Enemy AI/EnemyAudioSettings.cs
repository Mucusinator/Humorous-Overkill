using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// audio settings for enemies
[System.Serializable]
public struct EnemyAudioSettings
{
    [Tooltip("sound that will always be played when this enemy dies")]
    public AudioClip deathSound;
    [Tooltip("volume to play the death sound at")]
    [Range(0, 1)]
    public float deathSoundVolume;

    [Range(0.0f, 1.0f)]
    [Tooltip("chance to play one of the random death sounds when this enemy dies")]
    public float randomDeathSoundChance;

    [Tooltip("death sounds to randomy play when this enemy dies")]
    public List<AudioClip> randomDeathSounds;

    [Tooltip("volume to play the random death sounds at")]
    [Range(0, 1)]
    public float randomDeathSoundsVolume;

    [Tooltip("sound to play when shooting at the player")]
    public AudioClip shootSound;

    [Tooltip("volume to play the shoot sound at")]
    [Range(0, 1)]
    public float shootSoundVolume;
}
