using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string sceneName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            LoadScene();
        }
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}