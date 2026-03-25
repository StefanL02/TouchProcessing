using UnityEngine;

public class CubeScript : MonoBehaviour, IInteractable
{
    private Renderer r;
    private Color defaultColor;

    private float fixedY;
    private Plane groundPlane;
    private Vector3 dragOffset;

    private Vector3 scaleAtPinchStart;
    private Quaternion rotationAtTwistStart;

    [SerializeField] private float minSize = 0.2f;
    [SerializeField] private float maxSize = 5f;

    void Start()
    {
        r = GetComponent<Renderer>();
        if (r != null) defaultColor = r.material.color;

        fixedY = transform.position.y;
    }

    public void SelectObject()
    {
        if (r != null) r.material.color = Color.yellow;
    }

    public void UnselectObject()
    {
        if (r != null) r.material.color = defaultColor;
    }

    // FLOOR DRAG: drag on horizontal plane under object
    public void StartDrag(Vector3 hitPoint)
    {
        groundPlane = new Plane(Vector3.up, new Vector3(0f, fixedY, 0f));
        dragOffset = transform.position - hitPoint;
    }

    public void DragTo(Vector2 screenPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);

        if (groundPlane.Raycast(ray, out float enter))
        {
            Vector3 worldPos = ray.GetPoint(enter);
            Vector3 finalPos = worldPos + dragOffset;
            finalPos.y = fixedY;
            transform.position = finalPos;
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