using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField]
    private StudioEventEmitter grabSound, idleSound;

    private void Start()
    {
        idleSound.Play();
    }

    void Update()
    {
        transform.Rotate(0, 1, 0.1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindObjectOfType<GameManager>().GotCollectable();
            grabSound.Play();
            Destroy(gameObject);
        }
    }
}
