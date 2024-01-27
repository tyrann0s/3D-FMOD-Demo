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
    [Range(0f, 1f)]
    private float startVolume;

    [SerializeField]
    protected float maxDistance, retranslatorDistanceOffset;
    public float MaxDistance => maxDistance;
    public float RetranslatorDistanceOffset => retranslatorDistanceOffset;

    protected FMOD.Studio.EventInstance soundInstance;

    private Retranslator retranslator;
    private List<Retranslator> retranslatorList = new List<Retranslator>();
    
    protected virtual void Start()
    {
        soundInstance = FMODUnity.RuntimeManager.CreateInstance(soundPath);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(soundInstance, transform);

        retranslatorList.AddRange(FindObjectsOfType<Retranslator>());

        soundInstance.start();
        soundInstance.setVolume(startVolume);
    }

    private void FixedUpdate()
    {
        if (IsPlayerInSight())
        {
            if (PlayerHit.collider.CompareTag("Player"))
            {
                FMODUnity.RuntimeManager.AttachInstanceToGameObject(soundInstance, transform);
                if (retranslator != null) retranslator.Set(false);

                float dist = PlayerHit.distance / maxDistance;

                SetSoundParameters(dist);

                soundInstance.getVolume(out float value);
                Debug.Log(gameObject.name + " - " + PlayerHit.distance + "/" + value);
                Debug.DrawLine(transform.position, player.transform.position, Color.blue);
            }
        }
        else
        {
            FindRetranslator();
            if (retranslator == null) return;

            Physics.Raycast(transform.position, transform.TransformDirection(retranslator.transform.position - transform.position), out RaycastHit hit, Mathf.Infinity);
            float dist = (hit.distance + retranslator.PlayerHit.distance + retranslatorDistanceOffset) / maxDistance;

            SetSoundParameters(dist);

            soundInstance.getVolume(out float value);
            Debug.Log(retranslator.gameObject.name + " - " + (hit.distance + retranslator.PlayerHit.distance + retranslatorDistanceOffset) + "/" + value);
            Debug.DrawLine(transform.position, retranslator.transform.position, Color.blue);
        }
    }

    private void SetSoundParameters(float input)
    { 
        float volume = 1 - input;
        soundInstance.setVolume(Mathf.Clamp01(volume));
        soundInstance.setParameterByName("DistanceFilter", volume);
    }

    private void FindRetranslator()
    {
        if (retranslator != null) retranslator.Set(false);
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

        if (retranslator == null) return;

        FMODUnity.RuntimeManager.AttachInstanceToGameObject(soundInstance, retranslator.transform);
        retranslator.Set(true);
    }

    private bool IsInSight(GameObject gameObject)
    {
        RaycastHit hit;
        Vector3 direction = gameObject.transform.position - transform.position;

        if (Physics.Raycast(transform.position, transform.TransformDirection(direction), out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject == gameObject && hit.collider.CompareTag("Retranslator"))
            {
                return true;
            } else return false;
        }

        return false;
    }

    private void OnDestroy()
    {
        soundInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
