using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tSoundOriginal : tSound
{
    public void SetInRoom(bool value, Room room)
    {
        if (value)
        {
            soundInstance.setParameterByName("RoomFilter", 1);
            TargetRoom = room;
        }
        else
        {
            soundInstance.setParameterByName("RoomFilter", 0);
            TargetRoom = null;
        }
    }

    public void SetOutside(bool value)
    {
        if (value)
        {
            soundInstance.setParameterByName("RoomFilter", 1);
        }
        else
        {
            soundInstance.setParameterByName("RoomFilter", 0);
        }
    }

    public float GetVolume()
    {
        soundInstance.getVolume(out float result);
        return result;
    }
}
