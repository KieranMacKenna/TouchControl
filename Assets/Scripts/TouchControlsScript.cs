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
            //Get the position of the first touch in world space
            Vector3 firstTouchPos = Input.GetTouch(0).position;
                        
            //Cast a ray on the touch position
            Ray ray = MainCam.ScreenPointToRay(firstTouchPos);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                //Get the cube
                GameObject Cube = hit.collider.gameObject;
                if (Cube.CompareTag("Cube"))
                {
                    Debug.Log("Selected cube");
                    //Set cube's color to a random color
                    Cube.GetComponent<MeshRenderer>().material.color = GetRandomColor();
                }                
            }
        }
    }

    private Vector3 GetTouchPositionInWorldSpace(Touch aTouch)
    {
        //Get the position of the touch in world space
        return MainCam.ScreenToWorldPoint(aTouch.position);
    }

    private Color GetRandomColor()
    {
        float red = 0.3f;
        float green = Random.Range(0f, 1f);
        float blue = Random.Range(0f, 1f);

        return new Color(red, green, blue);
    }
    
}
