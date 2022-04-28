using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {
    public GameObject menu;


    private bool isActive = true;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            isActive = !isActive;
            menu.SetActive(isActive);            
        }
    }

    public void SetIsActive(bool isActive) {
        this.isActive = isActive;
        menu.SetActive(isActive);
    }

    public void ExitGame() {
        Application.Quit();
    }
}
