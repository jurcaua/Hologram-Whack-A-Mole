using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageTargetCollison : MonoBehaviour {

    private Collider c;

    private void Start() {
        c = GetComponent<Collider>();
    }

    void Update() {
        // always make sure collider is ON
        if (!c.enabled) {
            c.enabled = true;
        }
    }
}

