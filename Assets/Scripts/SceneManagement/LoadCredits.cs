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

    // Start is called before the first frame update
    void Start()
    {
        //endCredits = GameObject.FindGameObjectWithTag("EndCredits");
        hud = GameObject.FindGameObjectWithTag("HUD");
        bossMusic = GameObject.FindGameObjectWithTag("BossMusic");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _ThirdPersonController = other.GetComponent<ThirdPersonController>();
            
            StartCoroutine(EndCredit());
        }
    }

    IEnumerator EndCredit()
    {
        //turn off music
        bossMusic.SetActive(false);

        //Locks player movement and plays end credits
        _ThirdPersonController._fixedPosition = true;
        _ThirdPersonController.invincible = true;
        hud.SetActive(false);
        Instantiate(endCredits);
        //endCredits.SetActive(true);

        //wait until video is finished
        yield return new WaitForSeconds(endCreditWaitTime);

        //Respawn player with reseted stats
        _ThirdPersonController.invincible = false;
        _PlayerStats.ResetStats();
        SceneManager.LoadSceneAsync("1_Respawn");
    }
}
