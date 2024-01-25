using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tSound : MonoBehaviour
{
    [SerializeField]
    [FMODUnity.EventRef]
    private string soundPath;

    [SerializeField]
    [Range(0f, 1f)]
    private float startVolume;

    [SerializeField]
    private float maxDistance;

    private FMOD.Studio.EventInstance soundInstance;
    private Player player;
    private Retranslator retranslator;
    private List<Retranslator> retranslatorList = new List<Retranslator>();

    private void Start()
    {
        soundInstance = FMODUnity.RuntimeManager.CreateInstance(soundPath);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(soundInstance, transform);

        player = FindObjectOfType<Player>();

        retranslatorList.AddRange(FindObjectsOfType<Retranslator>());

        soundInstance.start();
        soundInstance.setVolume(startVolume);
    }

    private void FixedUpdate()
    {
        RaycastHit hit;

        Vector3 direction = player.transform.position - transform.position;

        if (Physics.Raycast(transform.position, transform.TransformDirection(direction), out hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("Player"))
            {
                FMODUnity.RuntimeManager.AttachInstanceToGameObject(soundInstance, transform);
                if (retranslator != null) retranslator.Set(false);

                float volume = 1 - (hit.distance / maxDistance);

                soundInstance.setVolume(Mathf.Clamp01(volume));
                soundInstance.getVolume(out float value);
                Debug.Log("Stright - " + hit.distance + "/" + value);
                Debug.DrawLine(transform.position, player.transform.position, Color.blue);
            }
            else
            {
                FindRetranslator();

                if (retranslator != null && retranslator.SeePlayer)
                {
                    float volume = 1 - ((hit.distance + retranslator.Hit.distance) / maxDistance);

                    soundInstance.setVolume(Mathf.Clamp01(volume));
                    soundInstance.getVolume(out float value);
                    Debug.Log("Retranslator - " + (hit.distance + retranslator.Hit.distance) + "/" + value);
                    Debug.DrawLine(transform.position, retranslator.transform.position, Color.blue);
                }
            }
        }
    }

    private void FindRetranslator()
    {

        if (retranslator != null) retranslator.Set(false);
        float lastDist = maxDistance;

        for (int i = 0; i < retranslatorList.Count; i++)
        {
            float dist = Vector3.Distance(retranslatorList[i].transform.position, player.transform.position);

            if (IsInSight(retranslatorList[i].gameObject) && dist < lastDist)
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
