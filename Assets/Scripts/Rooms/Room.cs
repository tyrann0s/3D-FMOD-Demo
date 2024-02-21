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
    private List<tSoundClone> portalSoundsList = new List<tSoundClone>();

    private Player player;

    private void Start()
    {
        portalList.AddRange(GetComponentsInChildren<Portal>());

        BoxCollider collider = GetComponent<BoxCollider>();
        collider.size = new Vector3(collider.size.x + .1f, collider.size.y + .1f, collider.size.z + .1f);

        player = FindObjectOfType<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Original Sound"))
        {
            tSoundOriginal sound = other.GetComponent<tSoundOriginal>();

            if (player.CurrentRoom == this)
            {
                sound.SetInRoom(false, this);
            } else
            { 
                CreateClonesAtPortals(sound);

                sound.SetInRoom(true, this);
            }

            soundList.Add(sound);
        }

        if (other.CompareTag("Player"))
        {
            foreach (tSoundClone soundClone in portalSoundsList)
            {
                soundClone.StopImmediate();
            }

            SetSoundsInRoom(false);

            TranslateExternalSounds();

            other.GetComponent<Player>().SetRoom(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Original Sound"))
        {
            tSoundOriginal sound = other.GetComponent<tSoundOriginal>();

            if (soundList.Contains(sound))
            {
                DeleteClonesAtPortals(sound);
            }
        }

        if (other.CompareTag("Player"))
        {
            SetSoundsInRoom(true);

            foreach (tSoundClone soundClone in portalSoundsList)
            {
                soundClone.PlayClone();
            }

            ClearExternalSound();

            other.GetComponent<Player>().SetRoom(null);
        }
    }

    private void SetSoundsInRoom(bool value)
    {
        foreach (tSoundOriginal soundOriginal in soundList)
        {
            soundOriginal.SetInRoom(value, this);
        }
    }

    private void TranslateExternalSounds()
    {
        // Собираем все tSound в определенном радиусе и для каждого создаем копию на каждойм портале.
        // Важно сделать чтобы оригинал каждый кадр передавал настройки на свои клоны, чтобы 3д работало адекватно
        // Оригинал отключаем/ставим на него комнатный фильтр

        foreach (tSoundOriginal soundOriginal in FindObjectsOfType<tSoundOriginal>())
        {
            if (soundOriginal.TargetRoom == null)
            {
                CreateClonesAtPortals(soundOriginal);
                soundOriginal.SetOutside(true);
            }
        }
    }

    private void CreateClonesAtPortals(tSoundOriginal soundOriginal)
    {
        foreach (Portal portal in portalList)
        {
            GameObject go = Instantiate(tSoundPrefab);
            go.transform.position = portal.transform.position;

            tSoundClone tSoundClone = go.GetComponent<tSoundClone>();
            tSoundClone.SetUp(soundOriginal);
            portalSoundsList.Add(tSoundClone);
        }
    }

    private void ClearExternalSound()
    {
        // Собираем все клоны всех tSound'ов и удаляем их, включаем оригинал обратно
        foreach (tSoundOriginal soundOriginal in FindObjectsOfType<tSoundOriginal>())
        {
            if (soundOriginal.TargetRoom == null)
            {
                DeleteClonesAtPortals(soundOriginal);
                soundOriginal.SetOutside(false);
            }
        }
    }

    private void DeleteClonesAtPortals(tSoundOriginal soundOriginal)
    {
        List<tSoundClone> sounds2Delete = new List<tSoundClone>();

        foreach (tSoundClone soundClone in portalSoundsList)
        {
            sounds2Delete.Add(soundClone);
        }

        foreach (tSoundClone soundClone in sounds2Delete)
        {
            if (soundClone.OriginalSound == soundOriginal)
            {
                soundClone.StopImmediate();
                portalSoundsList.Remove(soundClone);
                Destroy(soundClone.gameObject);
            }
        }

        soundOriginal.SetInRoom(false, this);
        soundList.Remove(soundOriginal);
    }
}
