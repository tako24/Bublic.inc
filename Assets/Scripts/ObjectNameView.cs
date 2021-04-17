using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectNameView : MonoBehaviour
{
    public string objectName;
    private string text;
    public int textSize;
    public Font textFont;
    public Color textColor = Color.white;
    public float textHeight = 0.8f;
    public Color shadowColor = new Color(0, 0, 0, 0.5f);
    public Vector2 shadowOffset = new Vector2(1, 1);
    private void Awake()
    {
        text = String.Format("<b>������� \"{0}\", ��� �� ����� </b> <color=#ffea00> {1}</color>", "E", objectName);
        textSize = 8;
    }
    void OnGUI()
    {
        GUI.depth = 9999;

        GUIStyle style = new GUIStyle();
        style.fontSize = textSize;
        style.richText = true;
        if (textFont) style.font = textFont;
        style.normal.textColor = textColor;
        style.alignment = TextAnchor.MiddleCenter;


        Vector3 worldPosition = new Vector3(transform.position.x, transform.position.y + textHeight, transform.position.z);
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
        screenPosition.y = Screen.height - screenPosition.y;

        GUI.Label(new Rect(screenPosition.x, screenPosition.y, 0, 0), text, style);
    }
    void PickUp()
    {
        enabled = false;
    }


}
