using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Sword : MonoBehaviour
{
    private DamagingCollider _damaging;

    public bool attacking;
    public bool heavyAttack;
    public int damageToBeDone;
    public VisualEffect VfxSlashPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        _damaging = GetComponent<DamagingCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<EnemyStats>(out EnemyStats enemyComponent) && attacking == true)
        {
            Debug.Log("Hit");
            _damaging.CallDamage(other, heavyAttack);
            attacking = false;
        }
    }

    public void Vfx()
    {
        VfxSlashPrefab.transform.position = transform.position;
        VfxSlashPrefab.transform.rotation = transform.rotation;
        
        VfxSlashPrefab.Play();
    }
    
}
