using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tSoundOriginal : tSound
{
    public void SetInRoom(bool value)
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
}
