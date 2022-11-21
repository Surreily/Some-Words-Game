using UnityEngine;

public class CameraMovement : MonoBehaviour {
    [SerializeField]
    private Vector3 Offset;

    [SerializeField]
    private float Speed;

    private GameObject target;

    public void LateUpdate() {
        if (target != null) {
            Vector3 targetPosition = target.transform.position + Offset;

            Vector3 finalPosition = Vector3.Lerp(transform.position, targetPosition, Speed);

            finalPosition.z = transform.position.z;

            transform.position = finalPosition;
        }
    }

    public void Target(GameObject target) {
        this.target = target;
    }

    public void Untarget() {
        target = null;
    }
}
