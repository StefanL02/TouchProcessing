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
                    break;
                    
                case TouchPhase.Stationary:
                    timer += Time.deltaTime;
                    break;

                case TouchPhase.Moved:
                    timer += Time.deltaTime;
                    hasMoved = true;
                    break;

                case TouchPhase.Ended:
                     if (timer < maxTimer && !hasMoved)
                     {
                       theManager.tap(t.position);
                     }

                    /*{
                        print("TAP detected at: " + t.position );
                    }
                    else
                    {
                        print("NOT a tap (long press or moved)");
                    } */
                    break;
            }

           
        }
    }
}
