using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public float MaxScale = 3;
    public float MinScale = 0.3f;
    
    private Color DefaultColor;

    private MeshRenderer meshRend;
    
    // Start is called before the first frame update
    void Start()
    {
        meshRend = GetComponent<MeshRenderer>();
        DefaultColor = meshRend.material.color;
    }

    internal void Accellerate(Vector3 acc)
    {
        GetComponent<Rigidbody>().AddForce((acc));
        
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

    

    /// Increases the scale of the object by certain amount
    public void ScaleOut(float amount)
    {
        //Get the current scale
        Vector3 newScale = transform.localScale;
        
        //Add the scaling amount and clamp it between the minimun and maximum value
        newScale.x = Mathf.Clamp(newScale.x + amount, MinScale, MaxScale);
        newScale.y = Mathf.Clamp(newScale.y + amount, MinScale, MaxScale);
        newScale.z = Mathf.Clamp(newScale.z + amount, MinScale, MaxScale);
        
        //Set the new scale
        transform.localScale = newScale;
    }

 
    /// Decreases the scale of the object by certain amount
    public void ScaleIn(float amount)
    {
        //Get the current scale
        Vector3 newScale = transform.localScale;
        
        //Add the scaling amount and clamp it between the minimun and maximum value
        newScale.x = Mathf.Clamp(newScale.x - amount, MinScale, MaxScale);
        newScale.y = Mathf.Clamp(newScale.y - amount, MinScale, MaxScale);
        newScale.z = Mathf.Clamp(newScale.z - amount, MinScale, MaxScale);
        
        //Set the new scale
        transform.localScale = newScale;
    }
}
