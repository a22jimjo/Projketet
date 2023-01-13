using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeHandler : MonoBehaviour
{

    public string currentSceneName;


    void Start()
    {
        // Register the OnSceneLoaded event to run the specified method
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // This method will be called every time a new scene is loaded
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Do something here, such as logging the name of the loaded scene
        //gets the current scene and saves it as a string
       string currentSceneName = SceneManager.GetActiveScene().name;

        GameObject player;


        player = GameObject.FindGameObjectWithTag("Player");
    }
}