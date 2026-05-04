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
    [SerializeField] private TMP_Text accelerometerButtonText;

    [Header("Optional Button Images")]
    [SerializeField] private Image cameraRestartImage;
    [SerializeField] private Image movementButtonImage;
    [SerializeField] private Image rotationButtonImage;
    [SerializeField] private Image gyroButtonImage;
    [SerializeField] private Image zoomButtonImage;
    [SerializeField] private Image accelerometerButtonImage;

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

    public void TurnOnOffAccelerometer()
    {
        if (cameraManager == null) return;

        cameraManager.isAccelerometer = !cameraManager.isAccelerometer;
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

        if (accelerometerButtonText != null)
            accelerometerButtonText.text = cameraManager.isAccelerometer ? "Accelerometer ON" : "Accelerometer OFF";

        if (movementButtonImage != null)
            movementButtonImage.color = cameraManager.isMoving ? Color.green : Color.red;

        if (rotationButtonImage != null)
            rotationButtonImage.color = cameraManager.isRotatingAroundItself ? Color.green : Color.red;

        if (gyroButtonImage != null)
            gyroButtonImage.color = cameraManager.isGyro ? Color.green : Color.red;

        if (zoomButtonImage != null)
            zoomButtonImage.color = cameraManager.isZoom ? Color.green : Color.red;

        if (accelerometerButtonImage != null)
            accelerometerButtonImage.color = cameraManager.isAccelerometer ? Color.green : Color.red;
    }

    public void ShareScore()
    {
        int tapCount = TapCounter.Instance.GetTapCount();
        string shareMessage = "I scored " + tapCount + " taps in TouchProcessing! Can you beat me?";

        AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
        AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");

        intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
        intentObject.Call<AndroidJavaObject>("setType", "text/plain");
        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), shareMessage);

        AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");

        AndroidJavaObject chooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, "Share your score!");
        currentActivity.Call("startActivity", chooser);
    }
}