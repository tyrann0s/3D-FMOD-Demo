using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tAudio : MonoBehaviour
{
    protected Player player;
    public RaycastHit PlayerHit { get; protected set; }

    [SerializeField]
    private bool isPlayerInSight;

    [SerializeField]
    private string hittedObject;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    public virtual bool IsPlayerInSight()
    {
        if (Physics.Raycast(transform.position, (player.GetEyesPosition() - transform.position).normalized, out RaycastHit hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("Player"))
            {
                PlayerHit = hit;
                isPlayerInSight = true;

                hittedObject = hit.collider.tag.ToString() + "/" + hit.collider.gameObject.name;
                return true;
            }
            
            hittedObject = hit.collider.tag.ToString() + "/" + hit.collider.gameObject.name;
        }

        isPlayerInSight = false;
        return false;
    }
}
