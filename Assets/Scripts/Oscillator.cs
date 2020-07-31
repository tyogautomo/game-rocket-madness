using UnityEngine;


[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour {

    [SerializeField] Vector3 maxMovementVector;
    // TODO move this later
    [SerializeField] [Range(0, 1)] float movementFactor;
    Vector3 startingPosition;

    void Start() {
        startingPosition = transform.position;
    }

    void Update() {
        Vector3 offset = maxMovementVector * movementFactor;
        transform.position = startingPosition + offset;
    }
}
