using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Camera Feature Toggles")]
    public bool isRotatingAroundItself = true;
    public bool isGyro = false;
    public bool isZoom = true;
    public bool isMoving = true;

    [Header("Pan Settings")]
    [SerializeField] private float panSpeed = 0.0025f;

    [Header("Zoom Settings")]
    [SerializeField] private float zoomSpeed = 0.08f;
    [SerializeField] private float minFov = 20f;
    [SerializeField] private float maxFov = 80f;

    [Header("Gyro Settings")]
    [SerializeField] private float gyroRotationSpeed = 0.5f;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private float startFov;

    private Quaternion rotationAtTwistStart;
    private float startAngle;
    private float currentPitch = 0f;
    

    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;

        if (Camera.main != null)
            startFov = Camera.main.fieldOfView;

        Input.gyro.enabled = true;
    }

    void Update()
    {
        if (isGyro)
        {
            float rotationSpeed = -Input.gyro.rotationRateUnbiased.z;
            transform.Rotate(0f, 0f, rotationSpeed * gyroRotationSpeed);
        }
    }

    public void BeginRotate(float currentAngle)
    {
        if (!isRotatingAroundItself) return;

        rotationAtTwistStart = transform.rotation;
        startAngle = currentAngle;
    }

    public void UpdateRotate(float currentAngle)
    {
        if (!isRotatingAroundItself) return;

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

        if (Camera.main != null)
            Camera.main.fieldOfView = startFov;
    }

    public void LookAround(Vector2 delta)
    {
        if (!isRotatingAroundItself) return;

        float pitch = -delta.y * 0.2f;
        float yaw = delta.x * 0.2f;

        currentPitch += pitch;
        currentPitch = Mathf.Clamp(currentPitch, -60f, 60f);

        transform.rotation = Quaternion.Euler(currentPitch, transform.eulerAngles.y + yaw, 0);
    }

}