using UnityEngine;

public class InputCaptureScript : MonoBehaviour
{
    private ManagerAction theManager;
    private CameraManager cameraManager;

    [SerializeField] private float maxTapTime = 0.25f;
    [SerializeField] private float moveDeadzone = 6.0f;

    private float timer;
    private bool hasMoved;
    private bool beganDragging;

    // Two-finger gesture tracking
    private bool isPinching;
    private float prevPinchDistance;
    

    void Start()
    {
        theManager = FindFirstObjectByType<ManagerAction>();
        cameraManager = FindFirstObjectByType<CameraManager>();
    }

    void Update()
    {
        if (theManager == null) return;

        if (Input.touchCount != 2)
            isPinching = false;

        // --- 2 FINGERS: PINCH + TWIST ---
        if (Input.touchCount == 2)
        {
            Touch t1 = Input.GetTouch(0);
            Touch t2 = Input.GetTouch(1);

            // Cancel single-touch drag
            theManager.EndDrag();
            beganDragging = false;

            timer = 0f;
            hasMoved = false;

            float currentDistance = Vector2.Distance(t1.position, t2.position);

            float currentAngle = Mathf.Atan2(
                t1.position.y - t2.position.y,
                t1.position.x - t2.position.x
            ) * Mathf.Rad2Deg;

            if (!isPinching)
            {
                isPinching = true;

                prevPinchDistance = currentDistance;

                // Start object scaling
                theManager.StartPinch();

                // Start camera rotation ONLY if no object selected
                if (cameraManager != null && !theManager.HasSelectedObject)
                    cameraManager.BeginRotate(currentAngle);
            }
            else
            {
                // ----- PINCH -----
                float pinchDelta = currentDistance - prevPinchDistance;
                prevPinchDistance = currentDistance;

                theManager.Pinch(pinchDelta);

                // ----- TWIST -----
                if (cameraManager != null && !theManager.HasSelectedObject)
                    cameraManager.UpdateRotate(currentAngle);
            }

            return;
        }

        // --- 1 FINGER ---
        isPinching = false;

        if (Input.touchCount == 1)
        {
            Touch t = Input.GetTouch(0);

            switch (t.phase)
            {
                case TouchPhase.Began:
                    timer = 0f;
                    hasMoved = false;
                    beganDragging = theManager.TryBeginDrag(t.position);
                    break;

                case TouchPhase.Moved:
                    timer += Time.deltaTime;

                    if (t.deltaPosition.magnitude > moveDeadzone)
                        hasMoved = true;

                    theManager.DragAt(t.position, t.deltaPosition);
                    break;

                case TouchPhase.Stationary:
                    timer += Time.deltaTime;
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    theManager.EndDrag();

                    if (!beganDragging && !hasMoved && timer <= maxTapTime)
                        theManager.TapAt(t.position);

                    timer = 0f;
                    hasMoved = false;
                    beganDragging = false;
                    break;
            }
        }
        else
        {
            // no touches
            timer = 0f;
            hasMoved = false;
            beganDragging = false;
        }
    }
}