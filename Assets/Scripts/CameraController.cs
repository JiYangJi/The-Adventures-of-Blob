using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    Vector2 offset;
    Transform player;

    // Use this for initialization
    void Start () {
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update () {
		
	}

    //Called after Update, resets camera
    void LateUpdate()
    {
        Vector3 playerpos = player.position;
        playerpos.z = transform.position.z;
        transform.position = playerpos;
    }
}
