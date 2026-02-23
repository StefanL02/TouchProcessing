using UnityEngine;

public class CubeScript : MonoBehaviour, IInteractable
{
    private Renderer r;
    private float fixedY;        
    private float landingZoneZ;  
    private bool canMove = true;

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
        
    }

    public void PrepareScale()
    {
        
    }

    public void StartDrag(Vector3 hitPoint)
    {
        
    }
}
