using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControlsScript : MonoBehaviour
{
    private Camera MainCam;

    private Cube CurrentlySelectedCube;
    
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
            
            //Check if the touch just began
            if (firstTouch.phase == TouchPhase.Began)
            {
                //Get the position of the first touch in world space
                Vector3 firstTouchPos = firstTouch.position;

                //Cast a ray on the touch position
                Ray ray = MainCam.ScreenPointToRay(firstTouchPos);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    //Get the cube
                    GameObject CubeObj = hit.collider.gameObject;
                    if (CubeObj.CompareTag("Cube"))
                    {
                        Debug.Log("Selected cube");
                        
                        //Get the cube script
                        Cube cubeScript = CubeObj.GetComponent<Cube>();
            
                        //Check if we have a cube script attached to the object
                        if (cubeScript == null)
                        {
                            Debug.LogWarning("Cube script not found on Cube object");
                        }
                        else
                        {
                            //Deselect old cube 
                            if (CurrentlySelectedCube != null)
                                CurrentlySelectedCube.Deselect();
                            
                            //Select new cube
                            cubeScript.Select();
                            CurrentlySelectedCube = cubeScript;
                        }
                    }
                }
            }
        }
    }

    private Vector3 GetTouchPositionInWorldSpace(Touch aTouch)
    {
        //Get the position of the touch in world space
        return MainCam.ScreenToWorldPoint(aTouch.position);
    }

    
    
}
