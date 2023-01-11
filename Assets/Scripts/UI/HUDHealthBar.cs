using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDHealthBar : MonoBehaviour
{
    public Image HealthBarSprite;
    public float ReduceSpeed = 2;
    private float _target = 1;

    private void Start()
    {
    }

    public void UpdateHealthBar(float maxHealth, float currentHealth)
    {
        _target = currentHealth / maxHealth;
    }

    private void Update()
    {
        HealthBarSprite.fillAmount = Mathf.MoveTowards(HealthBarSprite.fillAmount, _target, ReduceSpeed * Time.deltaTime);
    }
}
