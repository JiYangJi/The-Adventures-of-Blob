using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupExperience : Drop {

    void Start() {
        Init();
    }

    void Update() {}

    protected override void ActivatePowerup(Player player) {
        player.gainExperience(1);
    }
}