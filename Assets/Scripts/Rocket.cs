using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    Rigidbody rigidbody;
    AudioSource thrustAudio;

    // Start is called before the first frame update
    void Start() {
        rigidbody = GetComponent<Rigidbody>();
        thrustAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        ProcessInput();
    }

    private void ProcessInput() {
        if (Input.GetKey(KeyCode.Space)) {
            rigidbody.AddRelativeForce(Vector3.up);
            if (!thrustAudio.isPlaying) {
                thrustAudio.Play();
            }
        }

        if (Input.GetKeyUp(KeyCode.Space)) {
            thrustAudio.Stop();
        }

        if (!Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A)) {
            transform.Rotate(Vector3.forward);
        }
        if (!Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)) {
            transform.Rotate(-Vector3.forward);
        }
    }
}
