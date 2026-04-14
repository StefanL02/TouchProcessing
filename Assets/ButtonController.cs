using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CameraManager cameraManager;

    [Header("Optional Button Labels")]
    [SerializeField] private TMP_Text movementButtonText;
    [SerializeField] private TMP_Text rotationButtonText;
    [SerializeField] private TMP_Text gyroButtonText;
    [SerializeField] private TMP_Text zoomButtonText;

    [Header("Optional Button Images")]
    [SerializeField] private Image cameraRestartImage;
    [SerializeField] private Image movementButtonImage;
    [SerializeField] private Image rotationButtonImage;
    [SerializeField] private Image gyroButtonImage;
    [SerializeField] private Image zoomButtonImage;

private void Start()
    {
        UpdateButtonUI();
    }

    public void ResetCamera()
    {
        if (cameraManager == null) return;

        cameraManager.ResetCamera();
    }

    public void TurnOnOffCameraMovement()
    {
        if (cameraManager == null) return;

        cameraManager.isMoving = !cameraManager.isMoving;
        UpdateButtonUI();
    }

    public void TurnOnOffCameraRotationItself()
    {
        if (cameraManager == null) return;

        cameraManager.isRotatingAroundItself = !cameraManager.isRotatingAroundItself;
        UpdateButtonUI();
    }

    public void TurnOnOffGyro()
    {
        if (cameraManager == null) return;

        cameraManager.isGyro = !cameraManager.isGyro;
        Debug.Log("Gyro toggled: " + cameraManager.isGyro);
        UpdateButtonUI();
    }

    public void TurnOnOffCameraZoom()
    {
        if (cameraManager == null) return;

        cameraManager.isZoom = !cameraManager.isZoom;
        UpdateButtonUI();
    }

    private void UpdateButtonUI()
    {
        if (cameraManager == null) return;

        if (movementButtonText != null)
            movementButtonText.text = cameraManager.isMoving ? "Camera Movement ON" : "Camera Movement OFF";

        if (rotationButtonText != null)
            rotationButtonText.text = cameraManager.isRotatingAroundItself ? "Camera Rotation ON" : "Camera Rotation OFF";

        if (gyroButtonText != null)
            gyroButtonText.text = cameraManager.isGyro ? "Gyro ON" : "Gyro OFF";

        if (zoomButtonText != null)
            zoomButtonText.text = cameraManager.isZoom ? "Zoom ON" : "Zoom OFF";

        if (movementButtonImage != null)
            movementButtonImage.color = cameraManager.isMoving ? Color.green : Color.red;

        if (rotationButtonImage != null)
            rotationButtonImage.color = cameraManager.isRotatingAroundItself ? Color.green : Color.red;

        if (gyroButtonImage != null)
            gyroButtonImage.color = cameraManager.isGyro ? Color.green : Color.red;

        if (zoomButtonImage != null)
            zoomButtonImage.color = cameraManager.isZoom ? Color.green : Color.red;
    }
}