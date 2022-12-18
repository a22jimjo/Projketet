using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string sceneName;
    public GameObject player;
    public GameObject spawnPoint;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");


    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Portal entered");
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(Transition());
        }
    }

    private IEnumerator Transition()
    {
        DontDestroyOnLoad(gameObject);
        yield return SceneManager.LoadSceneAsync(sceneName);
        player.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        spawnPoint = GameObject.FindGameObjectWithTag("PlayerStartPosition");
        player.transform.position = spawnPoint.transform.position;
        player.SetActive(true);
        print("Message from portal in last scene");
        Destroy(gameObject);
    }
}