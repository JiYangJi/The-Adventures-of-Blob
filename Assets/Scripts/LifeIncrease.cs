﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeIncrease : Powerup {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    protected override void ActivatePowerup(Player player) {
        player.increaseMaxHealth();
    }
}
