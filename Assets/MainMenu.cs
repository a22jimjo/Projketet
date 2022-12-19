using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public void PlayGame ()
    {
        Debug.Log("Big Black Cock");
        SceneManager.LoadScene("2. Forest theme");
    }

    public void QuitGame ()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
