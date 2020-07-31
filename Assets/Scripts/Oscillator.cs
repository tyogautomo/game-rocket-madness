using UnityEngine;


[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour {

    [SerializeField] Vector3 maxMovementVector;
    [SerializeField] float periodDivider = 2f;
    // TODO move serialize later
    [SerializeField] [Range(0, 1)] float movementFactor;
    Vector3 startingPosition;

    void Start() {
        startingPosition = transform.position;
    }

    void Update() {
        if (periodDivider <= Mathf.Epsilon) return;

        float period = Time.time / periodDivider;
        const float tau = Mathf.PI * 2;

        float rawSin = Mathf.Sin(period * tau);

        movementFactor = (rawSin / 2f) + 0.5f;

        Vector3 offset = maxMovementVector * movementFactor;
        transform.position = startingPosition + offset;
    }
}
