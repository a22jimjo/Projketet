using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HurtScreen : MonoBehaviour
{

    private PlayerStats stats;
    
    private GameObject player;
    [SerializeField]private float displayTime;

    private float lastHp;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        stats = player.GetComponent<PlayerStats>();
        lastHp = stats.health;
    }

    // Update is called once per frame
    void Update()
    {
        if (stats.health < lastHp)
        {
            StartCoroutine(TakeDamage());
        }
        lastHp = stats.health;  
    }
    
    private IEnumerator TakeDamage()
    {
        gameObject.GetComponent<Toggle>().isOn = true;
        yield return new WaitForSeconds(displayTime);
        gameObject.GetComponent<Toggle>().isOn = false;
    }
}
