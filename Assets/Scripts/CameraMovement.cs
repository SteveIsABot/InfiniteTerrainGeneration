using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] float mainSpeed = 100.0f;
    [SerializeField] float shiftAdd = 250.0f;
    [SerializeField] float maxShift = 1000.0f;
    [SerializeField] float camSens = 0.25f;
    void Update() {

        if(Input.GetKey (KeyCode.W)) { transform.position += transform.forward.normalized * mainSpeed * Time.fixedDeltaTime; }
        if(Input.GetKey (KeyCode.A)) { transform.position -= transform.right.normalized * mainSpeed * Time.fixedDeltaTime; }
        if(Input.GetKey (KeyCode.S)) { transform.position -= transform.forward.normalized * mainSpeed * Time.fixedDeltaTime; }
        if(Input.GetKey (KeyCode.D)) { transform.position += transform.right.normalized * mainSpeed * Time.fixedDeltaTime; }
    }
}
