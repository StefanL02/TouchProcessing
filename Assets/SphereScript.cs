using UnityEngine;

public class SphereScript : MonoBehaviour, IInteractable
{
    private Renderer r;
    private Color defaultColor;

    private Vector3 dragOffset;
    private float dragDepth;

    private Vector3 scaleAtPinchStart;
    private Quaternion rotationAtTwistStart;

    [SerializeField] private float minSize = 0.2f;
    [SerializeField] private float maxSize = 5f;

    void Start()
    {
        r = GetComponent<Renderer>();
        if (r != null) defaultColor = r.material.color;
    }

    public void SelectObject()
    {
        if (r != null) r.material.color = Color.cyan;
    }

    public void UnselectObject()
    {
        if (r != null) r.material.color = defaultColor;
    }

    // FREE DRAG: keep object at same screen depth
    public void StartDrag(Vector3 hitPoint)
    {
        dragOffset = transform.position - hitPoint;
        dragDepth = Camera.main.WorldToScreenPoint(hitPoint).z;
    }

    public void DragTo(Vector2 screenPos)
    {
        Vector3 screenPoint = new Vector3(screenPos.x, screenPos.y, dragDepth);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPoint);

        transform.position = worldPos + dragOffset;
    }

    public void PrepareScale()
    {
        scaleAtPinchStart = transform.localScale;
    }

    public void ScaleTo(float scaleRatio)
    {
        Vector3 newScale = scaleAtPinchStart * scaleRatio;
        float clamped = Mathf.Clamp(newScale.x, minSize, maxSize);
        transform.localScale = new Vector3(clamped, clamped, clamped);
    }

    public void PrepareRotate()
    {
        rotationAtTwistStart = transform.rotation;
    }

    public void RotateTo(float angleDelta)
    {
        transform.rotation =
            rotationAtTwistStart *
            Quaternion.AngleAxis(angleDelta, Vector3.forward);
    }
}