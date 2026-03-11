using UnityEngine;

public class CylinderInteractable : MonoBehaviour, IInteractable
{
    private Renderer r;
    private Color defaultColor;
    private float fixedY;

    private Vector3 scaleAtPinchStart;
    [SerializeField] private float minSize = 0.2f;
    [SerializeField] private float maxSize = 5f;

    private Vector3 dragOffset;
    private float dragDepth;

    private Quaternion rotationAtTwistStart;

    void Start()
    {
        r = GetComponent<Renderer>();
        defaultColor = r.material.color;

        fixedY = transform.position.y;
    }

    public void SelectObject()
    {
        r.material.color = Color.magenta;
    }

    public void UnselectObject()
    {
        r.material.color = defaultColor;
    }

    public void StartDrag(Vector3 hitPoint)
    {
        dragOffset = transform.position - hitPoint;
        dragDepth = Camera.main.WorldToScreenPoint(hitPoint).z;
    }

    public void DragTo(Vector2 screenPos)
    {
        Vector3 screenPoint = new Vector3(screenPos.x, screenPos.y, dragDepth);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPoint);

        Vector3 finalPos = worldPos + dragOffset;
        finalPos.y = fixedY;

        transform.position = finalPos;
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
        transform.rotation = rotationAtTwistStart * Quaternion.AngleAxis(angleDelta, Vector3.up);
    }
}