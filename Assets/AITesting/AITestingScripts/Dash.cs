using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{

    SimpleCharacterController moveScript;

    public float dashSpeed;
    public float dashTime;

    public Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        moveScript = GetComponent<SimpleCharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            float startTime = Time.time;
            Vector3 mousePos = Input.mousePosition;
            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;
                Debug.DrawLine(transform.position, objectHit.position);
                // Do something with the object that was hit by the raycast.
            }
            Debug.Log(mousePos.x);
            Debug.Log(mousePos.y);
            StartCoroutine(DashEnum());
        }
    }

    public IEnumerator DashEnum()
    {
        float startTime = Time.time;

        while(Time.time < startTime + dashTime)
        {
            //moveScript.controller.Move(mousePos * dashSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
