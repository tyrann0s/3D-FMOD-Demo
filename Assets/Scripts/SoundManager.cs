using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private List<tSoundOriginal> sounds = new List<tSoundOriginal>();

    public Player SMPlayer { get; private set; }

    private void Start()
    {
        SMPlayer = FindObjectOfType<Player>();
    }

    public void AddSound(tSoundOriginal sound)
    {
        sounds.Add(sound);
    }

    public void RemoveSound(tSoundOriginal sound)
    {
        sounds.Remove(sound);
    }

    public void PlayerInRoom()
    {
        foreach (tSoundOriginal sound in sounds)
        {
            if (sound.TargetRoom == null)
            {
                sound.SetInRoom(true, null);
            }
        }
    }

    public void PlayerOutside()
    {
        foreach (tSoundOriginal sound in sounds)
        {
            if (sound.TargetRoom != null)
            {
                sound.SetInRoom(true, sound.TargetRoom);
            }
            else sound.SetOutside();
        }
    }
}
