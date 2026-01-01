using UnityEngine;
using UnityEngine.SceneManagement;

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

    //Differential equations stuff
    public bool useDifferentialMotion = false;

    [SerializeField] private float engineForce = 8000f;
    [SerializeField] private float brakeForceODE = 12000f;
    [SerializeField] private float airDragCoefficient = 0.4257f;
    [SerializeField] private float rollingResistance = 12.8f;

    [SerializeField] private float steeringTorque = 1500f;
    [SerializeField] private float yawInertia = 2500f;

    private Vector3 odeVelocity;
    private Vector3 odePosition;
    private float odeYaw;
    private float odeYawRate;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        carRigidbody.centerOfMass = carCenterOfMassTransform.localPosition;
        originalDrag = carRigidbody.drag;

        odePosition = transform.position;
        odeVelocity = Vector3.zero;
        odeYaw = transform.eulerAngles.y;
        odeYawRate = 0f;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            useDifferentialMotion = !useDifferentialMotion;
            if (useDifferentialMotion) SyncODEFromRigidbody();
        }  
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Main");
        }
    }
    void FixedUpdate()
    {
        GetInput();

        if (useDifferentialMotion)
        {
            DifferentialForceMotion();
            UpdateWheelCollidersODE();
        }
        else
        {
            MotorForce();
            Steering();
            ApplyBrakes();
        }
        UpdateWheels();
        //Debug.Log(Carspeed());
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

    public void DifferentialForceMotion()
    {
        float dt = Time.fixedDeltaTime;
        float mass = carRigidbody.mass;

        Vector3 forward = Quaternion.Euler(0f, odeYaw, 0f) * Vector3.forward;
        Vector3 velocityDir = odeVelocity.normalized;

        //engine force
        Vector3 engine = forward * (engineForce * verticalInput);

        //brake force
        bool braking = Input.GetKey(KeyCode.Space);
        Vector3 brake = braking && odeVelocity.magnitude > 0.1f? -velocityDir * brakeForceODE : Vector3.zero;

        //drag forces
        Vector3 airDrag = -odeVelocity * odeVelocity.magnitude * airDragCoefficient;
        Vector3 rollingDrag = -odeVelocity * rollingResistance;

        //final force
        Vector3 netForce = engine + brake + airDrag + rollingDrag;

        //now we make the differential equations
        Vector3 acceleration = netForce / mass;  //iz F = ma => a = F/m
        odeVelocity += acceleration * dt;   // v = a * dt

        odePosition += odeVelocity * dt; // s = v * dt

        float steerInput = horizontalInput;
        float torque = steeringTorque * steerInput;

        float angularAcceleration = torque / yawInertia;
        odeYawRate += angularAcceleration * dt;

        odeYawRate *= 0.98f;

        odeYaw += odeYawRate * dt;

        if (odeVelocity.magnitude > 0.1f)
        {
            Quaternion yawRotation = Quaternion.Euler(0f, odeYawRate * dt * Mathf.Rad2Deg, 0f);
            odeVelocity = yawRotation * odeVelocity;
        }

        carRigidbody.velocity = odeVelocity;
        carRigidbody.angularVelocity = new Vector3(0f, odeYawRate, 0f);

        odePosition = carRigidbody.position;
        odeYaw = carRigidbody.rotation.eulerAngles.y;

    }

    void SyncODEFromRigidbody()
    {
        odePosition = carRigidbody.position;
        odeVelocity = carRigidbody.velocity;
        odeYaw = carRigidbody.rotation.eulerAngles.y;
        odeYawRate = carRigidbody.angularVelocity.y;
    }

    void UpdateWheelCollidersODE()
    {
        float wheelRPM = odeVelocity.magnitude * 30f;

        frontRightWheelCollider.motorTorque = 0f;
        frontLeftWheelCollider.motorTorque = 0f;

        frontRightWheelCollider.steerAngle = steeringAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = steeringAngle * horizontalInput;

        frontRightWheelCollider.brakeTorque = 0f;
        frontLeftWheelCollider.brakeTorque = 0f;
        backRightWheelCollider.brakeTorque = 0f;
        backLeftWheelCollider.brakeTorque = 0f;
    }
}
