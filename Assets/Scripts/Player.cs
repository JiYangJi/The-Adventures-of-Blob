using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character {
    public GameObject Stick;
    public bool recovering;
    private float recoveryTime = 1f; //seconds
    private float recoveryClock = 0;

    // Use this for initialization
    void Start() {
        color = new Color32(65, 234, 101, 255);
        this.GetComponent<SpriteRenderer>().color = color;
        body = GetComponent<Rigidbody2D>();
        body.freezeRotation = true;
        maxHealth = 12;
        health = maxHealth;
        attack = 1;
        defense = 1;
        maxSpeed = 8;
        jumpAmount = 50f;
        numJumps = 1;
        jumpCounter = numJumps;
    }

    void Update() {
        grounded = isGrounded();
        setAnimatorParams();
        if (recovering) {
            recoveryClock += Time.deltaTime;
            SpriteRenderer sprite = this.GetComponent<SpriteRenderer>();
            //flicker for 0.1 seconds in the beginning of every 0.2 second period
            if (recoveryClock % 0.2 < 0.1) {
                sprite.enabled = false;
            } else {
                sprite.enabled = true;
            }
            if (recoveryClock >= recoveryTime) {
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemies"), false);
                recoveryClock = 0;
                recovering = false;
                sprite.enabled = true;
            }
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
            body.velocity += new Vector2(0, -2f);
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

    public void increaseJumpHeight() {
        jumpAmount += 5;
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
