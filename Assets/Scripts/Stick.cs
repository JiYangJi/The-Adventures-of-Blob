using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : MonoBehaviour {

    public BlobController player;

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<BlobController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        player.setStickWeapon();
        Destroy(this.gameObject);
    }
}