using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private List<tSound> sounds = new List<tSound>();
    public List<tSoundOriginal> OriginalSounds { get; private set; } = new List<tSoundOriginal>();

    public Player SMPlayer { get; private set; }

    private void Start()
    {
        SMPlayer = FindObjectOfType<Player>();
    }

    public void AddSound(tSound sound)
    {
        sounds.Add(sound);

        if (sound.GetType() == typeof(tSoundOriginal)) OriginalSounds.Add((tSoundOriginal)sound);
    }

    public void RemoveSound(tSound sound)
    {
        sounds.Remove(sound);
    }

    public void PlayerInRoom(Room room)
    {
        foreach (tSoundOriginal sound in OriginalSounds)
        {
            if (sound.TargetRoom == null)
            {
                sound.SetInRoom(true, null);
            }
        }
    }

    public void PlayerOutside()
    {
        foreach (tSoundOriginal sound in OriginalSounds)
        {
            if (sound.TargetRoom != null)
            {
                sound.SetInRoom(true, sound.TargetRoom);
            }
        }
    }
}
