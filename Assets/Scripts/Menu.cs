using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {
    public GameObject menu;
    public GameObject loadLevelCanvas;
    public Slider progressBar;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void LoadGame() {
        menu.SetActive(false);
        loadLevelCanvas.SetActive(true);
        StartCoroutine(LoadAsynchronous(SceneManager.GetActiveScene().buildIndex + 1));
    }

    private IEnumerator LoadAsynchronous(int level) {
        AsyncOperation operation = SceneManager.LoadSceneAsync(level);

        while (!operation.isDone) {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            progressBar.value = progress;

            yield return null;
        }
    }

    public void ExitGame() {
        Application.Quit();
    }
}
