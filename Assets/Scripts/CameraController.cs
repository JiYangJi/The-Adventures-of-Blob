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
    void FixedUpdate() {
        Vector3 camera = Vector3.Lerp(transform.position, player.transform.position, 0.05f);
        //disregard player's z coordinate
        camera.z = transform.position.z;
        transform.position = camera;
        float dist = Mathf.Sqrt(Vector2.Distance(transform.position, player.transform.position));
        float newSize = 5 + dist;
        Debug.Log("Distance is: " + dist);
        float interp = 0;
        if (dist > 1) {
            interp = 0.01f;
        } else if (dist > 0.8) {
            interp = 0.02f;
        } else if (dist > 0.6) {
            interp = 0.03f;
        } else if (dist > 0.4) {
            interp = 0.04f;
        } else if (dist > 0.2) {
            interp = 0.05f;
        } else if (dist > 0.1) {
            interp = 0.06f;
        } else if (dist > 0.05) {
            interp = 0.07f;
        } else if (dist > 0.01) {
            interp = 0.08f;
        } else if (dist > 0.005) {
            interp = 0.09f;
        } else if (dist > 0.001) {
            interp = 0.1f;
        } else {
            interp = 0.2f;
        }
        GetComponent<Camera>().orthographicSize = Mathf.Lerp(GetComponent<Camera>().orthographicSize, newSize, interp);
    }
}
