using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField]
    private GameObject tSoundPrefab;
    private List<Portal> portalList = new List<Portal>();
    private List<tSoundOriginal> soundList = new List<tSoundOriginal>();
    
    private List<tSoundClone> portalSoundsList = new List<tSoundClone>();

    private void Start()
    {
        portalList.AddRange(GetComponentsInChildren<Portal>());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("tSound"))
        {
            tSoundOriginal sound = other.GetComponent<tSoundOriginal>();

            foreach (Portal portal in portalList)
            {
                GameObject go = Instantiate(tSoundPrefab);
                go.transform.position = portal.transform.position;

                tSoundClone tSoundClone = go.GetComponent<tSoundClone>();
                tSoundClone.SetUp(sound.SoundPath, sound.MaxDistance, sound.RetranslatorDistanceOffset);
            }

            sound.SetInRoom(true);
            soundList.Add(sound);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("tSound"))
        {
            tSoundOriginal sound = other.GetComponent<tSoundOriginal>();

            if (soundList.Contains(sound))
            {
                sound.SetInRoom(false);
                soundList.Remove(sound);
            }
        }
    }
}
