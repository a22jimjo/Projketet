using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private DamagingCollider _damaging;

    public bool attacking;
    public bool heavyAttack;
    public int damageToBeDone;
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
    
}
