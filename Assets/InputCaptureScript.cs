using UnityEngine;

public class InputCaptureScript : MonoBehaviour
{
    private Touch t;

    private float timer = 0f;
    [SerializeField] private float maxTapTime = 0.25f;     // tap threshold
    [SerializeField] private float moveDeadzone = 6.0f;    // pixels

    private bool hasMoved = false;
    private bool beganDragging = false;

    // Pinch
    private float t_d_start;
    private bool isPinching = false;

    private ManagerAction theManager;

    void Start()
    {
        theManager = FindObjectOfType<ManagerAction>();
    }

    void Update()
    {
        if (theManager == null) return;

        // --- PINCH TO SCALE ---
        if (Input.touchCount == 2)
        {
            hasMoved = false;
            timer = 0f;
            beganDragging = false;

            Touch t1 = Input.GetTouch(0);
            Touch t2 = Input.GetTouch(1);

            if (!isPinching)
            {
                t_d_start = Vector2.Distance(t1.position, t2.position);
                theManager.StartPinch();
                isPinching = true;
            }
            else
            {
                float t_d_now = Vector2.Distance(t1.position, t2.position);
                if (t_d_start > 0f)
                    theManager.ScaleAt(t_d_now / t_d_start);
            }

            return;
        }

        // --- SINGLE TOUCH ---
        if (Input.touchCount == 1)
        {
            isPinching = false;
            t = Input.GetTouch(0);

            switch (t.phase)
            {
                case TouchPhase.Began:
                    timer = 0f;
                    hasMoved = false;

                    // Start drag IF finger began on an interactable
                    beganDragging = theManager.TryBeginDrag(t.position);
                    break;

                case TouchPhase.Moved:
                    timer += Time.deltaTime;

                    if (t.deltaPosition.magnitude > moveDeadzone)
                        hasMoved = true;

                    if (beganDragging)
                        theManager.DragAt(t.position);

                    break;

                case TouchPhase.Stationary:
                    timer += Time.deltaTime;
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    theManager.EndDrag();

                    // Tap only if it wasn't a drag and it was quick + still
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
            isPinching = false;
            beganDragging = false;
            timer = 0f;
            hasMoved = false;
        }
    }
}