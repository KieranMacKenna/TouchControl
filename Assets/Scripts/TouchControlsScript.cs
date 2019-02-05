using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControlsScript : MonoBehaviour
{
    private Camera MainCam;

    private Cube CurrentlySelectedCube;

    private Vector3 DragStartPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        MainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        HandleCubeSelection();

        HandleDrag();
    }

  

    private void HandleCubeSelection()
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
                        //We are touching a cube
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
                    else
                    {
                        //We are not touching a cube
                        //Deselect old cube 
                        if (CurrentlySelectedCube != null)
                            CurrentlySelectedCube.Deselect();
                        CurrentlySelectedCube = null;
                    }
                }
            }
        }
    }
    
    private void HandleDrag()
    {
        //Check if there is any touch on the screen
        if (Input.touchCount == 1)
        {
            Touch firstTouch = Input.GetTouch(0);

            //Check if the touch just began
            if (firstTouch.phase == TouchPhase.Began)
            {
                //Get the initial position of the touch
                DragStartPosition = Input.GetTouch(0).position;
            }
            
            //Check if the touch is dragged
            if (firstTouch.phase == TouchPhase.Moved)
            {
                if (CurrentlySelectedCube == null)
                {
                    //Camera dragging mode
                    Vector3 CurrentDragPosition = Input.GetTouch(0).position;
                    Vector3 deltaDragPosition = CurrentDragPosition - DragStartPosition;
                    deltaDragPosition.z = -deltaDragPosition.z;
                    Vector3 newPos = transform.position + deltaDragPosition;
                    newPos.y = transform.position.y;
                    transform.position = newPos;
                    DragStartPosition = Input.GetTouch(0).position;
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
