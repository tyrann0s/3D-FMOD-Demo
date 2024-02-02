using FMOD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tSoundClone : tSound
{
    public tSoundOriginal OriginalSound { get; private set; }

    public void SetUp(tSoundOriginal ogSound)
    {
        StopImmediate();

        soundPath = ogSound.SoundPath;
        retranslatorDistanceOffset = ogSound.RetranslatorDistanceOffset;
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

    private void Update()
    {
        if (OriginalSound != null)
        {
            maxDistance = OriginalSound.DistanceToPlayer();
        }
    }
}
