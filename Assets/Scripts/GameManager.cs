using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int targetCollectables;
    private int currentCollectables;

    private void Start()
    {
        targetCollectables = FindObjectsOfType<Collectable>().Length;
    }

    public void DeathSequence()
    {
        StartCoroutine(ReloadScene());
    }

    private IEnumerator ReloadScene()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(0);
    }

    public void GotCollectable()
    {
        currentCollectables++;

        if (currentCollectables >= targetCollectables) WinSequence();
    }

    private void WinSequence()
    {
        Debug.Log("WIN");
        StartCoroutine(ReloadScene());
    }
}
