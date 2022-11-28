using UnityEngine;

public class PulseAnimationBehaviour : MonoBehaviour {
    public float Speed { get; set; }
    public float Scale { get; set; }

    private float currentPosition;

    private void Start() {
        currentPosition = 1f;
    }

    private void Update() {
        if (currentPosition < 1f) {
            currentPosition = currentPosition + Speed * Time.deltaTime;
            transform.localScale = Vector3.Lerp(Vector3.one * Scale, Vector3.one, currentPosition);
        }
    }

    public void Pulse() {
        currentPosition = 0f;
    }
}