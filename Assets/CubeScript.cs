using UnityEngine;

public class CubeScript : MonoBehaviour, IInteractable
{
    private Renderer r;
    private float fixedY;        
    private float landingZoneZ;  
    private bool canMove = true;
    private Vector3 scaleAtPinchStart;
    [SerializeField] private float minSize = 0.2f;
    [SerializeField] private float maxSize = 5f;

    private Vector3 dragOffset;
    private float dragDepth;

    void Start()
    {
        r = GetComponent<Renderer>();
        fixedY = transform.position.y; 
    }

    
    public void SetLandingZoneZ(float z)
    {
        landingZoneZ = z;
    }

    public void SelectObject()
    {
        if (canMove)
        {
            r.material.color = Color.yellow;
        }
    }

    public void UnselectObject()
    {
        if (canMove)
        {
            r.material.color = Color.white;
        }
    }

    public void DragTo(Vector2 screenPos)
    {
        if (!canMove) return;

        
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(
            new Vector3(
                screenPos.x,
                screenPos.y,
                Camera.main.WorldToScreenPoint(transform.position).z
            )
        );

       
        transform.position = new Vector3(
            worldPos.x,      
            fixedY,         
            landingZoneZ    
        );
    }

    public void ScaleTo(float scaleRatio)
    {
        Vector3 newScale = scaleAtPinchStart * scaleRatio;
        float clamped = Mathf.Clamp(newScale.x, minSize, maxSize);
        transform.localScale = new Vector3(clamped, clamped, clamped);
    }

    public void PrepareScale()
    {
        scaleAtPinchStart = transform.localScale;
    }

    public void StartDrag(Vector3 hitPoint)
    {
        dragOffset = transform.position - hitPoint;
        dragDepth = Camera.main.WorldToScreenPoint(hitPoint).z;
    }
}
