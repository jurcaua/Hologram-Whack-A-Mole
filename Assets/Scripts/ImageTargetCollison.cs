using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageTargetCollison : MonoBehaviour {

    [HideInInspector] public bool collidingWithHammer = false;

    private Collider c;

    private void Start() {
        c = GetComponent<Collider>();
    }

    void Update() {
        if (!c.enabled) {
            c.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Hammer")) {
            collidingWithHammer = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Hammer")) {
            collidingWithHammer = false;
        }
    }
}

