using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Retranslator : tAudio
{
    private bool hasSound = false;

    public RaycastHit Hit {  get; private set; }
    public bool SeePlayer { get; private set; }

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    private void FixedUpdate()
    {
        if (hasSound)
        {
            if (IsPlayerInSight())
            {
                SeePlayer = true;
            }
            else SeePlayer = false;
        }
    }

    public void Set(bool value)
    {
        hasSound = value;
    }
}
