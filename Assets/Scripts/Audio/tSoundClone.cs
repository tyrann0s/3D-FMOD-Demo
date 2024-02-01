using FMOD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tSoundClone : tSound
{
    public tSoundOriginal OriginalSound { get; private set; }

    public void SetUp(string sPath, float distance, float retrDistanceOffset, tSoundOriginal ogSound)
    {
        StopImmediate();

        soundPath = sPath;
        maxDistance = distance / 2;
        retranslatorDistanceOffset = retrDistanceOffset;
        OriginalSound = ogSound;

        PlayClone();
    }

    public void PlayClone()
    {
        if (OriginalSound != null)
        {
            OriginalSound.SoundInstance.getTimelinePosition(out int position);
            soundInstance.setTimelinePosition(position);
        }

        Play();
    }
}
