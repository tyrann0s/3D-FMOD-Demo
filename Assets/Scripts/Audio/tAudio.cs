using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tAudio : MonoBehaviour
{
    protected Player player;
    public RaycastHit PlayerHit { get; protected set; }

    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    public virtual bool IsPlayerInSight(Vector3 originPos)
    {
        if (Physics.Raycast(originPos, transform.TransformDirection(player.transform.position - originPos), out RaycastHit hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("Player"))
            {
                PlayerHit = hit;
                return true;
            }
        }

        return false;
    }
}
