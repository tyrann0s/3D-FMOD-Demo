using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySounds : MonoBehaviour
{
    [SerializeField]
    private tSoundOriginal fStepWalk;

    [SerializeField]
    private tSoundOriginal fStepRun;

    [SerializeField]
    private tSoundOriginal creepyVO;

    [SerializeField]
    private tSoundOriginal killPlayer;

    public void PlayFStepWalk()
    {
        fStepWalk.Play();
    }

    public void PlayFStepRun()
    {
        fStepRun.Play();
    }

    public void PlayCreepyVO()
    {
        creepyVO.Play();
    }

    public void PlayKillPlayer()
    {
        killPlayer.Play();
    }
}
