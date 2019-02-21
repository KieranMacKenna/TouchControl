using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.XR;

public class TouchControlsScript : MonoBehaviour
{
    //Public variables, editable in the inspector
    public float DragSpeed = 2;
    public float CubeDragSpeed = 1f;
    public float PinchSpeed = 4;
    public float PinchDistanceTreshold = 0.3f;
    public float RotationAngleTreshold = 20;
    public int MaxCameraZoom = 20;
    public int MinCameraZoom = 100;
    public float RotationSpeed = 0.5f;
    public float deselectionTime = 0.8f;
    public float AccelerometerRotationSpeed = 1;
    
    private Camera MainCam;

    private Cube CurrentlySelectedCube;

    private Vector3 DragStartPosition;

    private float PinchStartingDistance;

    private Vector3 RotationStartingVector;

    private bool deselectionQueued;

    private float initialXAcc;

    // Start is called before the first frame update
    void Start()
    {
        MainCam = Camera.main;
        initialXAcc = Input.acceleration.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        
        HandleCubeSelection();

        HandleDrag();

        HandlePinchAndRotation();

        HandleThreeFingerClick();
        
        HandleAccelerometer();
    }

    private void HandleAccelerometer()
    {
        if(CurrentlySelectedCube)
        CurrentlySelectedCube.Accellerate(Input.acceleration);
      //  transform.Translate(Input.acceleration.x * 0.00f, Input.acceleration.y * 0.00f, -Input.acceleration.z *0.01f);
        
        
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
                            StartCoroutine(WaitAndDeselect(deselectionTime));
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
            if (firstTouch.phase == TouchPhase.Moved && !deselectionQueued)
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
                    Vector3 dragDirection =
                        new Vector3(deltaDragPosition.x * DragSpeed, 0, deltaDragPosition.y * DragSpeed);

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

                            CurrentlySelectedCube.transform.position =
                                Vector3.Lerp(CurrentlySelectedCube.transform.position, newPos, CubeDragSpeed / 10);
                        }
                    }
                }
            }
        }
    }

    //Reference https://www.youtube.com/watch?v=S3pjBQObC90
    private void HandlePinchAndRotation()
    {
        //Check if there are more than two touches currently
        if (Input.touchCount == 2)
        {
            //Get both touches
            Touch firstTouch = Input.GetTouch(0);
            Touch secondTouch = Input.GetTouch(1);

            //Check if the touch just began for the first touch or the second touch
            if (firstTouch.phase == TouchPhase.Began || secondTouch.phase == TouchPhase.Began)
            {
                StopAllCoroutines();
                deselectionQueued = false;
                //Get the initial distance between the two touches
                PinchStartingDistance = Vector3.Distance(firstTouch.position, secondTouch.position);

                RotationStartingVector = firstTouch.position - secondTouch.position;
                //RotationStartingAngle = Vector3.Angle(rotationVector, Vector3.up);
            }
            else if (firstTouch.phase == TouchPhase.Moved || firstTouch.phase == TouchPhase.Moved)
            {
                //When one of them has moved get the current distance between the two touches
                float CurrentPinchDistance = Vector3.Distance(firstTouch.position, secondTouch.position);

                //Get the difference between current distance and the initial distance
                float deltaPinchDistance = CurrentPinchDistance - PinchStartingDistance;

                //Get the angle between the starting touches and current touches
                Vector3 rotationVector = firstTouch.position - secondTouch.position;
                int CurrentRotationAngle = (int) Vector3.Angle(rotationVector, RotationStartingVector);
                Vector3 LR = Vector3.Cross(RotationStartingVector, rotationVector);

                if (CurrentRotationAngle > RotationAngleTreshold)
                {
                    //We have rotation. Check if we are in camera mode or cube selected mode
                    if (CurrentlySelectedCube == null)
                    {
                        //No cube selected, rotate the camera
                        //Get rotation direction
                        if (LR.z > 0)
                        {
                            //Anticlockwise
                            transform.Rotate(0f, (RotationSpeed) * CurrentRotationAngle, 0f, Space.World);
                        }
                        else if (LR.z < 0)
                        {
                            //Clockwise
                            transform.Rotate(0f, (RotationSpeed) * -CurrentRotationAngle, 0f, Space.World);
                        }
                    }
                    else
                    {
                        //Cube is selected, rotate the cube
                        //Get rotation direction
                        if (LR.z < 0)
                        {
                            CurrentlySelectedCube.transform.Rotate(0f, (RotationSpeed) * CurrentRotationAngle, 0f, Space.World);
                        }
                        else if (LR.z >  0)
                        {
                            CurrentlySelectedCube.transform.Rotate(0f, (RotationSpeed) * -CurrentRotationAngle, 0f, Space.World);
                        }
                    }
                }
                else if (Math.Abs(deltaPinchDistance) >= PinchDistanceTreshold)
                {
                    //The distance is more than the treshold, we are pinching, check the direction (out or in)
                    if (deltaPinchDistance < 0)
                    {
                        //We are pinching in -> Zoom out or scale the object
                        if (CurrentlySelectedCube == null)
                        {
                            //No cube selected, zoom in the camera
                            float newZoom = Mathf.Clamp(MainCam.fieldOfView + PinchSpeed / 10, MaxCameraZoom,
                                MinCameraZoom);
                            MainCam.fieldOfView = newZoom;
                        }
                        else
                        {
                            //Cube is selected, scale the cube accordingly
                            CurrentlySelectedCube.ScaleIn(PinchSpeed / 100);
                        }
                    }
                    else if (deltaPinchDistance > 0)
                    {
                        //We are pinching out -> zoom in or scale the selected object
                        if (CurrentlySelectedCube == null)
                        {
                            //No cube selected, zoom in the camera
                            float newZoom = Mathf.Clamp(MainCam.fieldOfView - PinchSpeed / 10, MaxCameraZoom,
                                MinCameraZoom);
                            MainCam.fieldOfView = newZoom;
                        }
                        else
                        {
                            //Cube is selected, scale the cube accordingly
                            CurrentlySelectedCube.ScaleOut(PinchSpeed / 100);
                        }
                    }
                }
            }
        }
    }
    
    private void HandleThreeFingerClick()
    {
        //Check if we have three touches currentl
        if (Input.touchCount == 3)
        {
            //Check if we have a selected cube
            if (CurrentlySelectedCube != null)
            {
                //Check if any of the touches just began
                if (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(1).phase == TouchPhase.Began ||
                    Input.GetTouch(2).phase == TouchPhase.Began)
                {
                    StartCoroutine(RotateCube360());
                }
            }
        }
    }


    private Vector3 GetTouchPositionInWorldSpace(Touch aTouch)
    {
        //Get the position of the touch in world space
        return MainCam.ScreenToWorldPoint(aTouch.position);
    }


    private IEnumerator WaitAndDeselect(float time)
    {
        deselectionQueued = true;
        yield return new WaitForSeconds(time);

        if (CurrentlySelectedCube != null)
            CurrentlySelectedCube.Deselect();
            CurrentlySelectedCube = null;
            deselectionQueued = false;
    }

    private IEnumerator RotateCube360()
    {
        for (int n = 0; n < 37; n++)
        {
            CurrentlySelectedCube.transform.Rotate(0, 10, 0);
            yield return null;
        }
    }
}