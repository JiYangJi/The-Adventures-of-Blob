using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobController : MonoBehaviour {
    public enum direction { left, right, up, down};

    public float maxSpeed = 10;
    public float jump = 30f;
    public direction facing = direction.right;
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

    // FixedUpdate not necessarily the same as frame update
    void FixedUpdate()
    {
        float move_x = Input.GetAxis("Horizontal");
        float move_y = Input.GetAxis("Vertical");
        if (move_y == 0) {
            stoppedUp = true;
        }
        bool grounded = isGrounded();
        if (grounded) {
            jumpCounter = numJumps;
        }

        if ((grounded || jumpCounter > 0) && stoppedUp && move_y > 0.001)
        {
            body.velocity = new Vector2(move_x * maxSpeed, jump);
            stoppedUp = false;
            jumpCounter--;
        }
        else {
            body.velocity = new Vector2(move_x * maxSpeed, body.velocity.y);
        }

        if (move_x > 0 && facing == direction.left)
        {
            FlipLeftRight();
        }
        else if (move_x < 0 && facing == direction.right)
        {
            FlipLeftRight();
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

    //ground colliding
    private void OnCollisionEnter2D(Collision2D collision)
    {
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
    }

    void FlipLeftRight()
    {
        if (facing == direction.left)
        {
            facing = direction.right;
        } 
        else if (facing == direction.right)
        {
            facing = direction.left;
        }

        transform.localScale *= new Vector2(-1, 1);
    }

    public void increaseNumJumps() {
        numJumps++;
    }

    public void setToStartPosition() {
        transform.position = new Vector2(1, 1);
    }
}
