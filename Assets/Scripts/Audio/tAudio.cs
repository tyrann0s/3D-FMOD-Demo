using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tAudio : MonoBehaviour
{
    protected Player player;
    public RaycastHit PlayerHit { get; private set; }

    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    public bool IsPlayerInSight()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(player.transform.position - transform.position), out RaycastHit hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("Player"))
            {
                PlayerHit = hit;
                return true;
            }
            else return false;
        }
        else return false;
    }
}
