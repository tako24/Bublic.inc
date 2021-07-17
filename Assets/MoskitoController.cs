using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoskitoStages
{
    One,
    Two,
    Three,
    Four,
    Five
}
public class MoskitoController : MonoBehaviour
{
    public int HP;
    public float flyRange;
    public float flyingSpeed;
    public float atackRange;
    public float fastAtackSpeed;
    public int damage;
    public float KD;
    public float MoveToPositionSpeed;
    public float AtackStageTime;
    private MoskitoStages Stages = MoskitoStages.One;
    private AIPath AIPath;
    private AIDestinationSetter AIDestinationSetter;
    private float moveToTargetTimer = 0;
    private GameObject Player;
    private float atackTimer;
    private Vector3 Vel;
    private float KDTimer;
    private bool CanAtack=true;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponentInChildren<HP>()._maxHP = HP;
        gameObject.GetComponentInChildren<HP>()._currentHP = HP;
        gameObject.GetComponentInChildren<ContactDamage>().Damage = damage;
        gameObject.transform.GetChild(0).localPosition = new Vector3(0, flyRange, 0);
        AIPath = gameObject.GetComponentInChildren<AIPath>();
        AIDestinationSetter = gameObject.GetComponentInChildren<AIDestinationSetter>();
        Player = GameObject.Find("Player");
        gameObject.GetComponent<CircleCollider2D>().radius = atackRange;
    }

    // Update is called once per frame
    void Update()
    {
        if (Stages == MoskitoStages.One) {
            gameObject.transform.Rotate(0, 0, -flyingSpeed * Time.deltaTime);
            gameObject.transform.GetChild(0).Rotate(0, 0, flyingSpeed * Time.deltaTime);
            if (!CanAtack)
            {
                if (KDTimer <= 0)
                {
                    CanAtack = true;
                    KDTimer = KD;
                }
                else KDTimer -= Time.deltaTime;
            }
        }
        if (Stages == MoskitoStages.Two)
        {
            Stages = MoskitoStages.Three;
            gameObject.transform.GetChild(0).localPosition = new Vector3(0, 0, 0);
            Vel = (Player.transform.position - gameObject.transform.position).normalized * fastAtackSpeed;
            atackTimer = AtackStageTime;
        }
        if(Stages== MoskitoStages.Three)
        {
            moveToTargetTimer += Time.deltaTime;
            gameObject.transform.position += Vel * Time.deltaTime;
        }
        if (Stages == MoskitoStages.Four || Stages == MoskitoStages.Three)
        {
            if (atackTimer <= 0)
            {
                Stages = MoskitoStages.Five;
                moveToTargetTimer = 0;
                atackTimer = 0;
            }
            else atackTimer -= Time.deltaTime;
        }
        if (Stages == MoskitoStages.Four)
        {
            gameObject.transform.position += Vel * Time.deltaTime;
            if (moveToTargetTimer <= 0)
            {
                moveToTargetTimer = 0;
                Stages = MoskitoStages.Five;
            }
            else moveToTargetTimer -= Time.deltaTime;
        }
        if (Stages == MoskitoStages.Five)
        {
            gameObject.transform.GetChild(0).localPosition = new Vector3(0, flyRange, 0);
            Stages = MoskitoStages.One;
            KDTimer = KD;
        }
    }
    void StartAtack()
    {
        if (Stages == MoskitoStages.One&&CanAtack)
        {
            Stages = MoskitoStages.Two;
            CanAtack = false;
        }
    }
    public void TheeToFour()
    {
        if (Stages == MoskitoStages.Three)
        {
            Stages = MoskitoStages.Four;
            atackTimer = AtackStageTime;
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
            StartAtack();
    }
}
