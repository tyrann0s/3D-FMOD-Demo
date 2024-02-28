using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tSound : MonoBehaviour
{
    [SerializeField]
    [EventRef]
    protected string soundPath;
    public string SoundPath => soundPath;

    [SerializeField]
    private bool isPlayedFromStart;

    [SerializeField]
    [Range(0f, 1f)]
    private float startVolume;

    [SerializeField]
    protected float maxDistance;
    public float MaxDistance => maxDistance;

    [SerializeField]
    private LayerMask playerLayer;

    [Header("Debug")]
    [SerializeField]
    private float isFiltered;
    [SerializeField]
    protected float volume;
    [SerializeField]
    private float distanceFilter;

    public Room TargetRoom { get; set; }
    public Room LastRoom { get; set; }

    protected FMOD.Studio.EventInstance soundInstance;
    public FMOD.Studio.EventInstance SoundInstance => soundInstance;

    protected SoundManager soundManager;

    private Player player;
    
    protected virtual void Start()
    {
        soundInstance = RuntimeManager.CreateInstance(soundPath);

        soundManager = FindObjectOfType<SoundManager>();

        if (isPlayedFromStart) Play();
        soundInstance.setVolume(startVolume);

        player = FindObjectOfType<Player>();
    }

    private void FixedUpdate()
    {
        Physics.Raycast(transform.position, (player.transform.position - transform.position).normalized, out RaycastHit hit, Mathf.Infinity, playerLayer);
        float dist = hit.distance / maxDistance;

        SetSoundParameters(dist);

        //if (TargetRoom == null)
        //{
        //    float dist = PlayerHit.distance / maxDistance;

        //    SetSoundParameters(dist);
        //}
        //else
        //{
        //    //Physics.Raycast(transform.position, (player.transform.position - transform.position).normalized, out RaycastHit hit, Mathf.Infinity, playerLayer);

        //    float dist = hit.distance / maxDistance;

        //    SetSoundParameters(dist);
        //}
    }

    protected virtual void SetSoundParameters(float input)
    {
        float volume = Mathf.Clamp01(1 - input);

        soundInstance.setVolume(volume);
        soundInstance.setParameterByName("DistanceFilter", volume);
    }

    protected virtual void OnDestroy()
    {
        soundInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void Play()
    {
        RuntimeManager.AttachInstanceToGameObject(soundInstance, transform);
        soundInstance.start();
    }

    public void Pause(bool value)
    {
        soundInstance.setPaused(value);
    }

    public void Stop()
    {
        soundInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void StopImmediate()
    {
        soundInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    private void Update()
    {
        soundInstance.getParameterByName("RoomFilter", out isFiltered);
        soundInstance.getVolume(out volume);
        soundInstance.getParameterByName("DistanceFilter", out distanceFilter);
    }
}
