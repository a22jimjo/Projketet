using UnityEngine;

public class PersistentObjects : MonoBehaviour
{
    public GameObject[] objectsToPersist;

    void Start()
    {
        foreach (GameObject obj in objectsToPersist)
        {
            DontDestroyOnLoad(obj);
        }
    }
}