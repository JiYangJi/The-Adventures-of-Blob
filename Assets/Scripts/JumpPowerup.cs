using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPowerup : MonoBehaviour {

    public Player player;

    // Use this for initialization
    void Start() {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            Player player = collision.GetComponent<Player>();
            player.increaseNumJumps();
            Destroy(this.gameObject);
        }
    }
}
