using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character {

    protected bool moveLeft = true;

	// Use this for initialization
	void Start () {
        color = new Color32(100, 50, 230, 255);
        this.GetComponent<SpriteRenderer>().color = color;
        body = GetComponent<Rigidbody2D>();
        body.freezeRotation = true;
        health = 3;
        attack = 1;
        defense = 0;
        maxSpeed = 10;
        jumpAmount = 20f;
        numJumps = 1;
        jumpCounter = numJumps;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemies"), LayerMask.NameToLayer("Enemies"));
    }

    // Update is called once per frame
    void Update () {
        grounded = isGrounded();

        setAnimatorParams();
        if (!grounded) {
            return; //don't move if in air
        }
        int platforms = LayerMask.GetMask("Platforms");

        bool endLeft = !leftBottomCollide(platforms) || leftCollide(platforms);
        bool endRight = !rightBottomCollide(platforms) || rightCollide(platforms);

        if (endLeft != endRight) {
            moveLeft = endRight;
        }

        if (moveLeft) {
            move(-0.1f);
        } else {
            move(0.1f);
        }
	}

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.tag == "Player") {
            Player player = collision.collider.GetComponent<Player>();
            if (!player.recovering) {
                player.attackCharacter(this.attack, collision.transform.position - this.transform.position, 120);
                player.recovering = true;
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemies"));
            }
        }
    }
}
