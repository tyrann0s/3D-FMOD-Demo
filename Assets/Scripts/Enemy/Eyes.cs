using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eyes : MonoBehaviour
{
    private Enemy enemy;

    private bool inTrigger;

    private void Start()
    {
        enemy = GetComponentInParent<Enemy>();
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
        if (inTrigger && Physics.Raycast(transform.position, (enemy.Player.GetEyesPosition() - transform.position).normalized, out RaycastHit hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("BoxWall")) return;

            if (hit.collider.CompareTag("Player")) enemy.PlayerInSight(true);
            else StartCoroutine(enemy.WatchingTimeout());
        }
    }
}
