using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Retranslator : MonoBehaviour
{
    private bool hasSound = false;
    private Player player;

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
            RaycastHit hit;
            Vector3 direction = player.GetEyesPosition() - transform.position;

            if (Physics.Raycast(transform.position, transform.TransformDirection(direction), out hit, Mathf.Infinity))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    Hit = hit;
                    SeePlayer = true;
                    Debug.DrawLine(transform.position, player.GetEyesPosition(), Color.blue);
                } else SeePlayer = false;
            }
        }
    }

    public void Set(bool value)
    {
        hasSound = value;
    }
}
