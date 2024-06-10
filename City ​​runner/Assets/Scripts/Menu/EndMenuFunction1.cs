using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenuFunction : MonoBehaviour
{

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
