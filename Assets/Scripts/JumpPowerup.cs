using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPowerup : MonoBehaviour {

    public PlayerBlob player;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player").GetComponent<PlayerBlob>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        player.increaseNumJumps();
        Destroy(this.gameObject);
    }
}
