using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
    public bool facingRight = true;
    public int maxHealth;
    public int health;
    public int attack;
    public int defense;

    protected bool incapacitated;
    protected float incapacitatedTime = 0.5f; // seconds
    protected float hitTime = 0.1f;
    protected float hitCount = 0;
    protected float incapacitatedCount = 0;
    protected float maxSpeed;
    protected float jumpAmount;
    protected int numJumps;
    protected int jumpCounter;
    protected bool grounded;
    protected Color32 color;

    protected Rigidbody2D body;

    protected void setAnimatorParams() {
        SpriteRenderer sprite = this.GetComponent<SpriteRenderer>();

        if (incapacitated) {
            incapacitatedCount += Time.deltaTime;
            hitCount += Time.deltaTime;
            if (hitCount > hitTime) {
                sprite.color = color;
                hitCount = 0;
            }
            if (incapacitatedCount > incapacitatedTime) {
                incapacitated = false;
                incapacitatedCount = 0;
                sprite.color = color;
            }
        }
        GetComponent<Animator>().SetBool("isMoving", Mathf.Abs(body.velocity.x) > 0.01);
        Transform equipped = transform.Find("Equipped");
        if (equipped != null && equipped.childCount > 0) {
            equipped.GetChild(0).GetComponent<Animator>().SetBool("attack", Input.GetButtonDown("Attack"));
        }
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
            Destroy(this.gameObject);
            return;
        }
        body.velocity = Vector2.zero; //zero
        incapacitated = true;
        incapacitatedCount = 0;
        hitCount = 0;
        SpriteRenderer sprite = this.GetComponent<SpriteRenderer>();
        sprite.color = new Color32();
        direction = direction.normalized;
        if (direction.normalized == Vector2.zero) {
            direction.y = 1;
        }
        body.AddForce(direction.normalized * amount);
    }
}
