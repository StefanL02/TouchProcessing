using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Camera Feature Toggles")]
    public bool isRotatingAroundItself = true;
    public bool isGyro = false;
    public bool isZoom = true;
    public bool isMoving = true;
    public bool isAccelerometer = false;

    [Header("Pan Settings")]
    [SerializeField] private float panSpeed = 0.0025f;

    [Header("Accelerometer Settings")]
    [SerializeField] private float accelerometerMoveSpeed = 2f;

    [Header("Zoom Settings")]
    [SerializeField] private float zoomSpeed = 0.08f;
    [SerializeField] private float minFov = 20f;
    [SerializeField] private float maxFov = 80f;

    [Header("Gyro Settings")]
    [SerializeField] private float gyroSmoothing = 3f;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private float startFov;

    private Quaternion rotationAtTwistStart;
    private float startAngle;

    private float currentPitch = 0f;
    private float currentYaw = 0f;

    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;

        Vector3 euler = transform.rotation.eulerAngles;
        currentPitch = NormalizeAngle(euler.x);
        currentYaw = NormalizeAngle(euler.y);

        if (Camera.main != null)
            startFov = Camera.main.fieldOfView;

        Input.gyro.enabled = true;
    }

    void Update()
    {
        if (isGyro)
        {
            Quaternion deviceRotation = GyroToUnity(Input.gyro.attitude);

            // Smooth toward device rotation
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                deviceRotation,
                gyroSmoothing * Time.deltaTime
            );
        }

        if (isAccelerometer)
        {
            Vector3 acc = Input.acceleration;

            // Move camera sideways and forward/back based on tilt
            Vector3 move = new Vector3(acc.x, 0f, acc.y) * accelerometerMoveSpeed * Time.deltaTime;
            transform.Translate(move, Space.World);
        }
    }

    private Quaternion GyroToUnity(Quaternion q)
    {
        // Convert right-handed device coordinates to Unity coordinates
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }

    public void BeginRotate(float currentAngle)
    {
        if (!isRotatingAroundItself || isGyro) return;

        rotationAtTwistStart = transform.rotation;
        startAngle = currentAngle;
    }

    public void UpdateRotate(float currentAngle)
    {
        if (!isRotatingAroundItself || isGyro) return;

        float angleDelta = Mathf.DeltaAngle(startAngle, currentAngle);

        transform.rotation =
            rotationAtTwistStart *
            Quaternion.AngleAxis(angleDelta, Vector3.forward);
    }

    public void Pan(Vector2 deltaPixels)
    {
        if (!isMoving) return;

        Vector3 move = new Vector3(
            -deltaPixels.x * panSpeed,
            -deltaPixels.y * panSpeed,
            0f
        );

        transform.Translate(move, Space.Self);
    }

    public void Zoom(float pinchDeltaPixels)
    {
        if (!isZoom) return;
        if (Camera.main == null) return;

        Camera.main.fieldOfView -= pinchDeltaPixels * zoomSpeed;
        Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, minFov, maxFov);
    }

    public void ResetCamera()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;

        Vector3 euler = startRotation.eulerAngles;
        currentPitch = NormalizeAngle(euler.x);
        currentYaw = NormalizeAngle(euler.y);

        if (Camera.main != null)
            Camera.main.fieldOfView = startFov;
    }

    public void LookAround(Vector2 delta)
    {
        if (!isRotatingAroundItself || isGyro) return;

        float pitch = -delta.y * 0.2f;
        float yaw = delta.x * 0.2f;

        currentPitch += pitch;
        currentYaw += yaw;

        currentPitch = Mathf.Clamp(currentPitch, -60f, 60f);

        transform.rotation = Quaternion.Euler(currentPitch, currentYaw, 0f);
    }

    private float NormalizeAngle(float angle)
    {
        while (angle > 180f) angle -= 360f;
        while (angle < -180f) angle += 360f;
        return angle;
    }
}