using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private List<Portal> portalList = new List<Portal>();

    private void Start()
    {
        portalList.AddRange(GetComponentsInChildren<Portal>());
    }
}
