using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour {

    public float startTime = 60;

    public GameController gameController;
    public VuforiaTracker tracker;

    private int displayTime;
    private float currentTime;
    private bool gameOver = false;

    private TextMeshProUGUI text;

	void Start () {
        text = GetComponent<TextMeshProUGUI>();
        if (gameController == null) {
            gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        }

        currentTime = startTime;
        displayTime = Mathf.FloorToInt(currentTime);
    }
	
    void UpdateTime() {
        text.text = "Time Remaining: " + displayTime;
    }

    void Update() {
        if ((tracker.TrackingFound() || gameController.debugMode) && !gameOver) {
            text.color = Color.white;

            currentTime -= Time.deltaTime;
            displayTime = Mathf.FloorToInt(currentTime);

            if (displayTime < 0) {
                gameController.GameOver();
                gameOver = true;
                currentTime = 0f;
            }

            UpdateTime();
        } else {
            text.color = Color.red;
        }

        if (Application.isEditor) {
            if (Input.GetKey(KeyCode.E)) {
                currentTime -= 1f;
            }
        }
	}
}
