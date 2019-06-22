using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour {

    public SpriteRenderer child;

    public void changeColor(Color color)
    {
        child.color = color;
    }
}
