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

    private float fortuneBonus = 1.1f;
    

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


    public void RestoreHealth(float bonus)
    {
        player.TryGetComponent<PlayerStats>(out PlayerStats stats);

        if (stats.health + healing * bonus <= stats.maxHealth)
        {
            stats.health += healing * bonus;
            stats._healthbar.UpdateHealthBar(stats.maxHealth, stats.health);
            
        }
        else
        {
            stats.health = stats.maxHealth;
            stats._healthbar.UpdateHealthBar(stats.maxHealth, stats.health);
        }
        FreePlayer();
    }

    public void IncreaseMaxHealth(float bonus)
    {
        player.TryGetComponent<PlayerStats>(out PlayerStats stats);
        stats.maxHealth += maxHealthIncrease * bonus;
        stats.health += maxHealthIncrease * bonus;
        stats._healthbar.UpdateHealthBar(stats.maxHealth, stats.health);
        FreePlayer();
    }

    public void IncreaseAttack(float bonus)
    {
        player.TryGetComponent<PlayerStats>(out PlayerStats stats);

        stats.damage *= damageModifier * bonus;
        stats.heavyDamage *= damageModifier * bonus;
        
        FreePlayer();
    }

    public void IncreaseDefence(float bonus)
    {
        player.TryGetComponent<PlayerStats>(out PlayerStats stats);
        stats.defenceModifier *= defenceModifier * bonus;
        FreePlayer();
    }
    

    public void IncreaseMoveSpeed(float bonus)
    {
        player.TryGetComponent <ThirdPersonController>(out ThirdPersonController controllerStats);
        controllerStats.MoveSpeed *= speedModifier * bonus;
        FreePlayer();
    }

    public void FourtuneCookie()
    {
        //Open panel that shows flavourtext. Closed opon anyinput

        int tempINT = Random.Range(1, 5);

        Debug.Log($"{tempINT}");
        
        switch (tempINT)
        {
            case 1: Debug.Log("1");
                IncreaseAttack(fortuneBonus);
                break;

            case 2:
                Debug.Log("2");
                IncreaseDefence(fortuneBonus);
                break;

            case 3:
                Debug.Log("3");
                IncreaseMoveSpeed(fortuneBonus);
                    break;
            case 4:
                Debug.Log("4");
                IncreaseMaxHealth(fortuneBonus);
                break;
            case 5:
                Debug.Log("5");
                RestoreHealth(fortuneBonus);
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
