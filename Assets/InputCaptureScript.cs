using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCaptureScript : MonoBehaviour
{
    Touch t;
    int RobInt = 5;
    float timer = 0;
    float maxTimer = 1.0f;
    bool hasMoved = false;
    ManagerAction theManager;




    // Start is called before the first frame update
    void Start()
    {
        theManager = FindObjectOfType<ManagerAction>();
    }

    // Update is called once per frame
    void Update()
    {
        if (theManager == null) return;
        if (Input.touchCount > 0)
        {
            t = Input.GetTouch(0);
            // print("Hello");

            //print(t.position);

            //print(t.phase);

            print("The touch count is" + Input.touchCount);



            switch (t.phase)
            {
                case TouchPhase.Began:
                    timer = 0f;
                    hasMoved = false;

                    theManager.TapAt(t.position);
                    break;

                case TouchPhase.Stationary:
                    timer += Time.deltaTime;
                    break;

                case TouchPhase.Moved:
                    timer += Time.deltaTime;
                    hasMoved = true;

                    theManager.DragAt(t.position);
                    break;

                case TouchPhase.Ended:
                    if (timer < maxTimer && !hasMoved)
                    {
                        theManager.TapAt(t.position);
                    }
                    break;
            }


        }
    }
}
