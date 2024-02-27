using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tSound : tAudio
{
    [SerializeField]
    [FMODUnity.EventRef]
    protected string soundPath;
    public string SoundPath => soundPath;

    [SerializeField]
    private bool isPlayedFromStart;

    [SerializeField]
    [Range(0f, 1f)]
    private float startVolume;

    [SerializeField]
    protected float maxDistance, retranslatorDistanceOffset;
    public float MaxDistance => maxDistance;
    public float RetranslatorDistanceOffset => retranslatorDistanceOffset;

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

    private Retranslator retranslator;
    public Retranslator Retranslator => retranslator;
    private List<Retranslator> retranslatorList = new List<Retranslator>();

    protected SoundManager soundManager;
    
    protected virtual void Start()
    {
        soundInstance = RuntimeManager.CreateInstance(soundPath);
        RuntimeManager.AttachInstanceToGameObject(soundInstance, transform);

        soundManager = FindObjectOfType<SoundManager>();

        if (isPlayedFromStart) Play();
        soundInstance.setVolume(startVolume);
    }

    private void FixedUpdate()
    {
        if (TargetRoom == null)
        {
            if (IsPlayerInSight())
            {
                RuntimeManager.AttachInstanceToGameObject(soundInstance, transform);
                SetRetranslator(false);

                float dist = PlayerHit.distance / maxDistance;

                SetSoundParameters(dist);
            }
            else
            {
                if (player.CurrentRoom != null)
                {
                    RuntimeManager.AttachInstanceToGameObject(soundInstance, transform);
                    SetRetranslator(false);

                    Physics.Raycast(transform.position, (player.transform.position - transform.position).normalized, out RaycastHit hit, Mathf.Infinity);
                    float dist = hit.distance / maxDistance;

                    SetSoundParameters(dist);
                }
                else CastToRetranslator();
            }
        }
        else
        {
            RuntimeManager.AttachInstanceToGameObject(soundInstance, transform);
            SetRetranslator(false);

            Physics.Raycast(transform.position, (player.transform.position - transform.position).normalized, out RaycastHit hit, Mathf.Infinity, playerLayer);

            float dist = hit.distance / maxDistance;

            SetSoundParameters(dist);
        }
    }

    private void SetRetranslator(bool value)
    {
        if (value && retranslator != null)
        {
            retranslator.Set(value);
        } else if (retranslator != null)
        {
            retranslator.Set(value);
            retranslator = null;
        }
    }

    protected virtual void SetSoundParameters(float input)
    {
        float volume = Mathf.Clamp01(1 - input);

        soundInstance.setVolume(volume);
        soundInstance.setParameterByName("DistanceFilter", volume);
    }

    private void CastToRetranslator()
    {
        FindRetranslator();
        if (retranslator == null) return;

        Physics.Raycast(transform.position, (retranslator.transform.position - transform.position).normalized, out RaycastHit hit, Mathf.Infinity);
        float dist = (hit.distance + retranslator.PlayerHit.distance + retranslatorDistanceOffset) / maxDistance;

        SetSoundParameters(dist);
    }

    private void FindRetranslator()
    {
        SetRetranslator(false);
        
        LayerMask layerMask = LayerMask.GetMask("Retranslator");

        List<Collider> colliders = new List<Collider>();
        colliders.AddRange(Physics.OverlapSphere(transform.position, maxDistance, layerMask));

        for (int i = 0; i < colliders.Count; i++)
        {
            retranslatorList.Add(colliders[i].GetComponent<Retranslator>());
        }

        float lastDist = maxDistance;

        for (int i = 0; i < retranslatorList.Count; i++)
        {
            float dist = Vector3.Distance(retranslatorList[i].transform.position, player.transform.position);

            if (IsInSight(retranslatorList[i].gameObject) && retranslatorList[i].IsPlayerInSight() && dist < lastDist)
            {
                lastDist = dist;
                retranslator = retranslatorList[i];
            }
        }

        retranslatorList.Clear();

        if (retranslator == null) return;

        RuntimeManager.AttachInstanceToGameObject(soundInstance, retranslator.transform);
        SetRetranslator(true);
    }

    public bool IsPlayerRoomInSight()
    {
        if (player.CurrentRoom != null)
        {
            foreach (Portal portal in player.CurrentRoom.PortalList)
            {
                if (Physics.Raycast(transform.position, (portal.transform.position - transform.position).normalized, out RaycastHit hit, Mathf.Infinity))
                {
                    if (hit.collider.CompareTag("Portal"))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private bool IsInSight(GameObject gameObject)
    {
        Vector3 direction = gameObject.transform.position - transform.position;

        if (Physics.Raycast(transform.position, direction.normalized, out RaycastHit hit, Mathf.Infinity))
        {
            GameObject hitGO = hit.collider.gameObject;

            if (hit.collider.CompareTag("Retranslator"))
            {
                if (hitGO.GetComponent<Retranslator>().IsPlayerInSight())
                {
                    return true;
                }

                if (hitGO.GetComponentInParent<Portal>())
                {
                    return true;
                }
            }
        }

        return false;
    }

    protected virtual void OnDestroy()
    {
        soundInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void Play()
    {
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

    protected virtual void Update()
    {
        soundInstance.getParameterByName("RoomFilter", out isFiltered);
        soundInstance.getVolume(out volume);
        soundInstance.getParameterByName("DistanceFilter", out distanceFilter);
    }
}
