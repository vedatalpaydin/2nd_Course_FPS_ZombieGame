using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSounds : MonoBehaviour
{
    public AudioSource sound;
    public bool sound3D = true;
    public float firstPlay;
    public float randomMin;
    public float randomMax;
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(PlaySound), firstPlay);
    }

    void PlaySound()
    {
        GameObject newSound = new GameObject();
        AudioSource newAS=newSound.AddComponent<AudioSource>();
        newAS.clip = sound.clip;
        if (sound3D)
        {
            newAS.spatialBlend = 1;
            newAS.maxDistance = sound.maxDistance;
            newAS.pitch = sound.pitch;
            newSound.transform.SetParent(transform);
            newSound.transform.localPosition = Vector3.zero;
        }
        newAS.Play();
        Invoke(nameof(PlaySound), Random.Range(randomMin,randomMax));
        Destroy(newSound,sound.clip.length);
    }
}
