using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Killzone : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerStats>(out PlayerStats playerComponent))
        {
            //Destroy(other.gameObject);
            Debug.Log(gameObject.name + "You've died to a killzone");
            Application.Quit();
            SceneManager.LoadScene("AITestingScene 1");
        }
    }
}
