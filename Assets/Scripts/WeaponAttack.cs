using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttack : MonoBehaviour {

    public Player player;

    // Use this for initialization
    void Start () {
        player = GameObject.Find("Player").GetComponent<Player>();

    }

    // Update is called once per frame
    void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "Enemy") {
            Character toAttack = collider.GetComponentInParent<Character>();
            toAttack.attackCharacter(player.attack, toAttack.transform.position - this.transform.position, 120);
        }
    }
}
