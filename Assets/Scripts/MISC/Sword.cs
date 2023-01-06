using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
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

    private VisualEffect currentVfxSlash;
    private VisualEffect currentVfxHeavySlash;
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
            attacking = false;
            Debug.Log("Hit");
            if(!heavyAttack)_damaging.CallDamage(other, damage);
            if(heavyAttack)_damaging.CallDamage(other, heavyDamage);
            Instantiate(HitMark, other.transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
        }
    }

    public void Vfx()
    {
        if (heavyAttack)
        {
            currentVfxHeavySlash = Instantiate(vfxheavySlashPrefab, transform.position, transform.rotation);
            currentVfxHeavySlash.Play();
        }
        else
        {
            currentVfxSlash = Instantiate(VfxSlashPrefab, transform.position, transform.rotation);
            currentVfxSlash.Play();
        }
    }
    
}
