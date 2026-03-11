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
                    if (selectedObject != null)
                        selectedObject.UnselectObject();

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
            if (cameraManager != null)
                cameraManager.Pan(deltaPixels);
        }
    }

    public void EndDrag()
    {
        draggedObject = null;
    }

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
                    if (selectedObject != null)
                        selectedObject.UnselectObject();

                    selectedObject = hitObj;
                    selectedObject.SelectObject();
                }

                return;
            }
        }

        if (selectedObject != null)
        {
            selectedObject.UnselectObject();
            selectedObject = null;
        }
    }

    public void StartPinch()
    {
        if (selectedObject != null)
            selectedObject.PrepareScale();
    }

    public void ScaleSelected(float ratio)
    {
        if (selectedObject != null)
            selectedObject.ScaleTo(ratio);
    }

    public void Pinch(float pinchDeltaPixels)
    {
        if (selectedObject == null && cameraManager != null)
            cameraManager.Zoom(pinchDeltaPixels);
    }

    public void StartTwist()
    {
        if (selectedObject != null)
            selectedObject.PrepareRotate();
    }

    public void RotateSelected(float angleDelta)
    {
        if (selectedObject != null)
            selectedObject.RotateTo(angleDelta);
    }
}