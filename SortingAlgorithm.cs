using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SortingAlgorithm
{

    public static Material lineMaterial(Color color)
    {
        Material mat = new Material(Shader.Find("Sprites/Default"));
        mat.color = color;
        return mat;
    }

    public static Vector3 ScreenWorldSize()
    {
        Vector3 screenWorldSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, -10));
        return screenWorldSize;
    }


}
