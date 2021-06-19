using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public InventoryScript inv;
    public Canvas canvas;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (canvas.enabled)
            {
                canvas.enabled = false;
                Time.timeScale = 1;
            }
            else
            {
                canvas.enabled = true;
                Time.timeScale = 0;
                inv.UpdateInv();
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            inv.Use(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            inv.Use(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            inv.Use(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            inv.Use(3);
        }

    }
}
