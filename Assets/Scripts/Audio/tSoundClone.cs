using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tSoundClone : tSound
{
    public void SetUp(string sPath, float distance, float retrDistanceOffset)
    {
        soundPath = sPath;
        maxDistance = distance / 2;
        retranslatorDistanceOffset = retrDistanceOffset;
    }
}
