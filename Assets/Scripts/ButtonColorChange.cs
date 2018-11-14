using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonColorChange : MonoBehaviour {

    public byte red;
    public byte green;
    public byte blue;
    public byte transparency;
    public Image image;
    public PlayerSettings Player;

    public void ChangeUIPlayerColor()
    {
        Color32 color = new Color32(red, green, blue, transparency);
        image.color = color;
        Player.playerColor = color;
    }    
}
