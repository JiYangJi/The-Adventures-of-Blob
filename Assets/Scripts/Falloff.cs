using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Falloff : MonoBehaviour {

    void Start() {}

    void Update() {}

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "PlayerPhysics") {
            Player player = collider.GetComponentInParent<Player>();
            player.setToStartPosition();
        }
    }
}
