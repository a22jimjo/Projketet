using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Cookie : MonoBehaviour
{
    [SerializeField] private GameObject attack;
    [SerializeField] private GameObject defense;
    [SerializeField] private GameObject speed;
    [SerializeField] private GameObject maxHealth;
    [SerializeField] private GameObject restoreHealth;
    

    public void DisplayCookie(int option)
    {
        switch (option)
        {
            case 1:
                attack.SetActive(true);
                break;

            case 2:
                defense.SetActive(true);
                break;

            case 3:
                speed.SetActive(true);
                break;
            case 4:
                maxHealth.SetActive(true);
                break;
            case 5:
                restoreHealth.SetActive(true);
                break;
        }
        
        
    }

    public void StopDisplay(int option)
    {
        switch (option)
        {
            case 1:
                attack.SetActive(false);
                break;

            case 2:
                defense.SetActive(false);
                break;

            case 3:
                speed.SetActive(false);
                break;
            case 4:
                maxHealth.SetActive(false);
                break;
            case 5:
                restoreHealth.SetActive(false);
                break;
        }
    }
    
    
}
