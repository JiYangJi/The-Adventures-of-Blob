using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
    public bool facingRight = true;
    protected float maxSpeed;
    protected float jumpAmount;
    protected int numJumps;
    protected int jumpCounter;
    protected bool grounded;

    protected Rigidbody2D body;

    protected void setAnimatorParams() {
        GetComponent<Animator>().SetBool("isMoving", Mathf.Abs(body.velocity.x) > 0.01);
        if (transform.Find("Equipped").childCount > 0) {
            transform.Find("Equipped").GetChild(0).GetComponent<Animator>().SetBool("attack", Input.GetButtonDown("Attack"));
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
        body.velocity = new Vector2(amount * maxSpeed, body.velocity.y);
        flipLeftRight(amount);
    }

    protected void flipLeftRight(float amount) {
        if ((amount > 0 && !facingRight) || (amount < 0 && facingRight)) {
            facingRight = !facingRight;
            transform.localScale *= new Vector2(-1, 1);
        }
    }

    protected void jump() {
        body.velocity = new Vector2(body.velocity.x, jumpAmount);
    }

}
