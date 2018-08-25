using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character {
    public GameObject LifeToken;

    protected bool moveLeft = true;

	// Use this for initialization
	void Start () {
        color = new Color32(100, 50, 230, 255);
        this.GetComponent<SpriteRenderer>().color = color;
        body = GetComponent<Rigidbody2D>();
        body.freezeRotation = true;
        maxHealth = 3;
        health = maxHealth;
        attack = 1;
        defense = 0;
        maxSpeed = 10;
        jumpAmount = 20f;
        numJumps = 1;
        jumpCounter = numJumps;
        recoveryTime = 0.5f;
    }

    // Update is called once per frame
    void Update () {
        UpdateCharacterParams();
        if (!grounded) {
            return; //don't move if in air
        }
        int platforms = LayerMask.GetMask("Platforms");

        bool endLeft = !leftBottomCollide(platforms) || leftCollide(platforms);
        bool endRight = !rightBottomCollide(platforms) || rightCollide(platforms);

        if (endLeft != endRight) {
            moveLeft = endRight;
        }

        if (moveLeft) {
            move(-0.1f);
        } else {
            move(0.1f);
        }
	}

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "Player") {
            Player player = collider.GetComponentInParent<Player>();
            player.attackCharacter(this.attack, collider.transform.position - this.transform.position, 120);
        }
    }

    protected override void DestroyCharacter() {
        System.Random rng = new System.Random();
        int numDrops = rng.Next(1, 3);
        for (int i = 0; i < numDrops; ++i) {
            GameObject token = Instantiate(LifeToken);
            token.transform.position = this.transform.position;
            float power = ((float)rng.NextDouble()) * 5 + 5;
            Debug.Log("Created life token with power " + power.ToString());
            int left_force = rng.Next(-9, -4); // -9 to -5 
            int right_force = rng.Next(5, 10); // 5 to 9
            int choose_left = rng.Next(0, 2); // 0 or 1
            int x_dir = 0;
            if (choose_left == 1) {
                x_dir = left_force;
            } else {
                x_dir = right_force;
            }
            int y_dir = rng.Next(5, 10);
            token.GetComponent<Rigidbody2D>().AddForce(power * new Vector2(x_dir, y_dir).normalized);
        }
        base.DestroyCharacter();
    }
}
