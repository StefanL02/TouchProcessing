using UnityEngine;

public class InputCaptureScript : MonoBehaviour
{
    Touch t;
    float timer = 0f;
    float maxTimer = 1.0f;
    bool hasMoved = false;
    float t_d_start;
    bool isPinching = false;

    ManagerAction theManager;

    void Start()
    {
        theManager = FindObjectOfType<ManagerAction>();
    }

    void Update()
    {
        if (theManager == null) return;

        // --- MULTI-TOUCH (PINCH TO SCALE) ---
        if (Input.touchCount == 2)
        {
            Touch t1 = Input.GetTouch(0);
            Touch t2 = Input.GetTouch(1);

            if (!isPinching)
            {
                // Began: Calculate initial distance and lock starting scale
                t_d_start = Vector2.Distance(t1.position, t2.position);
                theManager.StartPinch();
                isPinching = true;
            }
            else
            {
                // Update: Calculate ratio relative to start
                float t_d_now = Vector2.Distance(t1.position, t2.position);

                if (t_d_start > 0)
                {
                    float newScaleRatio = t_d_now / t_d_start;
                    theManager.ScaleAt(newScaleRatio);
                }
            }

            
            return;
        }

        // --- SINGLE-TOUCH (TAP & DRAG) ---
        if (Input.touchCount == 1)
        {
            isPinching = false; // Reset pinch state
            t = Input.GetTouch(0);

            switch (t.phase)
            {
                case TouchPhase.Began:
                    timer = 0f;
                    hasMoved = false;
                    theManager.TapAt(t.position);
                    break;

                case TouchPhase.Stationary:
                    timer += Time.deltaTime;
                    break;

                case TouchPhase.Moved:
                    timer += Time.deltaTime;
                    hasMoved = true;
                    theManager.DragAt(t.position);
                    break;

                case TouchPhase.Ended:
                    if (timer >= maxTimer) Debug.Log("Long press");
                    timer = 0f;
                    hasMoved = false;
                    break;
            }
        }
        else if (Input.touchCount == 0)
        {
            isPinching = false;
        }
    }
}