using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float speed = 10f;
    public float lifetime = 5f;

    bool isDebug = false;

    private float timeAlive = 0f;

    private VuforiaTracker tracker;
    private Renderer r;
	
    void Start() {
        tracker = GameObject.FindGameObjectWithTag("Tracker").GetComponent<VuforiaTracker>();
        r = GetComponent<Renderer>();
    }

    public void SetIsDebug(bool toSet) {
        isDebug = toSet;
    }

	void FixedUpdate () {
        if (tracker.TrackingFound() || isDebug) {
            //r.enabled = true;
            transform.Translate(transform.forward * Time.fixedDeltaTime * speed);
            //transform.position += transform.forward * speed * Time.fixedDeltaTime;
            timeAlive += Time.fixedDeltaTime;
            if (timeAlive > lifetime) {
                Destroy(gameObject);
            }
        } else {
            //r.enabled = false;
            Debug.Log("(" + gameObject.name + ")tracker.TrackingFound()=" + tracker.TrackingFound().ToString() + " | isDebug=" + isDebug.ToString());
        }
	}
}
