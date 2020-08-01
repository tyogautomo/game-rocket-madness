using UnityEngine;
using UnityEngine.SceneManagement;

using Utils;

public class Rocket : MonoBehaviour {

    Rigidbody rigidbody;
    AudioSource audioSource;
    State currentState = State.Alive;
    bool isCollided = true;
    [SerializeField] float loadDelay = 1f;
    [SerializeField] float rcsThrust = 200f;
    [SerializeField] float mainThrust = 200f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip explodeSound;
    [SerializeField] AudioClip successSound;
    [SerializeField] ParticleSystem engineParticles;
    [SerializeField] ParticleSystem explodeParticles;
    [SerializeField] ParticleSystem successParticles;

    // Start is called before the first frame update
    void Start() {
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
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
                print("friendly");
                break;
            case "Finish":
                ExcecuteFinish();
                break;
            default:
                if (isCollided) ExcecuteDeath();
                break;
        }
    }
    private void ExcecuteFinish() {
        audioSource.Stop();
        engineParticles.Stop();

        successParticles.Play();
        audioSource.PlayOneShot(successSound);
        currentState = State.Transcending;
        Invoke("LoadNextScene", loadDelay);
    }

    private void ExcecuteDeath() {
        audioSource.Stop();
        engineParticles.Stop();

        explodeParticles.Play();
        audioSource.PlayOneShot(explodeSound);
        currentState = State.Dying;
        Invoke("ResetLevel", loadDelay);
    }


    private void ResetLevel() {
        SceneManager.LoadScene(0);
    }

    private void LoadNextScene() {
        if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1) {
            SceneManager.LoadScene(0);
        } else {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    private void ProcessInput() {
        Thrusting();
        Rotating();
        if (Debug.isDebugBuild) {
            DebuggerKey();
        }
    }

    private void Thrusting() {
        float adjustFrame = Time.deltaTime * mainThrust;
        if (Input.GetKey(KeyCode.Space)) {
            rigidbody.AddRelativeForce(Vector3.up * adjustFrame);
            if (!audioSource.isPlaying) {
                audioSource.PlayOneShot(mainEngine);
            }
            engineParticles.Play();
        }

        if (Input.GetKeyUp(KeyCode.Space)) {
            engineParticles.Stop();
            audioSource.Stop();
        }
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

    private void DebuggerKey() {
        if (Input.GetKeyDown(KeyCode.L)) {
            LoadNextScene();
        } else if (Input.GetKeyDown(KeyCode.C)) {
            isCollided = !isCollided;
        }
    }

}
