using System;
using Surreily.SomeWords.Model.Game;
using UnityEngine;

public class MovableBehaviour : MonoBehaviour
{
    [SerializeField]
    public float speed;

    [SerializeField]
    public float distance;

    private Vector3 startPosition;
    private Vector3 endPosition;
    private float currentDistance;

    private void Start() {
        startPosition = transform.position;
        endPosition = transform.position;
        currentDistance = 1f;
    }

    void Update()
    {
        if (currentDistance < 1f) {
            currentDistance = currentDistance + speed * Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, endPosition, currentDistance);
        }
    }

    public void Move(Direction direction) {
        startPosition = endPosition; // Set new start position to old end position.
        currentDistance = 0f;

        switch (direction) {
            case Direction.Left:
                endPosition = startPosition + (Vector3.left * distance);
                break;
            case Direction.Right:
                endPosition = startPosition + (Vector3.right * distance);
                break;
            case Direction.Up:
                endPosition = startPosition + (Vector3.up * distance);
                break;
            case Direction.Down:
                endPosition = startPosition + (Vector3.down * distance);
                break;
            default:
                throw new ArgumentException("Unsupported direction.", nameof(direction));
        }
    }
}
