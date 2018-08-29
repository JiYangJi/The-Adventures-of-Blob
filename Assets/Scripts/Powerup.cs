using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Powerup : MonoBehaviour {

    void Start() {}

    void Update() {}

    protected abstract void ActivatePowerup(Player player);

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "PlayerPhysics") {
            ActivatePowerup(collider.GetComponentInParent<Player>());
            Destroy(this.gameObject);
        }
    }
}
