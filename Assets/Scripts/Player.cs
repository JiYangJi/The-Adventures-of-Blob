using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character {
    public GameObject Stick;
    public bool isAttacked;
    public float recoveryTime = 5; //seconds
    public float recoveryClock = 0;

    // Use this for initialization
    void Start() {
        body = GetComponent<Rigidbody2D>();
        body.freezeRotation = true;
        health = 12;
        attack = 1;
        defense = 1;
        maxSpeed = 10;
        jumpAmount = 30f;
        numJumps = 1;
        jumpCounter = numJumps;
    }

    void Update() {
        grounded = isGrounded();
        setAnimatorParams();
        if (isAttacked) {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Characters"), LayerMask.NameToLayer("Characters"));
            recoveryClock += Time.deltaTime;
            if (recoveryClock >= recoveryTime) {
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Characters"), LayerMask.NameToLayer("Characters"), false);
                recoveryClock = 0;
                isAttacked = false;
            }
            return;
        }

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
        GameObject stick = Instantiate(Stick);
        stick.transform.SetParent(this.transform.Find("Equipped").transform, false);
    }

    public void setToStartPosition() {
        body.velocity = new Vector2(0, 0);
        transform.position = new Vector2(1, 1);
    }
}
