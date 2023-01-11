using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.UI;

public class StatBar : MonoBehaviour
{
    
    private PlayerStats stats;
    private StarterAssetsInputs inputs;
    
    private GameObject player;
    [SerializeField]private float displayTime;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        stats = player.GetComponent<PlayerStats>();
        inputs = player.GetComponent<StarterAssetsInputs>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inputs.displayStats)
        {
            inputs.displayStats = false;
            StartCoroutine(DisplayStats());
        }
    }

    IEnumerator DisplayStats()
    {
        GetComponent<Toggle>().isOn = true;
        UpdateStats();
        yield return new WaitForSeconds(displayTime);
        GetComponent<Toggle>().isOn = false;
    }

    private void UpdateStats()
    {
        
    }
}
