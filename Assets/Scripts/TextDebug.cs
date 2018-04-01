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
	
	// Update is called once per frame
	void Update () {
        trackerText.text = "Tracking: " + tracker.TrackingFound().ToString();

        touchText.text = "Shooting: " + shooting.ToString() + " - " + bullets.childCount;
        shooting = false;

        cameraPosition.text = "Camera Position: " + ARCamera.position.ToString();

        modelPosition.text = "Model Position: " + model.position.ToString();

        if (bullets.childCount > 0) {
            bulletPosition.text = "Bullet Position: " + bullets.GetChild(0).position.ToString();
        } else{
            bulletPosition.text = "No bullets...";
        }

        /*
        distance.text = "Distance: " + Vector3.Distance(between[0].transform.position, hammer.position).ToString() 
            + ", " + Vector3.Distance(between[1].transform.position, hammer.position).ToString()
            + ", " + Vector3.Distance(between[2].transform.position, hammer.position).ToString();

        colliding.text = "Colliding: " + between[0].collidingWithHammer.ToString()
            + ", " + between[1].collidingWithHammer.ToString()
            + ", " + between[2].collidingWithHammer.ToString();
            */
    }
}
