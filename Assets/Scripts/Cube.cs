using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    private Color DefaultColor;

    private MeshRenderer meshRend;
    
    // Start is called before the first frame update
    void Start()
    {
        meshRend = GetComponent<MeshRenderer>();
        DefaultColor = meshRend.material.color;
    }


    public void Select()
    {
        //Set cube's color to a random color
        meshRend.material.color = GetRandomColor();
    }
    
    private static Color GetRandomColor()
    {
        float red = 0.3f;
        float green = Random.Range(0f, 1f);
        float blue = Random.Range(0f, 1f);

        return new Color(red, green, blue);
    }

    public void Deselect()
    {
        //Return the color to the default one
        meshRend.material.color = DefaultColor;
    }
}
