using FMOD;
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

            CreateClonesAtPortals(sound);

            sound.SetInRoom(true, this);
            soundList.Add(sound);
        }

        if (other.CompareTag("Player"))
        {
            foreach (tSoundClone soundClone in portalSoundsList)
            {
                soundClone.StopImmediate();
            }

            foreach (tSoundOriginal soundOriginal in soundList)
            {
                soundOriginal.SetInRoom(false, this);
            }

            TranslateExternalSounds();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("tSound"))
        {
            tSoundOriginal sound = other.GetComponent<tSoundOriginal>();

            if (soundList.Contains(sound))
            {
                DeleteClonesAtPortals(sound);
            }
        }

        if (other.CompareTag("Player"))
        {
            foreach (tSoundOriginal soundOriginal in soundList)
            {
                soundOriginal.SetInRoom(true, this);
            }

            foreach (tSoundClone soundClone in portalSoundsList)
            {
                soundClone.PlayClone();
            }

            ClearExternalSound();
        }
    }

    private void TranslateExternalSounds()
    {
        // �������� ��� tSound � ������������ ������� � ��� ������� ������� ����� �� ������� �������.
        // ����� ������� ����� �������� ������ ���� ��������� ��������� �� ���� �����, ����� 3� �������� ���������
        // �������� ���������/������ �� ���� ��������� ������

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
        // �������� ��� ����� ���� tSound'�� � ������� ��, �������� �������� �������
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
