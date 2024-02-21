using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Text collectablesText, announcmentText;

    [SerializeField]
    private Image fadeImage;

    [SerializeField]
    private StudioEventEmitter winSound;

    private int targetCollectables;
    private int currentCollectables;

    private bool isFade;
    private float fadeTimer;

    private void Start()
    {
        targetCollectables = FindObjectsOfType<Collectable>().Length;
        SetCollectablesText();
        StartCoroutine(ShowAnnouncementText("COLLECT ALL ITEMS"));
    }

    public void DeathSequence()
    {
        StartCoroutine(ShowAnnouncementText("YOU DIED"));
        StartCoroutine(ReloadScene());
    }

    private IEnumerator ReloadScene()
    {
        isFade = true;
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(0);
    }

    public void GotCollectable()
    {
        currentCollectables++;
        SetCollectablesText();

        if (currentCollectables >= targetCollectables) WinSequence();
    }

    private void SetCollectablesText()
    {
        collectablesText.text = currentCollectables.ToString() + "/" + targetCollectables.ToString();
    }

    private void WinSequence()
    {
        winSound.Play();
        StartCoroutine(ShowAnnouncementText("YOU WIN"));
        StartCoroutine(ReloadScene());
    }

    private IEnumerator ShowAnnouncementText(string text)
    {
        announcmentText.text = text;
        announcmentText.gameObject.SetActive(true);

        yield return new WaitForSeconds(3);
        announcmentText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isFade)
        { 
            fadeTimer += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, fadeTimer / 1);

            fadeImage.color = new Color(0, 0, 0, alpha);
        }
    }
}
