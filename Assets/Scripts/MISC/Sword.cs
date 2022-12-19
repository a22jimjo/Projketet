using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Sword : MonoBehaviour
{
    private DamagingCollider _damaging;

    public bool attacking;
    public bool heavyAttack;
    private int damageToBeDone;
    public VisualEffect VfxSlashPrefab;
    public VisualEffect vfxheavySlashPrefab;
    public GameObject HitMark;
    private float damage;
    private float heavyDamage;

    // Start is called before the first frame update
    void Start()
    {
        _damaging = GetComponent<DamagingCollider>();
        GameObject player = GameObject.FindWithTag("Player");
        PlayerStats stats = player.GetComponent<PlayerStats>();
        damage = stats.damage;
        heavyDamage = stats.heavyDamage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.TryGetComponent<EnemyStats>(out EnemyStats enemyComponent) && attacking == true)
        {
            Debug.Log("Hit");
            if(!heavyAttack)_damaging.CallDamage(other, false, damage);
            if(heavyAttack)_damaging.CallDamage(other, false, heavyDamage);
            attacking = false;
            Instantiate(HitMark, other.transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
        }
    }

    public void Vfx()
    {
        if(heavyAttack) VfxSlashPrefab.Play();
        else VfxSlashPrefab.Play();
    }
    
}
