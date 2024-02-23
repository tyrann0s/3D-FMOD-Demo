using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rain : MonoBehaviour
{
    [SerializeField]
    private List<ParticleSystem> pSystemList = new List<ParticleSystem>();

    private Player player;

    private float currentSize;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        transform.rotation = player.transform.rotation;

        if (player.CurrentRoom == null)
        {
            transform.position = player.transform.position;
            
            if (currentSize != 5) SetVFXShape(5);
        }
        else if (currentSize != 1) SetVFXShape(1);
    }

    private void SetVFXShape(float value)
    {
        foreach (ParticleSystem vfx in pSystemList)
        {
            var shape = vfx.shape;
            shape.scale = new Vector3(value, 1, value);

            vfx.Play();
        }

        currentSize = value;
    }
}
