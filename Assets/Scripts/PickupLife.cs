using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupLife : Drop {

	// Use this for initialization
	void Start () {
        Init();   
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    protected override void ActivatePowerup(Player player) {
        player.replenishHealth(1);
    }
}
