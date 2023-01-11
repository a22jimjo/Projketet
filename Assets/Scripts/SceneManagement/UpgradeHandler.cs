using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class UpgradeHandler : MonoBehaviour
{
    [SerializeField] private int healing = 25;
    [SerializeField] private int maxHealthIncrease = 10;
    [SerializeField] private float damageModifier = 1.05f;
    [SerializeField] private float defenceModifier = 1.05f;
    [SerializeField] private float speedModifier = 1.10f;
    

    private GameObject player;
    private GameObject UpgradeScreen;
    private PlayerStats _playerStats;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        UpgradeScreen = GameObject.FindGameObjectWithTag("UpgradeScreen");
        UpgradeScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EnableScreen()
    {
        UpgradeScreen.SetActive(true);
        player.GetComponent<ThirdPersonController>()._fixedPosition = true;
        player.GetComponent<ThirdPersonController>().invincible = true;
    }


    public void RestoreHealth()
    {
        player.TryGetComponent<PlayerStats>(out PlayerStats stats);

        if (stats.health + healing <= stats.maxHealth)
        {
            stats.health += healing;
            stats._healthbar.UpdateHealthBar(stats.maxHealth, stats.health);
            
        }
        else
        {
            stats.health = stats.maxHealth;
            stats._healthbar.UpdateHealthBar(stats.maxHealth, stats.health);
        }
        FreePlayer();
    }

    public void IncreaseMaxHealth()
    {
        player.TryGetComponent<PlayerStats>(out PlayerStats stats);
        stats.maxHealth += maxHealthIncrease;
        stats.health += maxHealthIncrease;
        stats._healthbar.UpdateHealthBar(stats.maxHealth, stats.health);
        FreePlayer();
    }

    public void IncreaseAttack()
    {
        player.TryGetComponent<PlayerStats>(out PlayerStats stats);

        stats.damage *= damageModifier;
        stats.heavyDamage *= damageModifier;
        
        FreePlayer();
    }

    public void IncreaseDefence()
    {
        player.TryGetComponent<PlayerStats>(out PlayerStats stats);
        stats.defenceModifier *= defenceModifier;
        FreePlayer();
    }
    

    public void IncreaseMoveSpeed()
    {
        player.TryGetComponent <ThirdPersonController>(out ThirdPersonController controllerStats);
        controllerStats.MoveSpeed *= speedModifier;
        FreePlayer();
    }

    public void FourtuneCookie()
    {
        //Open panel that shows flavourtext. Closed opon anyinput

        int tempINT = Random.Range(1, 5);

        switch (tempINT)
        {
            case 1: Debug.Log("1");
                IncreaseAttack();
                break;

            case 2:
                Debug.Log("2");
                IncreaseDefence();
                break;

            case 3:
                Debug.Log("3");
                IncreaseMoveSpeed();
                    break;
            case 4:
                Debug.Log("4");
                IncreaseMaxHealth();
                break;
            case 5:
                Debug.Log("5");
                RestoreHealth();
                break;
        }
    }

    private void FreePlayer()
    {
        UpgradeScreen.SetActive(false);
        player.GetComponent<ThirdPersonController>()._fixedPosition = false;
        player.GetComponent<ThirdPersonController>().invincible = false;
        player.GetComponent<ThirdPersonController>().ClearInputs();
    }
}
