using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerAction : MonoBehaviour
{
    IInteractable selectedObject;
    Transform selectedTransform;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


    }
    public void TapAt(Vector2 tapPosition)
    {
        Ray ourRay = Camera.main.ScreenPointToRay(tapPosition);
        Debug.DrawLine(ourRay.origin, ourRay.origin + 100 * ourRay.direction, Color.red, 1f );

        

        if (Physics.Raycast(ourRay, out RaycastHit hit))
        {
            selectedTransform = hit.collider.transform;
            selectedObject = hit.collider.GetComponent<IInteractable>();

            Debug.Log("Selected: " + hit.collider.name);
        }
        else
        {
            selectedTransform = null;
            Debug.Log("No object hit");
        }
    }

    public void DragAt(Vector2 dragPosition)
    {
        if (selectedTransform == null) return;

        Ray ray = Camera.main.ScreenPointToRay(dragPosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 target = hit.point;
            target.y = selectedTransform.position.y; 
            selectedTransform.position = target;
        }
    }

}