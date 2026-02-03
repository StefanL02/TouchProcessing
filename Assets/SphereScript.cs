using UnityEngine;

public class SphereScript : MonoBehaviour, IInteractable
{
    private Renderer r;
    private Color defaultColor;
    private float fixedY;

    void Start()
    {
        r = GetComponent<Renderer>();
        defaultColor = r.material.color;

        fixedY = transform.position.y;
    }

    public void SelectObject()
    {
        r.material.color = Color.cyan;
    }

    public void UnselectObject()
    {
        r.material.color = defaultColor;
    }

    public void DragTo(Vector2 screenPos)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(
            new Vector3(
                screenPos.x,
                screenPos.y,
                Camera.main.WorldToScreenPoint(transform.position).z
            )
        );

        worldPos.y = fixedY;

        transform.position = worldPos;
    }
}
