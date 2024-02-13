using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tAudio : MonoBehaviour
{
    protected Player player;
    public RaycastHit PlayerHit { get; protected set; }

    [SerializeField]
    private LayerMask portalPlayerLayer;

    [SerializeField]
    private bool isPlayerInSight;

    [SerializeField]
    private string hittedObject;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    public virtual bool IsPlayerInSight(Vector3 originPos)
    {
        if (portalPlayerLayer == LayerMask.GetMask("Player"))
        {
            if (Physics.Raycast(originPos, transform.TransformDirection(player.transform.position - originPos), out RaycastHit hit, Mathf.Infinity, portalPlayerLayer))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    PlayerHit = hit;
                    isPlayerInSight = true;

                    return true;
                }
                else hittedObject = hit.collider.tag.ToString() + "/" + hit.collider.gameObject.name;
            }
        } else
        {
            if (Physics.Raycast(originPos, transform.TransformDirection(player.transform.position - originPos), out RaycastHit hit, Mathf.Infinity))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    PlayerHit = hit;
                    isPlayerInSight = true;

                    return true;
                }
                else hittedObject = hit.collider.tag.ToString() + "/" + hit.collider.gameObject.name;
            }
        }
        

        isPlayerInSight = false;
        return false;
    }
}
