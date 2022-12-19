using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDespawner : MonoBehaviour
{
    [SerializeField]private float _despawnTime;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_despawnTime < 0)
        {
            Destroy(gameObject);
        }

        _despawnTime -= Time.deltaTime;
    }
}
