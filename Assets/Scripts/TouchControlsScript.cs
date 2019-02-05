﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControlsScript : MonoBehaviour
{
    public float DragSpeed = 2;
    
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
                    //Get the current touch position
                    Vector3 currentDragPosition = Input.GetTouch(0).position;
                    
                    //Get the direction we are dragging
                    Vector3 deltaDragPosition = MainCam.ScreenToViewportPoint(currentDragPosition - DragStartPosition);
                    
                    //Reverse it to move the camera the opposite way
                    deltaDragPosition = -deltaDragPosition;
                    
                    //Get the direction and multiply by the drag speed, so we can easily control our speed
                    Vector3 dragDirection = new Vector3(deltaDragPosition.x * DragSpeed, 0, deltaDragPosition.y * DragSpeed);
                    
                    //Start moving in the specified direction
                    transform.Translate(dragDirection, Space.World);
                }
                else
                {
                    //Cube dragging mode
                    
                    //Get the current touch position
                    Vector3 currentDragPosition = Input.GetTouch(0).position;
                    
                    //Cast a ray on the touch position
                    Ray ray = MainCam.ScreenPointToRay(currentDragPosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit))
                    {
                        //Check the object we have under the touch position
                        if (hit.collider.CompareTag("Ground"))
                        {
                            //If it's the ground object, move the cube to the x and z position of the touch
                            Vector3 newPos = hit.point;
                            newPos.y = CurrentlySelectedCube.transform.position.y;
                            CurrentlySelectedCube.transform.position = newPos;
                        }
                    }
                }
            }
        }
    }

    private void MoveWithDrag(Vector3 currentDragPosition, Transform transformToMove)
    {
        
    }

    private Vector3 GetTouchPositionInWorldSpace(Touch aTouch)
    {
        //Get the position of the touch in world space
        return MainCam.ScreenToWorldPoint(aTouch.position);
    }

    
    
}
