using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUpdate : MonoBehaviour {
    public Player player;
    public Slider healthbar;
    public RectTransform bar;

    // Use this for initialization
    void Start () {
        player = GameObject.Find("Player").GetComponent<Player>();
        healthbar = GetComponent<Slider>();
        bar = healthbar.GetComponentInChildren<RectTransform>();
        healthbar.maxValue = player.maxHealth;
        healthbar.value = player.health;
        bar.rect.Set(bar.rect.x, bar.rect.y, healthbar.maxValue * 10, bar.rect.height);
    }

    // Update is called once per frame
    void Update () {
        healthbar.maxValue = player.maxHealth;
        healthbar.value = player.health;
        bar.rect.Set(bar.rect.x, bar.rect.y, healthbar.maxValue * 10, bar.rect.height);
    }
}
