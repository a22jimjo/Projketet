using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemSmash : MonoBehaviour
{
    
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.gameObject.TryGetComponent<PlayerStats>(out PlayerStats playerComponent))
            {
                playerComponent.TakeDamage(GetComponentInParent<EnemyStats>().damage);
                gameObject.SetActive(false);
            }
        }
    }
}
