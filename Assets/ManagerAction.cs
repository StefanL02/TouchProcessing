using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerAction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


    }
    public void tap(Vector2 tapPosition)
    {
        Ray ourRay = Camera.main.ScreenPointToRay(tapPosition);
        Debug.DrawLine(ourRay.origin, ourRay.origin + 100 * ourRay.direction, Color.red, 1f );
    }
}