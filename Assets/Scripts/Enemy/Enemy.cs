using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool IsPlayerInSight { get; private set; }

    private BehaviourTree bt;

    private void Start()
    {
        bt = GetComponent<BehaviourTree>();
    }

    public void PlayerInSight(bool value)
    {
        IsPlayerInSight = value;
    }
}
