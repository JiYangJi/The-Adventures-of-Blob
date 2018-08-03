using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttack : MonoBehaviour {

    public Player player;

    // Use this for initialization
    void Start () {
        Debug.Log("Reached start!");
        player = GameObject.Find("Player").GetComponent<Player>();

    }

    // Update is called once per frame
    void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log("Reached collision!");
        if (collision.tag == "Enemy") {
            Debug.Log("Reached enemy!");
            Character toAttack = collision.GetComponentInParent<Character>();
            toAttack.attackCharacter(player.attack, toAttack.transform.position - this.transform.position, 120);
            Debug.Log("This Position: " + transform.position.ToString());
        }
    }
}
