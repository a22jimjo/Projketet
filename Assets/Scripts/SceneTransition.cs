using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    /*public Material fadeMaterial;
    public float fadeTime = 1.0f;

    void OnTriggerEnter(Collider other)
    {
        // start the fade coroutine when the player enters the trigger
        StartCoroutine(Fade(SceneManager.GetActiveScene().name, "NewSceneName"));
    }

    IEnumerator Fade(string fromScene, string toScene)
    {
        // set the _Fade value of the fade material to 0
        fadeMaterial.SetFloat("_Fade", 0);

        // enable the fade material
        fadeMaterial.enabled = true;

        // fade out the current scene
        float elapsedTime = 0.0f;
        while (elapsedTime < fadeTime)
        {
            // gradually increase the _Fade value of the material
            float t = elapsedTime / fadeTime;
            fadeMaterial.SetFloat("_Fade", Mathf.Lerp(0, 1, t));

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // load the new scene
        SceneManager.LoadScene(toScene);

        // wait for the new scene to be loaded
        yield return new WaitForSeconds(1.0f);

        // fade in the new scene
        elapsedTime = 0.0f;
        while (elapsedTime < fadeTime) ;
    } */
}

