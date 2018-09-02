using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Drop : Powerup {
    public static System.Random rng = Enemy.rng;

    // Use this for initialization
    protected void Init () {
        float power = ((float)rng.NextDouble()) * 5 + 5;
        Debug.Log("Created  drop with power " + power.ToString());
        int x_dir = rng.Next(-9, 10); // -9 to 9
        int y_dir = rng.Next(5, 10);
        this.GetComponent<Rigidbody2D>().AddForce(power * new Vector2(x_dir, y_dir).normalized);
        this.GetComponent<Rigidbody2D>().AddTorque(((float)rng.NextDouble()) * 0.1f - 0.05f);
    }

    private void Start() {
    }

    // Update is called once per frame
    void Update () { 	
	}
}
