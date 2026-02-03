using UnityEngine;

public class InputCaptureScript : MonoBehaviour
{
    Touch t;
    float timer = 0f;
    float maxTimer = 1.0f;
    bool hasMoved = false;

    ManagerAction theManager;

    void Start()
    {
        theManager = FindObjectOfType<ManagerAction>();
    }

    void Update()
    {
        if (theManager == null) return;
        if (Input.touchCount == 0) return;

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
                // Optional: use this only if you want long-press detection later
                if (timer >= maxTimer) Debug.Log("Long press");

                timer = 0f;
                hasMoved = false;
                break;
        }
    }
}
