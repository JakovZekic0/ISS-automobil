using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Wheel Colliders")]
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider backRightWheelCollider;
    [SerializeField] private WheelCollider backLeftWheelCollider;

    [Header("Wheel Transforms")]
    [SerializeField] private Transform frontRightWheelTransform;
    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform backRightWheelTransform;
    [SerializeField] private Transform backLeftWheelTransform;

    [Header("Car Physics")]
    [SerializeField] private Transform carCenterOfMassTransform;
    [SerializeField] private Rigidbody carRigidbody;

    [Header("Car Settings")]
    [SerializeField] private float motorForce = 100f;
    [SerializeField] private float steeringAngle = 30f;
    [SerializeField] private float brakeForce = 1000f;

    [Header("Brake Drag")]
    [SerializeField] private float brakeDrag = 1f;   // Inspector-controlled drag while braking

    private float verticalInput;
    private float horizontalInput;
    private float originalDrag;
    private Rigidbody rigidbody;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        carRigidbody.centerOfMass = carCenterOfMassTransform.localPosition;
        originalDrag = carRigidbody.drag;
    }

    void FixedUpdate()
    {
        GetInput();
        MotorForce();
        Steering();
        ApplyBrakes();
        UpdateWheels();
        Debug.Log(Carspeed());
    }

    void GetInput()
    {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
    }

    void ApplyBrakes()
    {
        bool isBraking = Input.GetKey(KeyCode.Space);
        float currentBrakeForce = isBraking ? brakeForce : 0f;

        frontRightWheelCollider.brakeTorque = currentBrakeForce;
        frontLeftWheelCollider.brakeTorque = currentBrakeForce;
        backRightWheelCollider.brakeTorque = currentBrakeForce;
        backLeftWheelCollider.brakeTorque = currentBrakeForce;

        // Apply ONLY the serialized brake drag while braking
        carRigidbody.drag = isBraking ? brakeDrag : originalDrag;
    }

    void MotorForce()
    {
        frontRightWheelCollider.motorTorque = motorForce * verticalInput;
        frontLeftWheelCollider.motorTorque = motorForce * verticalInput;
    }

    void Steering()
    {
        frontRightWheelCollider.steerAngle = steeringAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = steeringAngle * horizontalInput;
    }

    void UpdateWheels()
    {
        RotateWheel(frontRightWheelCollider, frontRightWheelTransform);
        RotateWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        RotateWheel(backRightWheelCollider, backRightWheelTransform);
        RotateWheel(backLeftWheelCollider, backLeftWheelTransform);
    }

    void RotateWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.position = pos;
        wheelTransform.rotation = rot;
    }
    public float Carspeed()
    {
        float speed = rigidbody.velocity.magnitude*2.23693629f;
        return speed;
    }
}
