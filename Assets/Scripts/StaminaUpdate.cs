using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaUpdate : MonoBehaviour {
    public Player player;
    public Slider staminabar;
    public RectTransform bar;

    // Use this for initialization
    void Start() {
        player = GameObject.Find("Player").GetComponent<Player>();
        staminabar = GetComponent<Slider>();
        bar = staminabar.GetComponent<RectTransform>();
        staminabar.maxValue = player.maxStamina;
        staminabar.value = player.stamina;
        bar.sizeDelta = new Vector2(staminabar.maxValue, bar.rect.height);
    }

    // Update is called once per frame
    void Update() {
        staminabar.maxValue = player.maxStamina;
        staminabar.value = player.stamina;
        bar.sizeDelta = new Vector2(staminabar.maxValue, bar.rect.height);
    }
}