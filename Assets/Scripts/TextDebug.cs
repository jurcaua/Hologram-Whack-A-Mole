using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class DistancePair {
    public Transform object1;
    public Transform object2;
}

public class TextDebug : MonoBehaviour {

    public bool debugMobile = true;

    public TextMeshProUGUI trackerText;
    public VuforiaTracker tracker;

    public TextMeshProUGUI touchText;
    [HideInInspector] public bool shooting = false;
    public Transform bullets;

    public Transform model;
    public TextMeshProUGUI modelPosition;
    public TextMeshProUGUI bulletPosition;
    public Transform ARCamera;
    public TextMeshProUGUI cameraPosition;

    private void Start() {
        if (debugMobile) {
            trackerText.gameObject.SetActive(true);

            touchText.gameObject.SetActive(true);

            modelPosition.gameObject.SetActive(true);
            trackerText.gameObject.SetActive(true);
            bulletPosition.gameObject.SetActive(true);
            cameraPosition.gameObject.SetActive(true);
        } else {
            trackerText.gameObject.SetActive(false);

            touchText.gameObject.SetActive(false);

            modelPosition.gameObject.SetActive(false);
            trackerText.gameObject.SetActive(false);
            bulletPosition.gameObject.SetActive(false);
            cameraPosition.gameObject.SetActive(false);
        }
    }

    void Update () {
        if (debugMobile) {
            trackerText.text = "Tracking: " + tracker.TrackingFound().ToString();

            touchText.text = "Shooting: " + shooting.ToString() + " - " + bullets.childCount;
            shooting = false;

            cameraPosition.text = "Camera Position: " + ARCamera.position.ToString();

            modelPosition.text = "Model Position: " + model.position.ToString();

            if (bullets.childCount > 0) {
                bulletPosition.text = "Bullet Position: " + bullets.GetChild(0).position.ToString();
            } else {
                bulletPosition.text = "No bullets...";
            }
        }
    }
}
