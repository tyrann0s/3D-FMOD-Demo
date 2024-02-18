using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eyes : MonoBehaviour
{
    private Enemy enemy;
    private Renderer rend;

    [SerializeField]
    private LayerMask layerMask;

    private bool inTrigger;

    private void Start()
    {
        enemy = GetComponentInParent<Enemy>();
        rend = GetComponent<Renderer>();
        layerMask = ~layerMask;
    }

    private void SetConeColor(Color color)
    {
        color.a = .5f;
        rend.material.color = color;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inTrigger = false;
        }
    }

    private void FixedUpdate()
    {
        if (!Physics.Linecast(transform.position, enemy.Player.transform.position, layerMask) && inTrigger)
        {
            enemy.StopAllCoroutines();
            enemy.PlayerInSight(true);
            SetConeColor(Color.red);
        } else
        {
            StartCoroutine(enemy.WatchingTimeout());
            SetConeColor(Color.white);
        }
    }
}
