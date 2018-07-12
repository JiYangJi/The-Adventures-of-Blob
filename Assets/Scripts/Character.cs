using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {
    protected bool facingRight = true;
    protected float maxSpeed;
    protected float jumpAmount;
    protected int numJumps;
    protected int jumpCounter;

    protected Rigidbody2D body;

    protected bool isGrounded()
    {
        Bounds bounds = GetComponent<BoxCollider2D>().bounds;
        int platformMask = LayerMask.GetMask("Platforms");
        float distance = 0.02f;
        //check left and right
        RaycastHit2D resultLeft = Physics2D.Raycast(bounds.min, Vector2.down, distance, platformMask);
        RaycastHit2D resultRight = Physics2D.Raycast(bounds.min + new Vector3(bounds.size.x, 0, 0),
            Vector2.down, distance, platformMask);
        if (resultLeft.collider == null && resultRight.collider == null)
        {
            return false;
        }
        return true;
    }

    protected void move(float amount)
    {
        body.velocity = new Vector2(amount * maxSpeed, body.velocity.y);
        flipLeftRight(amount);
    }

    protected void flipLeftRight(float amount)
    {
        if ((amount > 0 && !facingRight) || (amount < 0 && facingRight))
        {
            facingRight = !facingRight;
            transform.localScale *= new Vector2(-1, 1);
        }
    }

    protected void jump()
    {
        body.velocity = new Vector2(body.velocity.x, jumpAmount);
    }

}
