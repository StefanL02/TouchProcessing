using UnityEngine;

public class ManagerAction : MonoBehaviour
{
    private IInteractable selectedObject;
    private IInteractable draggedObject;

    private CameraManager cameraManager;

    void Start()
    {
        cameraManager = FindFirstObjectByType<CameraManager>();
    }

    public bool HasSelectedObject => selectedObject != null;
    public bool IsDraggingObject => draggedObject != null;

    // Begin drag only if finger starts on interactable
    public bool TryBeginDrag(Vector2 screenPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            IInteractable hitObj = hit.collider.GetComponent<IInteractable>();
            if (hitObj != null)
            {
                draggedObject = hitObj;
                draggedObject.StartDrag(hit.point);

                if (selectedObject != hitObj)
                {
                    if (selectedObject != null) selectedObject.UnselectObject();
                    selectedObject = hitObj;
                    selectedObject.SelectObject();
                }
                return true;
            }
        }

        draggedObject = null;
        return false;
    }

    public void DragAt(Vector2 screenPos, Vector2 deltaPixels)
    {
        if (draggedObject != null)
        {
            draggedObject.DragTo(screenPos);
        }
        else
        {
            // No object drag -> camera pan
            if (cameraManager != null)
                cameraManager.Pan(deltaPixels);
        }
    }

    public void EndDrag()
    {
        draggedObject = null;
    }

    // Tap selection toggle (call only on Ended when it was a tap)
    public void TapAt(Vector2 screenPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            IInteractable hitObj = hit.collider.GetComponent<IInteractable>();

            if (hitObj != null)
            {
                if (selectedObject == hitObj)
                {
                    selectedObject.UnselectObject();
                    selectedObject = null;
                }
                else
                {
                    if (selectedObject != null) selectedObject.UnselectObject();
                    selectedObject = hitObj;
                    selectedObject.SelectObject();
                }
                return;
            }
        }

        // Tap empty space -> deselect
        if (selectedObject != null)
        {
            selectedObject.UnselectObject();
            selectedObject = null;
        }
    }

    // Pinch scale object, else zoom camera
    public void Pinch(float pinchDeltaPixels)
    {
        if (selectedObject != null)
        {
            // Convert pixels to a gentle ratio
            // ratio ~ 1 + small amount
            float ratio = 1f + (pinchDeltaPixels * 0.002f);
            selectedObject.ScaleTo(ratio);
        }
        else
        {
            if (cameraManager != null)
                cameraManager.Zoom(pinchDeltaPixels);
        }
    }

    // Tell object a new pinch started so it can cache scale
    public void StartPinch()
    {
        if (selectedObject != null) selectedObject.PrepareScale();
    }

    public void ScaleSelected(float ratio)
    {
        if (selectedObject != null)
            selectedObject.ScaleTo(ratio);
    }
}