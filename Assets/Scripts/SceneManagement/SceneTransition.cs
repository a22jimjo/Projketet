using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string sceneName;
    public GameObject player;
    public GameObject spawnPoint;
    public GameObject followCamera;
    public GameObject mainCamera;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        followCamera = GameObject.FindGameObjectWithTag("FollowCamera");
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

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
        yield return new WaitForSeconds(0.5f);
        player.SetActive(false);
        followCamera.SetActive(false);
        mainCamera.SetActive(false);
        spawnPoint = GameObject.FindGameObjectWithTag("PlayerStartPosition");
        player.transform.position = spawnPoint.transform.position;
        player.SetActive(true);
        followCamera.SetActive(true);
        mainCamera.SetActive(true);
        print("Message from portal in last scene");
        Destroy(gameObject);
    }
}