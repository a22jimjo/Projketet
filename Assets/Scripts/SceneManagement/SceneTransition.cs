using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SceneTransition : MonoBehaviour
{
    public string sceneName;
    public GameObject player;
    public GameObject spawnPoint;
    private GameObject enemyHolder;
    public List<GameObject> enemyGameobjects = new List<GameObject>();


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemyHolder = GameObject.FindGameObjectWithTag("EnemyHolder");
    }

    private void Update()
    {

        FindAllEnemies();

        if (enemyGameobjects.Count <= 0)
        {
            //change vfx to active.

        }
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Portal entered");
        if (other.gameObject.tag == "Player" && enemyGameobjects.Count <= 0)
        {
            StartCoroutine(Transition());
        }
        else if(other.gameObject.tag == "Player")
        {
            Debug.Log("enemies must be cleared to enter");

        }
    }

    private IEnumerator Transition()
    {
        DontDestroyOnLoad(gameObject);
        yield return SceneManager.LoadSceneAsync(sceneName);
        yield return new WaitForSeconds(0.5f);
        player.SetActive(false);
        spawnPoint = GameObject.FindGameObjectWithTag("PlayerStartPosition");
        player.transform.position = spawnPoint.transform.position;
        player.SetActive(true);
        print("Message from portal in last scene");
        Destroy(gameObject);
    }

    private void FindAllEnemies()
    {
        Transform[] objectsInRoom = enemyHolder.GetComponentsInChildren<Transform>();
        enemyGameobjects.Clear();
        for (int i = 0; i < objectsInRoom.Length; i++)
        {
            if (objectsInRoom[i].CompareTag("Enemy"))
            {
                enemyGameobjects.Add(objectsInRoom[i].gameObject);
            }
        }
    }
}