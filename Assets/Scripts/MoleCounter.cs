using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleCounter : MonoBehaviour {

    public GameController gameController;
    public int molePointValue = 1;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Bullet")) {
            Destroy(other.gameObject);
            gameController.AddScore(molePointValue);
        }
    }
}
