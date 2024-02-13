using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class tSoundClone : tSound
{
    public tSoundOriginal OriginalSound { get; private set; }

    [Header("Clone Settings")]
    [SerializeField]
    private float distanceDivider;

    [SerializeField]
    private float syncInterval = 0.1f;
    private float timer;

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

        Play();
    }

    protected override void Update()
    {
        base.Update();

        if (OriginalSound != null)
        {
            OriginalSound.SoundInstance.getParameterByName("RoomFilter", out isFilteredOriginal);
            OriginalSound.SoundInstance.getVolume(out volumeOriginal);
            soundInstance.getTimelinePosition(out timeLinePosition);
            OriginalSound.SoundInstance.getTimelinePosition(out originalTimeLinePosition);

            timer += Time.deltaTime;
            if (timer >= syncInterval)
            {
                UpdateTimeLinePosition();
                timer = 0f;
            }
        }
    }

    private void UpdateTimeLinePosition()
    {
        soundInstance.setTimelinePosition(originalTimeLinePosition);
    }

    protected override void SetSoundParameters(float input)
    {
        if (OriginalSound != null)
        {
            if (volume < OriginalSound.GetVolume())
            {
                if (OriginalSound.IsPlayerRoomInSight())
                {
                    float distanceToOriginal = Vector3.Distance(OriginalSound.transform.position, transform.position) / OriginalSound.MaxDistance;
                    lastDistance = distanceToOriginal + input;
                    if (IfSeeingSource(OriginalSound)) base.SetSoundParameters(lastDistance);
                    Debug.DrawLine(transform.position, OriginalSound.transform.position, Color.blue);
                }
                else
                {
                    float distanceToOriginal = Vector3.Distance(OriginalSound.transform.position, transform.position) / OriginalSound.MaxDistance;
                    float distanceToRetranslator = Vector3.Distance(OriginalSound.transform.position, OriginalSound.Retranslator.transform.position) / OriginalSound.MaxDistance;
                    lastDistance = distanceToOriginal + distanceToRetranslator + input;
                    if (IfSeeingSource(OriginalSound.Retranslator)) base.SetSoundParameters(lastDistance);
                    Debug.DrawLine(transform.position, OriginalSound.Retranslator.transform.position, Color.blue);
                }
            }         
        } 
    }

    private bool IfSeeingSource(tAudio source)
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(source.transform.position - transform.position), out RaycastHit hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("Original Sound"))
            {
                return true;
            }

            if (hit.collider.CompareTag("Retranslator"))
            {
                return true;
            }
        }

        return false;
    }
}
