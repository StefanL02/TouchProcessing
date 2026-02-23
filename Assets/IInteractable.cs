using UnityEngine;

public interface IInteractable
{
    void SelectObject();
    void UnselectObject();

    void StartDrag(Vector3 hitPoint);
    void DragTo(Vector2 screenPos);

    void PrepareScale();
    void ScaleTo(float scaleRatio);
}