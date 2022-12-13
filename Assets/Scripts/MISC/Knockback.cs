using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    private Rigidbody rigidBody;
    private Vector3 _direction;


    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    public void Update()
    {
        _direction = rigidBody.velocity;
        rigidBody.AddForce(-_direction * (Time.deltaTime * 50));
    }

    public void PushRigidBdy(Rigidbody other)
    {
        _direction = rigidBody.transform.position - other.transform.position;
        rigidBody.AddForce(_direction * 250);
    }
    
}
