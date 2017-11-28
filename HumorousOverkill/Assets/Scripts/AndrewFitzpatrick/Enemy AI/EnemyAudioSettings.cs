using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// audio settings for enemies
[System.Serializable]
public struct EnemyAudioSettings
{
    [Tooltip("sound that will always be played when this enemy dies")]
    public AudioClip deathSound;

    [Range(0.0f, 1.0f)]
    [Tooltip("chance to play one of the random death sounds when this enemy dies")]
    public float randomDeathSoundChance;
    [Tooltip("death sounds to randomy play when this enemy dies")]
    public List<AudioClip> randomDeathSounds;
}
