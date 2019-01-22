using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
    public bool facingRight = true;
    public int maxHealth;
    public int health;
    public int attack;
    public int defense;

    protected bool incapacitated = false;
    protected float incapacitatedTime = 0.5f; // seconds
    protected float incapacitatedClock = 0;
    protected bool recovering = false;
    protected float recoveryTime; //seconds
    protected float recoveryClock = 0;
    protected float maxSpeed;
    protected float dashTime;
    protected float dashClock = 0f;
    protected bool isDashing = false;
    protected float jumpAmount;
    protected int numJumps;
    protected int jumpCounter;
    protected bool grounded;
    protected Color32 color;

    protected Rigidbody2D body;

    protected void UpdateCharacterParams() {
        grounded = isGrounded();
        SpriteRenderer sprite = this.GetComponent<SpriteRenderer>();

        if (incapacitated) {
            incapacitatedClock += Time.deltaTime;
            if (incapacitatedClock > incapacitatedTime) {
                incapacitated = false;
                incapacitatedClock = 0;
            }
        }
        if (recovering) {
            recoveryClock += Time.deltaTime;
            //flicker for 0.1 seconds in the beginning of every 0.2 second period
            if (recoveryClock % 0.2 < 0.1) {
                sprite.enabled = false;
            } else {
                sprite.enabled = true;
            }
            if (recoveryClock >= recoveryTime) {
                transform.Find("AttackableTrigger").GetComponent<BoxCollider2D>().enabled = true;
                recoveryClock = 0;
                recovering = false;
                sprite.enabled = true;
            }
        }
        GetComponent<Animator>().SetBool("isMoving", Mathf.Abs(body.velocity.x) > 0.01);
        GetComponent<Animator>().SetBool("isJumping", !grounded);
    }

    protected bool leftCollide(int layerMask, float dist = 0.02f) {
        Bounds bounds = GetComponent<BoxCollider2D>().bounds;
        return Physics2D.Raycast(bounds.center, Vector2.left, dist + bounds.extents.x, layerMask).collider != null;
    }

    protected bool rightCollide(int layerMask, float dist = 0.02f) {
        Bounds bounds = GetComponent<BoxCollider2D>().bounds;
        return Physics2D.Raycast(bounds.center, Vector2.right, dist + bounds.extents.x, layerMask).collider != null;
    }

    protected bool leftBottomCollide(int layerMask, float dist = 0.02f) {
        Bounds bounds = GetComponent<BoxCollider2D>().bounds;
        return Physics2D.Raycast(bounds.min, Vector2.down, dist, layerMask).collider != null;
    }

    protected bool rightBottomCollide(int layerMask, float dist = 0.02f) {
        Bounds bounds = GetComponent<BoxCollider2D>().bounds;
        return Physics2D.Raycast(bounds.min + new Vector3(bounds.size.x, 0, 0),
                                    Vector2.down, dist, layerMask).collider != null;
    }

    protected bool isGrounded() {
        int platforms = LayerMask.GetMask("Platforms");
        if (leftBottomCollide(platforms) || rightBottomCollide(platforms)) {
            return true;
        }
        return false;
    }

    protected void move(float amount) {
        if (!incapacitated) {
            body.velocity = new Vector2(amount * maxSpeed, body.velocity.y);
            flipLeftRight(amount);
        }
    }

    protected void dash() {
        if (dashClock <= dashTime) {
            if (facingRight) {
                body.velocity += new Vector2(10*Mathf.Sqrt(maxSpeed), 0);
                //body.AddForce(new Vector2(500, 0)); alternative method...
            } else {
                body.velocity += new Vector2(-10* Mathf.Sqrt(maxSpeed), 0);
            }
        } else {
            dashClock = 0;
            isDashing = false;
        }
        dashClock += Time.deltaTime;

    }

    protected void flipLeftRight(float amount) {
        if ((amount > 0 && !facingRight) || (amount < 0 && facingRight)) {
            facingRight = !facingRight;
            transform.localScale *= new Vector2(-1, 1);
        }
    }

    protected void jump() {
        if (!incapacitated) {
            body.velocity = new Vector2(body.velocity.x, Mathf.Sqrt(jumpAmount));
        }
    }

    public void attackCharacter(int attack, Vector2 direction, float amount) {
        float raw_damage = (attack * attack) / (float) (attack + defense);
        int damage = (int) raw_damage;
        if (damage == 0) {
            damage = 1;
        }
        health -= damage;
        if (health <= 0) {
            DestroyCharacter();
            return;
        }
        body.velocity = Vector2.zero; //zero
        direction = direction.normalized;
        if (direction.normalized == Vector2.zero) {
            direction.y = 1;
        }
        body.AddForce(direction.normalized * amount);

        incapacitated = true;
        SetRecovering();
    }

    public void SetRecovering() {
        recovering = true;
        transform.Find("AttackableTrigger").GetComponent<BoxCollider2D>().enabled = false;
    }

    protected virtual void DestroyCharacter() {
        Destroy(this.gameObject);
    }
}
