using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public Scene LevelToLoad;
   

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Player")
        {
            //SceneManager.LoadScene(LevelToLoad.ToString());
            SceneManager.LoadScene(LevelToLoad.name);
        }
    }


    /*public enum Scene
   {
       GameScene,
   }

   public static void Load (Scene scene)
   {
       SceneManager.LoadScene (scene.ToString ());
   }*/
}
