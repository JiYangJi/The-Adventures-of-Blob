using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character {
    public GameObject Stick;

    public float maxStamina;
    public float stamina;
    public float staminaTimer = 0;
    private float staminaTime = 0.2f;

    public bool exhausted = false;
    public float exhaustedTimer = 0;
    private float exhaustedTime = 2; //seconds

    public int experience = 0;
    public int level = 1;
    public int expToNextLevel;

    // Use this for initialization
    void Start() {
        color = new Color32(65, 234, 101, 255);
        this.GetComponent<SpriteRenderer>().color = color;
        body = GetComponent<Rigidbody2D>();
        body.freezeRotation = true;
        maxHealth = 10;
        health = maxHealth;
        maxStamina = 100;
        stamina = maxStamina;
        attack = 1;
        defense = 1;
        maxSpeed = 3;
        jumpAmount = 300f;
        numJumps = 1;
        jumpCounter = numJumps;
        recoveryTime = 1f;
        expToNextLevel = expFormula();
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("PhysicsObject"), LayerMask.NameToLayer("PhysicsObject"));
    }

    void Update() {
        UpdateCharacterParams();
        float move_x = Input.GetAxis("Horizontal");
        bool pressedJump = Input.GetButtonDown("Jump");

        Transform equipped = transform.Find("Equipped");
        if (equipped != null && equipped.childCount > 0) {
            if (Input.GetButtonDown("Attack")) {
                if (stamina != 0 && Input.GetButtonDown("Attack")) {
                    bool isAttacking = equipped.GetChild(0).GetComponent<WeaponAttack>().isAttacking;
                    equipped.GetChild(0).GetComponent<Animator>().SetBool("attack", true);
                    if (!isAttacking) {
                        stamina -= 20;
                        if (stamina <= 0) {
                            stamina = 0;
                            exhausted = true;
                        }
                    }
                } 
            } else {
                equipped.GetChild(0).GetComponent<Animator>().SetBool("attack", false);
            }
        }
        if (exhausted) {
            exhaustedTimer += Time.deltaTime;
            if (exhaustedTimer >= exhaustedTime) {
                Debug.Log("Exhausted timer is now at " + exhaustedTimer.ToString());
                exhausted = false;
                exhaustedTimer = 0;
            }
        } else {
            staminaTimer += Time.deltaTime;
            if (staminaTimer >= staminaTime) {
                stamina += 1;
                if (stamina > maxStamina) {
                    stamina = maxStamina;
                }
                staminaTimer = 0;
            }
        }

        if (grounded) {
            jumpCounter = numJumps;
        }

        if (!pressedJump) {
            move(move_x);
        } else if (grounded || jumpCounter > 0) {
            jump();
            jumpCounter--;
        }

    }
    //don't jump as high if the jump key is let go
    //fall fast
    void jumpAdjust() {
        if (body.velocity.y <= 0) {
            body.velocity -= new Vector2(0, 1.2f);
        } else if (body.velocity.y > 0 && !Input.GetButton("Jump")) {
            body.velocity -= new Vector2(0, 1.2f);
        }
    }

    // FixedUpdate not necessarily the same as frame update
    void FixedUpdate() {
        jumpAdjust();
    }

    public void increaseNumJumps() {
        numJumps++;
        jumpCounter++;
    }

    public void increaseJumpHeight() {
        jumpAmount += 60f;
    }

    public void increaseMaxHealth() {
        maxHealth += 5;
        health = maxHealth;
    }

    public void replenishHealth(int amount) {
        health += amount;
        if (health > maxHealth) {
            health = maxHealth;
        }
    }

    public void gainExperience(int amount) {
        experience += amount;
        if (experience >= expToNextLevel) { 
            //Level up:
            int leftover = experience - expToNextLevel;
            experience = leftover;
            ++level;
            ++attack;
            ++defense;
            maxStamina += 5;
            expToNextLevel = expFormula();
        }
    }

    //formula for amount of exp to next level, level^2
    public int expFormula() {
        return level * level + 1;
    }

    public void setStickWeapon() {
        GameObject stick = Instantiate(Stick);
        stick.transform.SetParent(this.transform.Find("Equipped").transform, false);
    }

    public void setToStartPosition() {
        body.velocity = new Vector2(0, 0);
        transform.position = new Vector2(1, 1);
    }
}
