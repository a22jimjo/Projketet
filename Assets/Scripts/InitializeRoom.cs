using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeRoom : MonoBehaviour
{
    public GameObject PlayerStartPosition;

    private GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        PlayerStartPosition = GameObject.FindGameObjectWithTag("PlayerStartPosition");

        player.transform.position = PlayerStartPosition.transform.position;
    }

    private void Start()
    {
        
    }
}
