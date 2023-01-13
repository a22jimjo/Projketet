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

    private GameObject hud;

    // Start is called before the first frame update
    void Start()
    {
        //endCredits = GameObject.FindGameObjectWithTag("EndCredits");
        hud = GameObject.FindGameObjectWithTag("HUD");
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
        _ThirdPersonController._fixedPosition = true;
        _ThirdPersonController.invincible = true;
        hud.SetActive(false);
        Instantiate(endCredits);
        //endCredits.SetActive(true);
        yield return new WaitForSeconds(endCreditWaitTime);
        SceneManager.LoadSceneAsync("1. Respawn");
        _ThirdPersonController.invincible = false;

    }
}
