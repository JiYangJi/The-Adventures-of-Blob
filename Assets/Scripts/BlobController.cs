using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobController : MonoBehaviour {
    public bool facingRight = true;
    public float maxSpeed = 10;
    public float jumpAmount = 30f;
    public int numJumps = 1;
    public int jumpCounter;

    Rigidbody2D body;

	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody2D>();
        body.freezeRotation = true;
        jumpCounter = numJumps;
        transform.GetChild(0).GetComponent<Renderer>().enabled = false;
        Destroy(transform.GetChild(0).GetComponent<GameObject>());
    }

    void Update()
    {
        GetComponent<Animator>().SetBool("isMoving", Mathf.Abs(Input.GetAxis("Horizontal")) > 0.01);
        this.transform.GetChild(0).GetComponent<Animator>().SetBool("attack", Input.GetButtonDown("Attack"));

        bool grounded = isGrounded();
        GetComponent<Animator>().SetBool("isJumping", !grounded);

        float move_x = Input.GetAxis("Horizontal");
        bool pressedJump = Input.GetButtonDown("Jump");
        if (grounded)
        {
            jumpCounter = numJumps;
        }

        if (!pressedJump)
        {
            move(move_x);
        }
        else if (grounded || jumpCounter > 0)
        {
            jump();
            jumpCounter--;
        }

    }

    void move(float amount) {
        body.velocity = new Vector2(amount * maxSpeed, body.velocity.y);
        checkFlip(amount);
    }

    void checkFlip(float amount) {
        if ((amount > 0 && !facingRight) || (amount < 0 && facingRight))
        {
            FlipLeftRight();
        }
    }

    void jump() {
        body.velocity = new Vector2(body.velocity.x, jumpAmount);
    }

    //don't jump as high if the jump key is let go
    //fall fast
    void jumpAdjust() {
        if (body.velocity.y < 0)
        {
            body.velocity += new Vector2(0, -0.1f);
        }
        else if (body.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            body.velocity += new Vector2(0, -1f);
        } 
    }

    // FixedUpdate not necessarily the same as frame update
    void FixedUpdate()
    {
        jumpAdjust();
    }

    private bool isGrounded() {
        Bounds bounds = GetComponent<BoxCollider2D>().bounds;
        int platformMask = LayerMask.GetMask("Platforms");
        float distance = 0.02f;
        //check left and right
        RaycastHit2D resultLeft = Physics2D.Raycast(bounds.min, Vector2.down, distance, platformMask);
        RaycastHit2D resultRight = Physics2D.Raycast(bounds.min + new Vector3(bounds.size.x, 0, 0), 
            Vector2.down, distance, platformMask);
        if (resultLeft.collider == null && resultRight.collider == null) {
            return false;
        }
        return true;
    }

    void FlipLeftRight()
    {

        facingRight = !facingRight; 
        transform.localScale *= new Vector2(-1, 1);
    }

    public void increaseNumJumps() {
        numJumps++;
        jumpCounter++;
    }

    public void setStickWeapon() {
        transform.GetChild(0).GetComponent<Renderer>().enabled = true;
    }

    public void setToStartPosition() {
        transform.position = new Vector2(1, 1);
    }
}
