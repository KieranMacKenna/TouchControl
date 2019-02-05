using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControlsScript : MonoBehaviour
{
    private Camera MainCam;
    
    // Start is called before the first frame update
    void Start()
    {
        MainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //Check if there is any touch on the screen
        if (Input.touchCount > 0)
        {
            Touch firstTouch = Input.GetTouch(0);
            //Get the position of the touch in world space
            Vector3 touchPosWS = MainCam.ScreenToWorldPoint(firstTouch.position);
            Ray ray = MainCam.ScreenPointToRay(touchPosWS);

            
        }
    }
}
