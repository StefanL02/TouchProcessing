using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Orbit Around This Point")]
    [SerializeField] private Transform rotationPoint;

    [Header("Pan Settings")]
    [SerializeField] private float panSpeed = 0.0025f;

    [Header("Zoom Settings")]
    [SerializeField] private float zoomSpeed = 0.08f;
    [SerializeField] private float minFov = 20f;
    [SerializeField] private float maxFov = 80f;

    // rotation
    private Quaternion rotationAtTwistStart;
    private float startAngle;
    private float currentPitch = 0f;

    public void BeginRotate(float currentAngle)
    {
        rotationAtTwistStart = transform.rotation;
        startAngle = currentAngle;
    }

    public void UpdateRotate(float currentAngle)
    {
        float angleDelta = Mathf.DeltaAngle(startAngle, currentAngle);

        transform.rotation =
            rotationAtTwistStart *
            Quaternion.AngleAxis(angleDelta, Vector3.forward);
    }


// pan
public void Pan(Vector2 deltaPixels)
    {
        Vector3 move = new Vector3(
            -deltaPixels.x * panSpeed,
            -deltaPixels.y * panSpeed,
            0f
        );

        transform.Translate(move, Space.Self);
    }

    // zoom
    public void Zoom(float pinchDeltaPixels)
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        cam.fieldOfView -= pinchDeltaPixels * zoomSpeed;
        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, minFov, maxFov);
    }

    public void LookAround(Vector2 delta)
    {
        float pitch = -delta.y * 0.2f;
        float yaw = delta.x * 0.2f;

        currentPitch += pitch;
        currentPitch = Mathf.Clamp(currentPitch, -60f, 60f);

        transform.rotation = Quaternion.Euler(currentPitch, transform.eulerAngles.y + yaw, 0);
    }
}