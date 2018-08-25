using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpIncrease : MonoBehaviour {


    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "PlayerPhysics") {
            Player player = collider.GetComponentInParent<Player>();
            player.increaseJumpHeight();
            Destroy(this.gameObject);
        }
    }
}