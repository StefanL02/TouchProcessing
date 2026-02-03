using UnityEngine;

public interface IInteractable
{
    void SelectObject();
    void UnselectObject();
    void DragTo(Vector2 screenPos);
}
