using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    Vector2 offset;
    Transform player;

    // Use this for initialization
    void Start() {
        player = GameObject.Find("Player").transform;
    }

    //Called after Update, resets camera
    void LateUpdate() {
        Vector3 camera = player.position;
        //disregard player's z coordinate
        camera.z = transform.position.z;
        transform.position = camera;
    }
}
