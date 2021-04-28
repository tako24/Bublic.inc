using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public List<GameObject> Weapons;
    private BoxCollider2D Collider;
    public GameObject Canvas;
    private bool IsClose = true;
    public void DropWeapon()
    {
        GameObject weapon = Instantiate(Weapons[Random.Range(0, Weapons.Count)], transform.position, Quaternion.identity) ;
        weapon.transform.position = new Vector3(transform.position.x, transform.position.y - 0.9f, transform.position.z);

    }


    private void OnTriggerStay2D(Collider2D collision)
    {
      
        if (Input.GetKey(KeyCode.E) && collision.tag =="Player" && IsClose)
        {
            IsClose = false;
            DropWeapon();
            Canvas.SetActive(false);
        }
    }
    private void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsClose)
            Canvas.SetActive(true);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Canvas.SetActive(false);
    }

}
