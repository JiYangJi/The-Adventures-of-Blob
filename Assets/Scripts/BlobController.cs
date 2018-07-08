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
    }

    void Update()
    {
        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.01)
        {
            GetComponent<Animator>().SetBool("isMoving", true);
        }
        else {
            GetComponent<Animator>().SetBool("isMoving", false);
        }
        GetComponent<Animator>().SetBool("isJumping", !isGrounded());
        this.transform.GetChild(0).GetComponent<Animator>().SetBool("attack", Input.GetButtonDown("Attack")); 
         
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
        float move_x = Input.GetAxis("Horizontal");
        bool pressedJump =  Input.GetButtonDown("Jump"); 
        bool grounded = isGrounded();
        if (grounded) {
            jumpCounter = numJumps;
        }

        if (!pressedJump)
        {
            move(move_x);
        }

        if (pressedJump && (grounded || jumpCounter > 0)) {
            jump();
            jumpCounter--;
        }
        jumpAdjust();
    }

    private bool isGrounded() {
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        RaycastHit2D[] results = new RaycastHit2D[1];
        //check left and right, use pythagoras
        float diagonal = Mathf.Sqrt(Mathf.Pow(box.size.y / 2f, 2) + Mathf.Pow(box.size.x / 2f, 2));
        float horizontal = box.size.x / box.size.y * 0.90f;

        int left = box.Raycast(new Vector2(-horizontal, -1), results, diagonal);
        int right = box.Raycast(new Vector2(horizontal, -1), results, diagonal);
        if (left + right > 0)
        {
            return true;
        }
        return false;
    }

    void FlipLeftRight()
    {

        facingRight = !facingRight; 
        transform.localScale *= new Vector2(-1, 1);
    }

    public void increaseNumJumps() {
        numJumps++;
    }

    public void setToStartPosition() {
        transform.position = new Vector2(1, 1);
    }
}
