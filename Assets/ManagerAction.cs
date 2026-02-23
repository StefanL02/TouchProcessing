using UnityEngine;

public class ManagerAction : MonoBehaviour
{
    private IInteractable selectedObject;
    private IInteractable draggedObject;

    // Called on TouchPhase.Began
    public bool TryBeginDrag(Vector2 screenPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            IInteractable hitObj = hit.collider.GetComponent<IInteractable>();
            if (hitObj != null)
            {
                // Start dragging THIS object
                draggedObject = hitObj;
                draggedObject.StartDrag(hit.point);

                // Also select it (optional, but usually desired)
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

    public void DragAt(Vector2 screenPos)
    {
        if (draggedObject == null) return;
        draggedObject.DragTo(screenPos);
    }

    public void EndDrag()
    {
        draggedObject = null;
    }

    // Called ONLY on TouchPhase.Ended when it was a real tap (not a drag)
    public void TapAt(Vector2 screenPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            IInteractable hitObj = hit.collider.GetComponent<IInteractable>();

            if (hitObj != null)
            {
                // Toggle selection
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

        // Tap empty space (or floor) -> deselect (ONLY happens on Ended now)
        if (selectedObject != null)
        {
            selectedObject.UnselectObject();
            selectedObject = null;
        }
    }

    public void StartPinch()
    {
        if (selectedObject != null) selectedObject.PrepareScale();
    }

    public void ScaleAt(float ratio)
    {
        if (selectedObject != null) selectedObject.ScaleTo(ratio);
    }
}