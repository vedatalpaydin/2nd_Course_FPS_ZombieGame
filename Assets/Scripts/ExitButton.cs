using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitButton : MonoBehaviour
{
    public void ExitScene()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("GameLevel");
    }
}
