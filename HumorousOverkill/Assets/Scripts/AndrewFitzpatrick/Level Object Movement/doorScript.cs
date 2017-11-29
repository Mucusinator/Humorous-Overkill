using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// opens and closes doors
public class doorScript : MonoBehaviour
{
    [Tooltip("how far up to move when opening")]
    public float openHeight;

    [Tooltip("time in seconds that it takes for the door to open / close")]
    public float openCloseTime = 1.0f;

    // points to move between
    private Vector3[] points = new Vector3[2];

    [Tooltip("When the door has closed this many times it will never open again")]
    public int closeCount = 1;

    // whether the door is open
    public bool open = false;

    // current close factor
    private float currentFactor = 0.0f;

    // audio for door open / close
    public AudioClip openSound;
    public AudioClip closeSound;

    // reference to audiosource
    private AudioSource audioSource;

    void Start()
    {
        // setup points
        points[0] = transform.position;
        points[1] = transform.position + Vector3.up * openHeight;

        // make sure there is no problems with divide by zero or negative numbers
        openCloseTime = Mathf.Max(openCloseTime, 0.0001f);

        // find an AudioSource (if one exists)
        if(GameObject.FindObjectOfType<AudioSource>() != null)
        {
            audioSource = GameObject.FindObjectOfType<AudioSource>();
        }
    }

    void Update()
    {
        // update factor
        if (open)
        {
            currentFactor = Mathf.Min(currentFactor + Time.deltaTime / openCloseTime, 1.0f);
        }
        else
        {
            currentFactor = Mathf.Max(currentFactor - Time.deltaTime / openCloseTime, 0.0f);
        }

        // update position
        transform.position = Vector3.Lerp(points[0], points[1], currentFactor);
    }

    public void openDoor()
    {
        if(closeCount > 0)
        {
            open = true;

            // play open sound effect
            if(audioSource != null && openSound != null)
            {
                audioSource.PlayOneShot(openSound);
            }
        }
    }

    public void closeDoor()
    {
        open = false;
        closeCount--;

        // play close sound effect
        if (audioSource != null && closeSound != null)
        {
            audioSource.PlayOneShot(closeSound);
        }
    }
}
