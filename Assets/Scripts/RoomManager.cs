using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{

    [SerializeField] List<GameObject> enemyGameobjects = new List<GameObject>();
    [SerializeField] List<GameObject> possibleRooms = new List<GameObject>();
    [SerializeField] bool randomRooms;
    [SerializeField] private GameObject currentRoom;
    private int nextRoom;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadRoom(Vector3 spawnPos)
    {
        if(currentRoom != null)
        {
            RemoveAllEnemies();
            Destroy(currentRoom);
        }
        GameObject room;
        if (randomRooms)
        {
            room = Instantiate(possibleRooms[Random.Range(0, possibleRooms.Count)], spawnPos, Quaternion.identity);
            currentRoom = room;
        }
        else
        {
            room = Instantiate(possibleRooms[nextRoom%possibleRooms.Count], spawnPos, Quaternion.identity);
            nextRoom++;
            currentRoom = room;
        }

        Transform[] objectsInRoom = room.GetComponentsInChildren<Transform>();

        for (int i = 0; i < objectsInRoom.Length; i++)
        {
            if (objectsInRoom[i].CompareTag("Enemy"))
            {
                enemyGameobjects.Add(objectsInRoom[i].gameObject);
            }
        }
    }

    public void RemoveAllEnemies()
    {
        for (int i = enemyGameobjects.Count - 1; i >= 0; i--)
        {
            Destroy(enemyGameobjects[i]);
            enemyGameobjects.RemoveAt(i);
        }
        enemyGameobjects.Clear();
    }
}
