using UnityEngine;

public class CylinderInteractable : MonoBehaviour, IInteractable
{
    private Renderer r;
    private Color defaultColor;

    private Plane dragPlane;
    private Vector3 dragOffset;

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
        if (r != null) r.material.color = Color.magenta;
    }

    public void UnselectObject()
    {
        if (r != null) r.material.color = defaultColor;
    }

    // WALL DRAG: drag on a camera-facing vertical plane
    public void StartDrag(Vector3 hitPoint)
    {
        // Plane facing camera, passing through hit point
        dragPlane = new Plane(Camera.main.transform.forward, hitPoint);
        dragOffset = transform.position - hitPoint;
    }

    public void DragTo(Vector2 screenPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);

        if (dragPlane.Raycast(ray, out float enter))
        {
            Vector3 worldPos = ray.GetPoint(enter);
            transform.position = worldPos + dragOffset;
        }
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