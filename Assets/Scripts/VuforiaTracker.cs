using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VuforiaTracker : MonoBehaviour {

    private MeshRenderer r;
    
	void Start () {
        r = GetComponent<MeshRenderer>();
	}
	
	public bool TrackingFound() {
        return r.enabled;
    }
}
