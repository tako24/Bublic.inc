using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour, IWeapon
{
    public string gunName;
    public Transform AttackPosition;
    public int Damage { get; set; }
    public int Durability { get; set; }
    public float TimeBtwnAttack { get; set; }
    public float StartTimeAttack { get; set; }
    public float AttackRadius;

    private string text ;
    public int textSize ;
    public Font textFont;
    public Color textColor = Color.white;
    public float textHeight = 0.8f;
    public Color shadowColor = new Color(0, 0, 0, 0.5f);
    public Vector2 shadowOffset = new Vector2(1, 1);


    private void Awake()
    {
        text = String.Format("<b>Нажмите \"{0}\", что бы взять </b> <color=#ffea00> {1}</color>","E", gunName);
        enabled = true;
        StartTimeAttack = 1f;
        TimeBtwnAttack = 0;
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
    void Update()
    {
        if (TimeBtwnAttack <= 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                DoDamage();
<<<<<<< HEAD
=======
                PickUp();
>>>>>>> parent of 19d4a1b (Inventory step1)
                TimeBtwnAttack = StartTimeAttack;

            }
        }
        else 
        {
            TimeBtwnAttack -= Time.deltaTime;
            print("Идет перезарядка");
        }

    }
    public void PickUp()
    {
<<<<<<< HEAD
        enabled = false;
=======
        GetComponent<ObjectNameView>().enabled = false;
>>>>>>> parent of 19d4a1b (Inventory step1)
    }
    public void DoDamage()
    {
            Collider2D[] enemysCollider = Physics2D.OverlapCircleAll(AttackPosition.position, AttackRadius, LayerMask.GetMask("Enemy"));

            if (enemysCollider.Length == 0)
            {
                print("Никого нет в радиусе аттаки");
                return;
            }
;
            foreach (Collider2D enemyCollider in enemysCollider)
            {
                enemyCollider.GetComponent<HP>().TakeDamage(15);
            }
            TimeBtwnAttack = StartTimeAttack; 
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(AttackPosition.position, AttackRadius);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(AttackPosition.position, AttackRadius);
    }
}
