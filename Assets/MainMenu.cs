using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
public void PlayGame ()
    {
        Debug.Log("nu ska scenen laddas");
        SceneManager.LoadScene("2. Forest theme");
    }
}
