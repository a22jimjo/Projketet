using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public Image HealthBarSprite;
    public float ReduceSpeed = 2;
    private float _target = 1;
    private Camera _cam;

    private void Start()
    {
        _cam = Camera.main;
    }

    public void UpdateHealthBar(float maxHealth, float currentHealth)
    {
        _target = currentHealth / maxHealth;
    }

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - _cam.transform.position);
        HealthBarSprite.fillAmount = Mathf.MoveTowards(HealthBarSprite.fillAmount, _target, ReduceSpeed * Time.deltaTime);
    }
}
