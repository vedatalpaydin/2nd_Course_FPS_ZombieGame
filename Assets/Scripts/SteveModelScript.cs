using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SteveModelScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("SwitchMainMenu",5f);
    }

    void SwitchMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
