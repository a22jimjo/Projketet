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

    private GameObject player;
    private GameObject UpgradeScreen;

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

    public void RestoreHealth()
    {
        player.TryGetComponent<PlayerStats>(out PlayerStats stats);

        if (stats.health + healing <= stats.maxHealth)
        {
            stats.health += healing;
        }
        else stats.health = stats.maxHealth;

        UpgradeScreen.SetActive(false);

    }

    public void IncreaseMaxHealth()
    {
        player.TryGetComponent<PlayerStats>(out PlayerStats stats);
        stats.maxHealth += maxHealthIncrease;
        stats.health += maxHealthIncrease;

        UpgradeScreen.SetActive(false);

    }

    public void IncreaseAttack()
    {
        player.TryGetComponent<PlayerStats>(out PlayerStats stats);

        stats.damage *= damageModifier;
        stats.heavyDamage *= damageModifier;

        UpgradeScreen.SetActive(false);

    }

    public void IncreaseDefence()
    {
        player.TryGetComponent<PlayerStats>(out PlayerStats stats);
        stats.defenceModifier *= defenceModifier;

        UpgradeScreen.SetActive(false);

    }
}
