using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobController : MonoBehaviour {
    public bool facingLeft = true;
    public float maxSpeed = 10;
    public float jumpAmount = 30f;
    public bool stoppedUp = true;
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
        if (isGrounded())
        {
            GetComponent<Animator>().SetBool("isJumping", false);
        }
        else
        {
            GetComponent<Animator>().SetBool("isJumping", true);
        }
    }

    void move(float amount) {
        body.velocity = new Vector2(amount * maxSpeed, body.velocity.y);
        checkFlip(amount);
    }

    void checkFlip(float amount) {
        if ((amount > 0 && !facingLeft) || (amount < 0 && facingLeft))
        {
            FlipLeftRight();
        }
    }

    void jump() {
        body.velocity = new Vector2(body.velocity.x, jumpAmount);
    }

    // FixedUpdate not necessarily the same as frame update
    void FixedUpdate()
    {
        float move_x = Input.GetAxis("Horizontal");
        bool pressedJump =  Input.GetButtonDown("Jump");

        if (!Input.GetButtonUp("Jump"))
        {
            stoppedUp = true;
        }
        else
        {
            stoppedUp = false; 
        } 
        bool grounded = isGrounded();
        if (grounded) {
            jumpCounter = numJumps;
        }

        if (!pressedJump)
        {
            move(move_x);
        }

        if (pressedJump && stoppedUp && (grounded || jumpCounter > 0)) {
            jump();
            jumpCounter--;
        }
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

        facingLeft = !facingLeft; 
        transform.localScale *= new Vector2(-1, 1);
    }

    public void increaseNumJumps() {
        numJumps++;
    }

    public void setToStartPosition() {
        transform.position = new Vector2(1, 1);
    }
}
