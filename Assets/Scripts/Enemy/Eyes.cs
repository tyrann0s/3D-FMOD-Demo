using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eyes : MonoBehaviour
{
    private Enemy enemy;
    private Renderer rend;

    private void Start()
    {
        enemy = GetComponentInParent<Enemy>();
        rend = GetComponent<Renderer>();
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
            enemy.PlayerInSight(true);
            SetConeColor(Color.red);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemy.PlayerInSight(false);
            SetConeColor(Color.white);
        }
    }
}
