using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance {get; private set;}
    private AudioMixerGroup sfxMixerGroup;
    
    //cap the max number of sounds that can play at a time
    private int poolSize = 10;

    private AudioSource[] sfxPool;

    //enforce the singleton I guess
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        InitializePool();
    }

    private void InitializePool()
    {
        sfxPool = new AudioSource[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = new GameObject("SFX_Source_" + i);
            obj.transform.SetParent(transform);

            AudioSource source = obj.AddComponent<AudioSource>();
            source.outputAudioMixerGroup = sfxMixerGroup;
            source.spatialBlend = 1f; //vcalue of 1 forces the sound to be fully 3D
            source.playOnAwake = false;

            sfxPool[i] = source;
        }
    }

    public void Play3DSound(AudioClip clip, Vector3 position, float volume = 1f)
    {
        if (clip == null)
        {
            return;
        }

        foreach (AudioSource source in sfxPool)
        {
            if(!source.isPlaying)
            {
                source.transform.position = position;
                source.clip = clip;
                source.volume = volume;

                //add pitch variance to prevent repetitive sounds
                source.pitch = Random.Range(0.9f, 1.1f);

                source.Play();
                return;
            }
        }
    }
}
