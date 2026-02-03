using UnityEngine;

public class ManagerAction : MonoBehaviour
{
    private IInteractable selectedObject;
   

    public void TapAt(Vector2 screenPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        Debug.DrawLine(ray.origin, ray.origin + 100f * ray.direction, Color.red, 1f);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            IInteractable newObj = hit.collider.GetComponent<IInteractable>();

            if (newObj != null)
            {
                if (selectedObject != null)
                    selectedObject.UnselectObject();

                selectedObject = newObj;
                selectedObject.SelectObject();

                Debug.Log("Selected: " + hit.collider.name);
            }
            else
            {
                if (selectedObject != null)
                    selectedObject.UnselectObject();

                selectedObject = null;
                Debug.Log("Hit something, but it's NOT interactable: " + hit.collider.name);
            }
        }
        else
        {
            if (selectedObject != null)
                selectedObject.UnselectObject();

            selectedObject = null;
            Debug.Log("No object hit");
        }
    }

    public void DragAt(Vector2 screenPos)
    {
        if (selectedObject == null) return;

        Debug.Log("Dragging at screen position: " + screenPos);

        selectedObject.DragTo(screenPos);
    }
}
