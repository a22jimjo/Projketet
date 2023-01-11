using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
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
    private PlayerStats stats;
    List<GameObject> go = new List<GameObject> ();

    private VisualEffect currentVfxSlash;
    private VisualEffect currentVfxHeavySlash;

    // Start is called before the first frame update
    void Start()
    {
        _damaging = GetComponent<DamagingCollider>();
        GameObject player = GameObject.FindWithTag("Player");
        stats = player.GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.TryGetComponent<EnemyStats>(out EnemyStats enemyComponent) && attacking == true)
        {
            if (!go.Contains(other.gameObject))
            {
                go.Add(other.gameObject);
                Debug.Log("Hit");
                if(!heavyAttack)_damaging.CallDamage(other, stats.damage);
                if(heavyAttack)_damaging.CallDamage(other, stats.heavyDamage);
                Instantiate(HitMark, other.transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
            }
            else
            {
                go.Clear();
                attacking = false;
            }
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
