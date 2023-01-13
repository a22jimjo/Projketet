using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadCredits : MonoBehaviour
{

    [SerializeField] private GameObject endCredits;
    [SerializeField] private float endCreditWaitTime;
    private ThirdPersonController _ThirdPersonController;
    private PlayerStats _PlayerStats;
    private GameObject bossMusic;

    private GameObject hud;

    private GameObject enemyHolder;
    public List<GameObject> enemyGameobjects = new List<GameObject>();


    public string sceneName;
    public GameObject player;
    public GameObject spawnPoint;
    [SerializeField] private Canvas fadeCanvas;

    // Start is called before the first frame update
    void Start()
    {
        //endCredits = GameObject.FindGameObjectWithTag("EndCredits");
        hud = GameObject.FindGameObjectWithTag("HUD");
        bossMusic = GameObject.FindGameObjectWithTag("BossMusic");

        enemyHolder = GameObject.FindGameObjectWithTag("EnemyHolder");
        player = GameObject.FindGameObjectWithTag("Player");



    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        FindAllEnemies();

        if (other.gameObject.CompareTag("Player") && enemyGameobjects.Count <= 0)
        {
            _ThirdPersonController = other.GetComponent<ThirdPersonController>();
            
            StartCoroutine(EndCredit());
        }
    }

    IEnumerator EndCredit()
    {
        DontDestroyOnLoad(gameObject);


        //turn off music
        bossMusic.SetActive(false);

        //Locks player movement and plays end credits
        _ThirdPersonController._fixedPosition = true;
        _ThirdPersonController.invincible = true;

        //starts video and turns off hud
        hud.SetActive(false);
        Instantiate(endCredits);

        //wait until video is finished
        yield return new WaitForSeconds(endCreditWaitTime);
        Application.Quit();
    }


    private void FindAllEnemies()
    {
        Transform[] objectsInRoom = enemyHolder.GetComponentsInChildren<Transform>();

        if (enemyHolder != null)
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
