using UnityEngine;
using UnityEngine.SceneManagement;

using Utils;

public class Rocket : MonoBehaviour {

    Rigidbody rigidbody;
    AudioSource thrustAudio;
    [SerializeField] private float rcsThrust = 200f;
    [SerializeField] private float mainThrust = 200f;
    State currentState = State.Alive;

    // Start is called before the first frame update
    void Start() {
        rigidbody = GetComponent<Rigidbody>();
        thrustAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        if (currentState == State.Alive) {
            ProcessInput();
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (currentState != State.Alive) return;

        switch (collision.gameObject.tag) {
            case "Friendly":
                print("nothing");
                break;
            case "Finish":
                currentState = State.Transcending;
                Invoke("LoadNextScene", 1f);
                break;
            default:
                print("dead");
                currentState = State.Dying;
                Invoke("ResetLevel", 1f);
                break;
        }
    }

    private void ResetLevel() {
        SceneManager.LoadScene(0);
    }

    private void LoadNextScene() {
        SceneManager.LoadScene(1);
    }

    private void ProcessInput() {
        Thrusting();
        Rotating();
    }

    private void Rotating() {
        float adjustFrame = Time.deltaTime * rcsThrust;
        if (!Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A)) {
            rigidbody.freezeRotation = true;
            transform.Rotate(Vector3.forward * adjustFrame);
        }
        if (!Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)) {
            rigidbody.freezeRotation = true;
            transform.Rotate(-Vector3.forward * adjustFrame);
        }
        rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionZ;
    }

    private void Thrusting() {
        float adjustFrame = Time.deltaTime * mainThrust;
        if (Input.GetKey(KeyCode.Space)) {

            rigidbody.AddRelativeForce(Vector3.up * adjustFrame);
            if (!thrustAudio.isPlaying) {
                thrustAudio.Play();
            }
        }

        if (Input.GetKeyUp(KeyCode.Space)) {
            thrustAudio.Stop();
        }
    }
}
