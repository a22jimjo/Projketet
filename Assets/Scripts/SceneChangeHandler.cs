using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeHandler : MonoBehaviour
{
    void Start()
    {
        // Register the OnSceneLoaded event to run the specified method
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // This method will be called every time a new scene is loaded
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Do something here, such as logging the name of the loaded scene
        Debug.Log($"Scene loaded: {scene.name}");

        GameObject player;
        Vector3 playerStartPos = new Vector3(0,0.5f,0);

        player = GameObject.FindGameObjectWithTag("Player");

        player.transform.position = playerStartPos;
    }
}