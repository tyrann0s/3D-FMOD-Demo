using FMOD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField]
    private GameObject tSoundPrefab;
    private List<Portal> portalList = new List<Portal>();
    public List<Portal> PortalList => portalList;

    private List<tSoundOriginal> soundList = new List<tSoundOriginal>();

    private SoundManager soundManager;

    private void Start()
    {
        portalList.AddRange(GetComponentsInChildren<Portal>());

        BoxCollider collider = GetComponent<BoxCollider>();
        collider.size = new Vector3(collider.size.x + .1f, collider.size.y + .1f, collider.size.z + .1f);

        soundManager = FindObjectOfType<SoundManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Original Sound"))
        {
            tSoundOriginal sound = other.GetComponent<tSoundOriginal>();

            if (soundManager.SMPlayer.CurrentRoom == this) sound.SetInRoom(false, this);
            else sound.SetInRoom(true, this);

            soundList.Add(sound);
        }

        if (other.CompareTag("Player"))
        {
            soundManager.SMPlayer.SetRoom(this);
            soundManager.PlayerInRoom();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Original Sound"))
        {
            tSoundOriginal sound = other.GetComponent<tSoundOriginal>();

            if (soundManager.SMPlayer.CurrentRoom == null) sound.SetOutside();
            else sound.SetInRoom(true, null);
        }

        if (other.CompareTag("Player"))
        {
            soundManager.SMPlayer.SetRoom(null);
            soundManager.PlayerOutside();
        }
    }
}
