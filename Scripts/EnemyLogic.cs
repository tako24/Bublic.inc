using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.Linq;

public enum State
{
    Patrooling,
    Atack
}
public class EnemyLogic : MonoBehaviour
{
    [SerializeField]
    private GameObject[] patroolPoints;
    public AIPath AIPath;
    public AIDestinationSetter AIDestinationSetter;
    public State state;
    private GameObject player;
    

    void Start()
    {
        patroolPoints = GameObject.FindGameObjectsWithTag("patrol").Where(x=>x.transform.parent.gameObject.GetHashCode()==gameObject.transform.parent.gameObject.GetHashCode()).ToArray();
        player = GameObject.Find("Player");
        if (patroolPoints.Length != 0)
        {
            state = State.Patrooling;
            Patroling();
        }
    }
    public void Atack()
    {
        state = State.Atack;
        AIDestinationSetter.target = player.transform;
    }
    public void StopAtack()
    {
        state = State.Patrooling;
        Patroling();
    }
    public void Patroling()
    {   
        var target = patroolPoints[Random.Range(0, patroolPoints.Length - 1)];
        AIDestinationSetter.target = target.transform;
    }
    // Update is called once per frame
    void Update()
    {
        if (AIPath.isStopped)
            StopAtack();
        if (state == State.Patrooling && AIPath.reachedDestination)
            Patroling();
    }
}
