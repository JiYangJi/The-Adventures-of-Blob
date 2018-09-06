using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupLife : Drop {

	void Start () {
        Init();   
	}
	
	void Update () {}

    protected override void ActivatePowerup(Player player) {
        player.replenishHealth(1);
    }
}
