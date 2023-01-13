using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Killzone : MonoBehaviour
{
    public string sceneName;
    public GameObject player;
    public GameObject spawnPoint;
    private Collider boxCollider;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        boxCollider = gameObject.GetComponent<BoxCollider>();
        spawnPoint = GameObject.FindGameObjectWithTag("PlayerStartPosition");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerStats>(out PlayerStats playerComponent))
        {
            Debug.Log(gameObject.name + "You've died to a killzone");
            playerComponent.health = playerComponent.maxHealth;
            playerComponent._healthbar.UpdateHealthBar(playerComponent.maxHealth, playerComponent.health);
            StartCoroutine(RestartGame());
        }
    }


    private IEnumerator RestartGame()
    {
        yield return null;
        DontDestroyOnLoad(gameObject);
        boxCollider.gameObject.SetActive(false);
        SceneManager.LoadSceneAsync("1_Respawn");
        player.SetActive(false);
        spawnPoint = GameObject.FindGameObjectWithTag("PlayerStartPosition");
        player.transform.position = spawnPoint.transform.position;
        player.SetActive(true);
        yield return new WaitForSeconds(1.25f);
        print("Player should have respawned at restartScene :)");
        Destroy(gameObject);
    }
}
