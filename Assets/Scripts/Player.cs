using UnityEngine;

public class Player : Character {
    public GameObject Stick;

    public float maxStamina;
    public float stamina;
    public float staminaTimer = 0;
    private float staminaTime = 0.2f;

    public bool exhausted = false;
    public float exhaustedTimer = 0;
    private float exhaustedTime = 1; //seconds

    public int experience = 0;
    public int level = 1;
    public int expToNextLevel;

    protected int attackStaminaUse = 15;
    protected int dashStaminaUse = 10;
    protected float staminaLeniency = 0.75f;

    void Start() {
        color = new Color32(65, 234, 101, 255);
        this.GetComponent<SpriteRenderer>().color = color;
        body = GetComponent<Rigidbody2D>();
        body.freezeRotation = true;
        maxHealth = 100;
        health = maxHealth;
        maxStamina = 100;
        stamina = maxStamina;
        attack = 5;
        defense = 5;
        maxSpeed = 1;
        dashTime = 0.15f;
        jumpAmount = 100f;
        numJumps = 1;
        jumpCounter = numJumps;
        numDashes = 3;
        dashCounter = numDashes;
        recoveryTime = 1f;
        incapacitatedTime = 0.3f;
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
                if (stamina >= attackStaminaUse * staminaLeniency && Input.GetButtonDown("Attack")) {
                    bool isAttacking = equipped.GetChild(0).GetComponent<WeaponAttack>().isAttacking;
                    equipped.GetChild(0).GetComponent<Animator>().SetBool("attack", true);
                    if (!isAttacking) {
                        stamina -= attackStaminaUse;
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

        if (grounded) {
            jumpCounter = numJumps;
            dashCounter = numDashes;
            isDashing = false;
            dashClock = 0;
            body.gravityScale = 1;
        }

        if (!pressedJump) {
            move(move_x);
        } else if (grounded || jumpCounter > 0) {
            jump();
            jumpCounter--;
        }
        //allow dashes only once, only in air
        if (stamina >= dashStaminaUse * staminaLeniency && !grounded && dashCounter > 0 && Input.GetButtonDown("Dash")) {
            isDashing = true;
            dash();
            dashCounter--;
            stamina -= dashStaminaUse;
            if (stamina <= 0) {
                stamina = 0;
                exhausted = true;
            }
        } else if (isDashing) {
            dash();
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
    // jump adjust and stamina timer must happen here
    void FixedUpdate() {
        if (exhausted) {
            exhaustedTimer += Time.deltaTime;
            if (exhaustedTimer >= exhaustedTime) {
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
        jumpAdjust();
    }

    public void increaseNumJumps() {
        numJumps++;
        jumpCounter++;
    }

    public void increaseJumpHeight() {
        jumpAmount += 50f;
    }

    public void increaseMaxHealth() {
        maxHealth += 5;
        health = maxHealth;
    }

    public void increaseSpeed() {
        maxSpeed += 0.5f;
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

    //formula for amount of exp to next level, level^2 / 2 + 2
    public int expFormula() {
        return level * level / 2 + 5;
    }

    public void setStickWeapon() {
        GameObject stick = Instantiate(Stick);
        stick.transform.SetParent(this.transform.Find("Equipped").transform, false);
    }

    public void setToStartPosition() {
        body.velocity = new Vector2(0, 0);
        transform.position = new Vector2(1, 1);
    }

    protected override void DestroyCharacter() {
        health = maxHealth;
        setToStartPosition();
    }
}
