using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character {

    protected bool moveLeft = true;

	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody2D>();
        body.freezeRotation = true;
        maxSpeed = 20;
        jumpAmount = 20f;
        numJumps = 1;
        jumpCounter = numJumps;
    }
	
	// Update is called once per frame
	void Update () {
        grounded = isGrounded();
        setAnimatorParams();

        int platforms = LayerMask.GetMask("Platforms");
        if (!leftBottomCollide(platforms) || leftCollide(platforms)) {
            moveLeft = false;
        }
        if (!rightBottomCollide(platforms) || rightCollide(platforms)) {
            moveLeft = true;
        }

        if (moveLeft) {
            move(-0.1f);
        } else {
            move(0.1f);
        }
	}
}
