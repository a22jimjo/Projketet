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
        freePlayer();
    }

    public void IncreaseMaxHealth()
    {
        player.TryGetComponent<PlayerStats>(out PlayerStats stats);
        stats.maxHealth += maxHealthIncrease;
        stats.health += maxHealthIncrease;
        stats._healthbar.UpdateHealthBar(stats.maxHealth, stats.health);
        freePlayer();
    }

    public void IncreaseAttack()
    {
        player.TryGetComponent<PlayerStats>(out PlayerStats stats);

        stats.damage *= damageModifier;
        stats.heavyDamage *= damageModifier;
        
        freePlayer();
    }

    public void IncreaseDefence()
    {
        player.TryGetComponent<PlayerStats>(out PlayerStats stats);
        stats.defenceModifier *= defenceModifier;
        freePlayer();
    }
    

    public void increaseMoveSpeed()
    {
        player.TryGetComponent <ThirdPersonController>(out ThirdPersonController controllerStats);
        controllerStats.MoveSpeed *= speedModifier;
        freePlayer();
    }

    private void freePlayer()
    {
        UpgradeScreen.SetActive(false);
        player.GetComponent<ThirdPersonController>()._fixedPosition = false;
        player.GetComponent<ThirdPersonController>().invincible = false;
        player.GetComponent<ThirdPersonController>().input.attack = false;
    }
}
