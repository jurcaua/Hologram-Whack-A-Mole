using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vuforia;

public class UIManager : MonoBehaviour {

	public void LoadGame() {
        SceneManager.LoadScene("main");
    }

    public void GoToMainMenu() {
        SceneManager.LoadScene("title");
    }

    public void GoToSettings() {
        SceneManager.LoadScene("settings");
    }
}
