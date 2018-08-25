using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character {
    public GameObject Stick;

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
        maxSpeed = 5;
        jumpAmount = 300f;
        numJumps = 1;
        jumpCounter = numJumps;
        recoveryTime = 1f; 
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("PlayerPhysics"), LayerMask.NameToLayer("EnemiesPhysics"));
    }

    void Update() {
        UpdateCharacterParams();

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
        if (body.velocity.y <= 0) {
            body.velocity -= new Vector2(0, 1.2f);
        } else if (body.velocity.y > 0 && !Input.GetButton("Jump")) {
            body.velocity -= new Vector2(0, 1.2f);
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
        jumpAmount += 60f;
    }

    public void increaseMaxHealth() {
        maxHealth += 5;
        health = maxHealth;
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
