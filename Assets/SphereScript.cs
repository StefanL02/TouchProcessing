using UnityEngine;

public class SphereScript : MonoBehaviour, IInteractable
{
    private Renderer r;
    private Color defaultColor;

    [Header("Lock to Floor (optional)")]
    [SerializeField] private bool lockYToStart = true;
    private float fixedY;

    // Dragging
    private Vector3 dragOffset;
    private float dragDepth;

    // Scaling
    private Vector3 scaleAtPinchStart;

    void Start()
    {
        r = GetComponent<Renderer>();
        if (r != null) defaultColor = r.material.color;

        fixedY = transform.position.y;
    }

    public void SelectObject()
    {
        if (r != null) r.material.color = Color.cyan;
    }

    public void UnselectObject()
    {
        if (r != null) r.material.color = defaultColor;
    }

    public void StartDrag(Vector3 hitPoint)
    {
        // Preserve where you grabbed (prevents snapping)
        dragOffset = transform.position - hitPoint;

        // IMPORTANT: use hitPoint depth, not center depth
        dragDepth = Camera.main.WorldToScreenPoint(hitPoint).z;
    }

    public void DragTo(Vector2 screenPos)
    {
        Vector3 screenPoint = new Vector3(screenPos.x, screenPos.y, dragDepth);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPoint);

        Vector3 finalPos = worldPos + dragOffset;

        if (lockYToStart)
            finalPos.y = fixedY;

        transform.position = finalPos;
    }

    public void PrepareScale()
    {
        scaleAtPinchStart = transform.localScale;
    }

    public void ScaleTo(float scaleRatio)
    {
        transform.localScale = scaleAtPinchStart * scaleRatio;
    }
}