using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tSoundClone : tSound
{
    public tSoundOriginal OriginalSound { get; private set; }

    [Header("Clone Settings")]
    [SerializeField]
    private float distanceDivider;

    [Header("Clone Debug")]
    [SerializeField]
    private float isFilteredOriginal;
    [SerializeField]
    private float volumeOriginal;
    [SerializeField]
    private int timeLinePosition;
    [SerializeField]
    private int originalTimeLinePosition;

    float lastDistance = 0;

    public void SetUp(tSoundOriginal ogSound)
    {
        StopImmediate();

        soundPath = ogSound.SoundPath;
        maxDistance = ogSound.MaxDistance;
        retranslatorDistanceOffset = ogSound.RetranslatorDistanceOffset;
        OriginalSound = ogSound;

        PlayClone();
    }

    public void PlayClone()
    {
        OriginalSound.SoundInstance.getTimelinePosition(out int position);

        Play(position);
    }

    protected override void Update()
    {
        base.Update();

        if (OriginalSound != null)
        {
            
            //Debug.Log(distanceToOriginal);
            //SetSoundParameters(distanceToOriginal);

            OriginalSound.SoundInstance.getParameterByName("RoomFilter", out isFilteredOriginal);
            OriginalSound.SoundInstance.getVolume(out volumeOriginal);
            soundInstance.getTimelinePosition(out timeLinePosition);
            OriginalSound.SoundInstance.getTimelinePosition(out originalTimeLinePosition);

        }
    }

    public override void SetSoundParameters(float input)
    {
        Debug.Log(input);
        if (OriginalSound != null)
        {
            if (volume < OriginalSound.GetVolume())
            {
                lastDistance = (Vector3.Distance(OriginalSound.transform.position, transform.position) / OriginalSound.MaxDistance) + input;
                base.SetSoundParameters(lastDistance);
                Debug.Log("SOOOOQAAAAAA " + lastDistance);
            }
                
        } 
    }
}
