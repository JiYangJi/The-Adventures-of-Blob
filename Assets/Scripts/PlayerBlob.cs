using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlob : Character {
    // Use this for initialization
    void Start() {
        body = GetComponent<Rigidbody2D>();
        body.freezeRotation = true;
        maxSpeed = 10;
        jumpAmount = 30f;
        numJumps = 1;
        jumpCounter = numJumps;
    }

    void Update() {
        GetComponent<Animator>().SetBool("isMoving", Mathf.Abs(Input.GetAxis("Horizontal")) > 0.01);
        if (transform.Find("Equipped").childCount > 0) {
            transform.Find("Equipped").GetChild(0).GetComponent<Animator>().SetBool("attack", Input.GetButtonDown("Attack"));
        }

        bool grounded = isGrounded();
        GetComponent<Animator>().SetBool("isJumping", !grounded);

        float move_x = Input.GetAxis("Horizontal");
        bool pressedJump = Input.GetButtonDown("Jump");
        if (grounded) {
            jumpCounter = numJumps;
        }

        if (!pressedJump) {
            move(move_x);
        } else if (grounded || jumpCounter > 0) {
            jump();
            jumpCounter--;
        }

    }
    //don't jump as high if the jump key is let go
    //fall fast
    void jumpAdjust() {
        if (body.velocity.y < 0) {
            body.velocity += new Vector2(0, -0.1f);
        } else if (body.velocity.y > 0 && !Input.GetButton("Jump")) {
            body.velocity += new Vector2(0, -1f);
        }
    }

    // FixedUpdate not necessarily the same as frame update
    void FixedUpdate() {
        jumpAdjust();
    }

    public void increaseNumJumps() {
        numJumps++;
        jumpCounter++;
    }

    public void setStickWeapon() {
        GameObject stick = Instantiate(Resources.Load("Weapons/Stick")) as GameObject;
        stick.transform.SetParent(this.transform.Find("Equipped").transform, false);
    }

    public void setToStartPosition() {
        transform.position = new Vector2(1, 1);
    }
}
