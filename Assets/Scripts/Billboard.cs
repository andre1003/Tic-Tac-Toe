using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour {
    private Camera mainCamera;

    private void Update() {
        mainCamera = Camera.main;
    }

    private void LateUpdate() {
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.forward);
    }
}
