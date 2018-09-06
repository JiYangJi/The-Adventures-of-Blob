using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpIncrease : Powerup {

    void Start() {}

    void Update() {}

    protected override void  ActivatePowerup(Player player) {
        player.increaseJumpHeight();
    }
}