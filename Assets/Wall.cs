using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    
    private GameObject enemyHolder;
    private GameObject Spawning;
    public List<GameObject> enemyGameobjects = new List<GameObject>();

    public string waveNumber = "";


    // Start is called before the first frame update
    void Start()
    {
        enemyHolder = GameObject.FindGameObjectWithTag($"Wave {waveNumber}");
    }

    // Update is called once per frame
    void Update()
    {
        FindAllEnemies();
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

        if (enemyGameobjects.Count == 0)
        {
            StartCoroutine(OpenUp());
        }
    }

    IEnumerator OpenUp()
    {
        transform.position -= new Vector3(0, 1 * Time.deltaTime, 0);
        yield return new WaitForSeconds(7);
        Destroy(this.gameObject);
    }
}
