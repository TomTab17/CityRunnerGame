using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndRunSequence : MonoBehaviour
{
    public GameObject liveCoins;
    public GameObject liveDist;
    public GameObject endScreen;
    public AudioSource gameOverSFX;
    public AudioSource BGM;
    public AudioSource ambienceSFX;

    void Start()
    {
        StartCoroutine(EndSequence());
    }

    IEnumerator EndSequence()
    {
        yield return new WaitForSeconds(0.1f);
        BGM.Stop();
        ambienceSFX.Play();
        yield return new WaitForSeconds(2);
        gameOverSFX.Play();
        liveCoins.SetActive(false);
        liveDist.SetActive(false);
        endScreen.SetActive(true);
    }
}
