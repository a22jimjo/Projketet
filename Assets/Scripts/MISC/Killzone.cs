using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Killzone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerStats>(out PlayerStats playerComponent))
        {
            //Destroy(other.gameObject);
            Debug.Log(gameObject.name + "You've died to a killzone");
            Application.Quit();
            //SceneManager.LoadScene(SceneChangeHandler.currentSceneName);
            SceneManager.LoadScene("2. Forest theme");
        }
    }
}
