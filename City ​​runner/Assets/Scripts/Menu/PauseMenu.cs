using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] GameObject pauseMenu;
    public GameObject BGM;

    public void Pause()
    {
        pauseMenu.SetActive(true);
        BGM.GetComponent<AudioSource>().volume = 0.2f;
        BGM.GetComponent<AudioSource>().pitch = 1.1f;
        Time.timeScale = 0;
    }

    public void Home()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        BGM.GetComponent<AudioSource>().volume = 0.4f;
        BGM.GetComponent<AudioSource>().pitch = 1.0f;
        Time.timeScale = 1;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    
}
