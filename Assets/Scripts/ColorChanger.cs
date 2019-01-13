using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorChanger
{
    Image image;
    public float r, g, b;
    public ColorChanger(Image img)
    {
        image = img;
        image.color=new Color(Random.value,Random.value,Random.value);
        
    }
    
    public void SetRGB(float red,float green, float blue)
    {
        image.color = new Color(red, green, blue);
    }
   public void UpdateRGB()
    {
        image.color = new Color(r, g, b);
    }
    public string ToHexadecimal()
    {
        return "#"+ ColorUtility.ToHtmlStringRGB(image.color).ToLower();
        
    }
}

