using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JagerStats : MonoBehaviour
{
    public int HP;
    public float range;
    public float speed;
    public float rotateSpeed;
    public float atackTime;
    public float atackKD;
    public int damage;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<ContactDamage>().Damage = damage;
        gameObject.GetComponentInChildren<ContactDamage>().Damage = damage;
        gameObject.GetComponent<AIPath>().maxSpeed = speed;
        gameObject.GetComponent<HP>()._maxHP = HP;
        gameObject.GetComponent<HP>()._currentHP = HP;
        gameObject.GetComponentInChildren<CircleCollider2D>().radius = 1.3f + range;
        gameObject.transform.GetChild(0).position += new Vector3(0, range, 0);
    }
}
