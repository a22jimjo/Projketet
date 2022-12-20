using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemSmash : MonoBehaviour
{

    [SerializeField] private int damage;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.gameObject.TryGetComponent<PlayerStats>(out PlayerStats playerComponent))
            {
                playerComponent.TakeDamage(damage);
                gameObject.SetActive(false);
            }
        }
    }
}
