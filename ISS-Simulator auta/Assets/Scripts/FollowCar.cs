using UnityEngine;

public class FollowCar : MonoBehaviour
{
    [Header("Targets")]
    [SerializeField] private Transform carTransform;
    [SerializeField] private Transform thirdPersonPoint;
    [SerializeField] private Transform firstPersonPoint;

    [Header("Camera Settings")]
    [SerializeField] private float smoothTime = 0.2f;

    private Vector3 velocity = Vector3.zero;
    private bool firstPerson = false;

    void Update()
    {
        // Toggle view with V key
        if (Input.GetKeyDown(KeyCode.V))
        {
            firstPerson = !firstPerson;

            // Reset smoothing when switching views
            velocity = Vector3.zero;
        }
    }

    void LateUpdate()
    {
        if (firstPerson)
        {
            // FIRST PERSON: hard lock (no lag)
            transform.position = firstPersonPoint.position;
            transform.rotation = firstPersonPoint.rotation;
        }
        else
        {
            // THIRD PERSON: smooth follow
            transform.position = Vector3.SmoothDamp(
                transform.position,
                thirdPersonPoint.position,
                ref velocity,
                smoothTime
            );

            transform.LookAt(carTransform);
        }
    }
}
