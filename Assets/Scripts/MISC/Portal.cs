using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject roomToDestroy;
    public GameObject roomToLoad;
    public GameObject player;
    public bool isEndPortal;



    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Portal Entered");

        if (other.gameObject.CompareTag("Player"))
        {
            if (isEndPortal)
            {
                EndGame();
            }
            else
            {
                LoadRoom();
            }
        }
    }

    public void LoadRoom()
    {
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<RoomManager>().LoadRoom(transform.position);
    }

    public void EndGame()
    {
        //load winning screen
        Application.Quit();
        Debug.Log("Game has ended");
    }
}
