using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tSoundOriginal : tSound
{
    protected override void Start()
    {
        base.Start();
        soundManager.AddSound(this);
    }

    public void SetInRoom(bool value, Room room)
    {
        if (value) SetRoomFilter(1);
        else SetRoomFilter(0);

        TargetRoom = room;
    }

    public void SetOutside()
    {
        SetRoomFilter(0);
        TargetRoom = null;
    }

    private void SetRoomFilter(float value)
    {
        soundInstance.setParameterByName("RoomFilter", value);
    }

    protected override void OnDestroy()
    {
        soundManager.RemoveSound(this);
        base.OnDestroy();
    }
}
