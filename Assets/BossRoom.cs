using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossRoom : MonoBehaviour
{
    
    public List<GameObject> enemyGameobjects = new List<GameObject>();
    
    private GameObject enemyHolder;


    private void Start()
    {
        enemyHolder = GameObject.FindGameObjectWithTag("EnemyHolder");
        
    }
    
    void OnTriggerEnter(Collider other)
    {
        FindAllEnemies();
        if (other.gameObject.tag == "Player" && enemyGameobjects.Count <= 0)
        {
            GameObject.FindWithTag("Music").SetActive(false);
        }
        
    }
    
    private void FindAllEnemies()
    {
        Transform[] objectsInRoom = enemyHolder.GetComponentsInChildren<Transform>();

        if(enemyHolder != null)
        {
            enemyGameobjects.Clear();
            for (int i = 0; i < objectsInRoom.Length; i++)
            {
                if (objectsInRoom[i].CompareTag("Enemy"))
                {
                    enemyGameobjects.Add(objectsInRoom[i].gameObject);
                }
            }
        }
    }
}
