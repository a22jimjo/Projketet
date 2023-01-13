using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    public string sceneName;
    public GameObject player;
    public GameObject spawnPoint;
    private GameObject enemyHolder;
    public List<GameObject> enemyGameobjects = new List<GameObject>();
    [SerializeField] private bool isEndPortal;
    [SerializeField] private Canvas fadeCanvas;
    private Animator animator;

    private UpgradeHandler upgradeHandler;
    private ThirdPersonController _thirdPersonController;
    private GameObject gameManager;
    private PersistentObjects persistentObjects;



    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemyHolder = GameObject.FindGameObjectWithTag("EnemyHolder");
        animator = fadeCanvas.GetComponent<Animator>();

        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        upgradeHandler = gameManager.GetComponent<UpgradeHandler>();
    }

    private void Update()
    {

        if (enemyGameobjects.Count <= 0)
        {
            //change vfx to active.

        }
    }


    private void OnTriggerEnter(Collider other)
    {
        FindAllEnemies();
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

        upgradeHandler.EnableScreen();

        yield return new WaitForSeconds(0.5f);
        yield return SceneManager.LoadSceneAsync(sceneName);
        player.SetActive(false);
        spawnPoint = GameObject.FindGameObjectWithTag("PlayerStartPosition");
        player.transform.position = spawnPoint.transform.position;
        player.SetActive(true);
        yield return new WaitForSeconds(1.25f);

        print("Message from portal in last scene");
        if (isEndPortal)
        {
            persistentObjects.DestroyPersistentObjects();
        }
        Destroy(gameObject);
    }


    private void FindAllEnemies()
    {
        Transform[] objectsInRoom = enemyHolder.GetComponentsInChildren<Transform>();

        if(enemyHolder != null)
        {
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
}