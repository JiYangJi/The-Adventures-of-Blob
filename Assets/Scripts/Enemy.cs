using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character {
    public GameObject LifeToken;
    public GameObject ExperienceToken;
    public static System.Random rng = new System.Random();

    protected bool moveLeft = true;

	// Use this for initialization
	void Start () {
        color = new Color32(100, 50, 230, 255);
        this.GetComponent<SpriteRenderer>().color = color;
        body = GetComponent<Rigidbody2D>();
        body.freezeRotation = true;
        maxHealth = 30;
        health = maxHealth;
        attack = 5;
        defense = 1;
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
        //can't attack while incapacitated
        if (!incapacitated && collider.tag == "Player") {
            Player player = collider.GetComponentInParent<Player>();
            player.attackCharacter(this.attack, collider.transform.position - this.transform.position, 120);
        }
    }

    protected void GenerateDrop(int num, GameObject dropType) {
        for (int i = 0; i < num; ++i) {
            GameObject token = Instantiate(dropType);
            token.transform.position = this.transform.position;
        }
    }

    protected override void DestroyCharacter() {
        GenerateDrop(rng.Next(0, 3), LifeToken);
        GenerateDrop(rng.Next(1, 5), ExperienceToken);
        base.DestroyCharacter();
    }
}
