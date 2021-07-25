using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.Linq;
using System;

public enum State
{
    Patrooling,
    Atack,
    Stop,
    MoveToPoint
}
public enum LogicType
{
    flyingMob,
    distanceMob,
    commonMob,
    jager,
    moskito
}
public class EnemyLogic : MonoBehaviour
{
    [SerializeField]
    private GameObject[] patroolPoints;
    public AIPath AIPath;
    public AIDestinationSetter AIDestinationSetter;
    public State state;
    public LogicType LogicType;
    private GameObject player;
    public float distanceToPlayer;
    public float baseDistance;
    public float runningAwayDistance=3.2f;
    private GameObject[] flyingPoints;
    

    void Start()
    {
        patroolPoints = GameObject.FindGameObjectsWithTag("patrol").Where(x=>x.transform.parent.gameObject.GetHashCode()==gameObject.transform.parent.gameObject.GetHashCode()).ToArray();
        flyingPoints= GameObject.FindGameObjectsWithTag("flypoint").Where(x => x.transform.parent.gameObject.GetHashCode() == gameObject.transform.parent.gameObject.GetHashCode()).ToArray();
        player = GameObject.Find("Player");
        if (LogicType == LogicType.distanceMob)
        {
            gameObject.transform.GetChild(1).localScale = Vector2.one * runningAwayDistance;
        }
        if (LogicType == LogicType.flyingMob)
        {
            state = State.Stop;
            //if(flyingPoints.Length!=0)
            //    gameObject.transform.position = flyingPoints[UnityEngine.Random.Range(0, flyingPoints.Length)].transform.position;
        }
        if (patroolPoints.Length != 0 && LogicType != LogicType.flyingMob)
        {
            state = State.Patrooling;
            Patroling();
        }
    }
    private void LookAtPlayer()
    {
        var playerPos = GameController.Player.transform.position;
        var enemyPos = transform.position;
        var animator = GetComponent<Animator>();

        if (playerPos.x > enemyPos.x)
        {
            animator.SetBool("LookRight", true);
            animator.SetBool("LookLeft", false);
        }
        else
        {
            animator.SetBool("LookRight", false);
            animator.SetBool("LookLeft", true);
        }

        if (playerPos.y > enemyPos.y)
        {
            animator.SetBool("LookUp", true);
            animator.SetBool("LookDown", false);
        }
        else
        {
            animator.SetBool("LookUp", false);
            animator.SetBool("LookDown", true);
        }

        //gameObject.transform.LookAt(player.transform);
        //AIPath.enableRotation = false;
    }
    public void Atack()
    {
        state = State.Atack;
        if(LogicType!=LogicType.distanceMob&& LogicType != LogicType.moskito&&LogicType!=LogicType.jager)
            AIDestinationSetter.target = player.transform;
        if (LogicType == LogicType.distanceMob)
        {
            gameObject.GetComponent<ShootSystem>().IsShooting = true;
            LookAtPlayer();
            AIDestinationSetter.target = player.transform;
        }

    }


    public void StopAtack()
    {
        if (LogicType != LogicType.flyingMob)
        {
            state = State.Patrooling;
            Patroling();
        }
        else state = State.Stop;
    }
    public void Patroling()
    {   
        var target = patroolPoints[UnityEngine.Random.Range(0, patroolPoints.Length - 1)];
        AIDestinationSetter.target = target.transform;
    }

    public void RunAway()
    {

    }
    // Update is called once per frame
    void Update()
    {
        if (state == State.Patrooling && AIPath.reachedDestination)
            Patroling();
        if (LogicType == LogicType.distanceMob && (state == State.Atack||state==State.MoveToPoint))
        {
            LookAtPlayer();
        }
        else AIPath.enableRotation = true;
        if (LogicType == LogicType.distanceMob)
        {
            var collider = gameObject.GetComponentInChildren<PolygonCollider2D>();
            var playerCollider = player.GetComponent<Collider2D>();
            if (state == State.Atack && collider.IsTouching(playerCollider))
            {
                state = State.MoveToPoint;
                var point = patroolPoints.Where(x => (x.transform.position - player.transform.position).sqrMagnitude > runningAwayDistance * runningAwayDistance).OrderBy(x => Math.Abs((x.transform.position - gameObject.transform.position).magnitude)).First();
                AIDestinationSetter.target = point.transform;
                AIPath.endReachedDistance = baseDistance;
            }
            else if ( state == State.MoveToPoint && !collider.IsTouching(playerCollider))
            {
                AIDestinationSetter.target = player.transform;
                state = State.Atack;
                AIPath.endReachedDistance = distanceToPlayer;
            }
        }
        if (LogicType == LogicType.flyingMob)
        {
            if (state == State.Atack && gameObject.GetComponent<Collider2D>().IsTouching(player.GetComponent<Collider2D>()))
            {
                if (flyingPoints.Length != 0)
                    AIDestinationSetter.target = flyingPoints[UnityEngine.Random.Range(0, flyingPoints.Length)].transform;
                state = State.MoveToPoint;
            }
            else if (state == State.MoveToPoint && AIPath.reachedEndOfPath)
            {
                AIDestinationSetter.target = player.transform;
                state = State.Atack;
            }
        }
        if (LogicType == LogicType.jager)
        {
            var stats = gameObject.GetComponentInChildren<Rotator>();
            if (stats.IsRotating)
            {
                state = State.Stop;
            }
            else
            {
                state = State.Patrooling;
            }
        }
    }
}
