using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Options : MonoBehaviour
{
    public AudioManager audioManager;
    public PlayerCamera playerCamera;
    public PlayerMovement playerMovement;
    public AudioSource loadingAudioSource;
    public UnityEngine.UI.Slider mouseSlider;
    public UnityEngine.UI.Slider volumeSlider;

    public void Start()
    {
        mouseSlider.value = audioManager.volume;
        volumeSlider.value = playerCamera.m_ply.m_cameraSensitivity;
        loadingAudioSource.volume = audioManager.volume;
    }
    public void UpdateMaxVolume()
    {
        audioManager.volume = volumeSlider.value;
        loadingAudioSource.volume = volumeSlider.value;
        foreach (AudioSource source in audioManager.sourceTable.Values)
        {
            source.volume = volumeSlider.value;
        }
    }
    public void UpdateMouseSensitivity()
    {
        playerCamera.m_ply.m_cameraSensitivity = mouseSlider.value * 3;
        playerMovement.m_ply.m_cameraSensitivity = mouseSlider.value * 3;
    }
}