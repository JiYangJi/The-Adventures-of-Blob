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
}
