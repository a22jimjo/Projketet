using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public Rigidbody rigidBody;
    private Vector3 _direction;


    public void PushRigidBdy(Rigidbody other)
    {
        _direction = rigidBody.transform.position - other.transform.position;
        rigidBody.AddForce(_direction * 5000);
    }
    
}
