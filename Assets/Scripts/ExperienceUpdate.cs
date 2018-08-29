using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceUpdate : MonoBehaviour {
    public Player player;
    public Slider experiencebar;
    public RectTransform bar;

    // Use this for initialization
    void Start() {
        player = GameObject.Find("Player").GetComponent<Player>();
        experiencebar = GetComponent<Slider>();
        bar = experiencebar.GetComponent<RectTransform>();
        experiencebar.maxValue = player.expToNextLevel;
        experiencebar.value = player.experience;
    }

    // Late update so it is after gameplay
    void LateUpdate() {
        experiencebar.maxValue = player.expToNextLevel;
        experiencebar.value = player.experience;
    }
}
