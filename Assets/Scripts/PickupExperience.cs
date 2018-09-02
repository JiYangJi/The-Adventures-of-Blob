using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupExperience : Drop {

    // Use this for initialization
    void Start() {
        Init();
    }

    // Update is called once per frame
    void Update() {

    }

    protected override void ActivatePowerup(Player player) {
        player.gainExperience(1);
    }
}