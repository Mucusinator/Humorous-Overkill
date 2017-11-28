using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Options : MonoBehaviour
{
    public AudioManager audioManager;
    public PlayerCamera playerCamera;
    public PlayerMovement playerMovement;
    public UnityEngine.UI.Slider mouseSlider;
    public UnityEngine.UI.Slider volumeSlider;

    public void Start()
    {
        mouseSlider.value = audioManager.volume;
        volumeSlider.value = playerCamera.m_ply.m_cameraSensitivity;
    }
    public void UpdateMaxVolume()
    {
        audioManager.volume = volumeSlider.value;
    }
    public void UpdateMouseSensitivity()
    {
        playerCamera.m_ply.m_cameraSensitivity = mouseSlider.value * 3;
        playerMovement.m_ply.m_cameraSensitivity = mouseSlider.value * 3;
    }
}