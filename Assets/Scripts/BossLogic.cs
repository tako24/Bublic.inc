using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BossLogic : MonoBehaviour
{
    public AIPath AIPath;
    public AIDestinationSetter destinationSetter;
    public GameObject lasergroup1;
    public GameObject lasergroup2;
    public GameObject lasergroup3;
    public float FirstStageAtackKD;
    public float TurboSpeed;
    public float NormalSpeed;
    public GameObject shild;
    public float SecondStageWaitingOnPoint;
    public float distanseNoFlySpawn;
    public float minFlySpawn;
    public GameObject fly;
    public SecondStageController SecondStageController;
    public float runningAwayDistance;

    private BossStage BossStage = BossStage.Sleep;
    private BossFirstStage firstStage = BossFirstStage.None;
    private GameObject[] movingPoints;
    private GameObject centerPoint;
    private GameObject player;
    private GameObject[] flyingPoints;
    private GameObject[] flySpawnPoint;
    private BossThirdStage BossThirdStage=BossThirdStage.Atack;
    private float waitingOnPoint=0;
    private float FirstStageAtackTime = 0;
    private bool IsWaitingOnPoint = false;
    private HP hP;

    // Start is called before the first frame update
    void Start()
    {
        movingPoints = GameObject.FindGameObjectsWithTag("bossMovingPoints").Where(x => x.transform.parent.gameObject.GetHashCode() == gameObject.transform.parent.gameObject.GetHashCode()).ToArray(); ;
        flyingPoints = GameObject.FindGameObjectsWithTag("flypoint").Where(x => x.transform.parent.gameObject.GetHashCode() == gameObject.transform.parent.gameObject.GetHashCode()).ToArray();
        flySpawnPoint = GameObject.FindGameObjectsWithTag("flyspawn");
        centerPoint = GameObject.FindGameObjectWithTag("center");
        hP = gameObject.GetComponent<HP>();
        player = GameObject.Find("Player");
        destinationSetter.target = centerPoint.transform;
        AIPath.canSearch = false;
        //var l1 = lasergroup1.GetComponentsInChildren<ShootSystem>();
        //var l2 = lasergroup2.GetComponentsInChildren<ShootSystem>();
        //var l3 = lasergroup3.GetComponentsInChildren<ShootSystem>();
        //foreach(var e in l1)
        //{
        //    e.weaponStats = GetComponent<WeaponStats>();
        //}
        //foreach (var e in l2)
        //{
        //    e.weaponStats = GetComponent<WeaponStats>();
        //}
        //foreach (var e in l3)
        //{
        //    e.weaponStats = GetComponent<WeaponStats>();
        //}
    }

    // Update is called once per frame
    void Update()
    {
        StageCheck();
        if (BossStage == BossStage.First && AIPath.reachedDestination)
        {
            Patroling();
        }
        if (BossStage == BossStage.First)
        {
            FirstStageAtackTime += Time.deltaTime;
        }
        if (FirstStageAtackTime >= FirstStageAtackKD)
        {
            FirstStageAtackTime = 0;
            BossStage = BossStage.FirstStageAtack;
            lasergroup1.SetActive(true);
            lasergroup1.GetComponent<BossShootSystem>().StartShooting();
            destinationSetter.target = centerPoint.transform;
            AIPath.maxSpeed = TurboSpeed;
            firstStage = BossFirstStage.First;
            gameObject.tag = "Untagged";
            shild.SetActive(true);
        }
        if (BossStage == BossStage.FirstStageAtack)
        {
            if (!lasergroup1.GetComponent<BossShootSystem>().IsShooting()&&firstStage==BossFirstStage.First)
            {
                lasergroup1.SetActive(false);
                lasergroup2.SetActive(true);
                lasergroup2.GetComponent<BossShootSystem>().StartShooting();
                firstStage = BossFirstStage.Second;
            }
            if (!lasergroup2.GetComponent<BossShootSystem>().IsShooting() && firstStage == BossFirstStage.Second)
            {
                lasergroup2.SetActive(false);
                lasergroup3.SetActive(true);
                lasergroup3.GetComponent<BossShootSystem>().StartShooting();
                firstStage = BossFirstStage.Third;
            }
            if (!lasergroup3.GetComponent<BossShootSystem>().IsShooting() && firstStage == BossFirstStage.Third)
            {
                lasergroup3.SetActive(false);
                AIPath.maxSpeed = NormalSpeed;
                BossStage = BossStage.First;
                shild.SetActive(false);
                firstStage = BossFirstStage.None;
                gameObject.tag = "Enemy";
            }

        }
        if (BossStage == BossStage.Second&&!IsWaitingOnPoint)
        {
            var point= flyingPoints[UnityEngine.Random.Range(0, flyingPoints.Length - 1)];
            gameObject.transform.position = point.transform.position;
            destinationSetter.target = point.transform;
            IsWaitingOnPoint = true;
            waitingOnPoint = 0;
        }
        if ( IsWaitingOnPoint&&waitingOnPoint >= SecondStageWaitingOnPoint && BossStage == BossStage.Second)
        {
            IsWaitingOnPoint = false;
            if (Math.Abs((player.transform.position - gameObject.transform.position).magnitude) >= distanseNoFlySpawn)
            {
                SpawnFly();
                SecondStageController.StartStage();
                gameObject.SetActive(false);
            }
        }
        else
        {
            if(IsWaitingOnPoint && BossStage == BossStage.Second)
            {
                waitingOnPoint += Time.deltaTime;
            }
        }
        if(BossStage == BossStage.Third)
        {
            destinationSetter.target = player.transform;
            AIPath.maxSpeed = TurboSpeed;
        }
        if (BossThirdStage==BossThirdStage.Atack && BossStage==BossStage.Third && (player.transform.position - gameObject.transform.position).sqrMagnitude <= runningAwayDistance * runningAwayDistance)
        {
            if (flyingPoints.Length != 0)
                destinationSetter.target = flyingPoints[UnityEngine.Random.Range(0, flyingPoints.Length)].transform;
            BossThirdStage = BossThirdStage.MoveToPoint;
        }else
        if (BossThirdStage == BossThirdStage.MoveToPoint && BossStage == BossStage.Third && AIPath.reachedEndOfPath)
        {
            destinationSetter.target = player.transform;
            BossThirdStage = BossThirdStage.Atack;
        }
    }

    private void SpawnFly()
    {
        if (minFlySpawn >= flySpawnPoint.Length)
            return;
        var count = 0;
        var spawnFly = UnityEngine.Random.Range(minFlySpawn, flySpawnPoint.Length);
        foreach (var e in flySpawnPoint)
        {
            if (count >= spawnFly)
                break;
            var newFly = Instantiate(fly, e.transform.position, Quaternion.identity);
            newFly.transform.parent = gameObject.transform.parent;
            count++;
        }
    }

    public void Activate()
    {
        BossStage = BossStage.First;
        AIPath.slowWhenNotFacingTarget = false;
        AIPath.canSearch = true;
    }
    private void Patroling()
    {
            var target = movingPoints[UnityEngine.Random.Range(0, movingPoints.Length - 1)];
            destinationSetter.target = target.transform;
    }

    private void StageCheck()
    {
        var hpPersent = (hP._currentHP*1f / hP._maxHP) * 100;
        if (hpPersent <= 70 && BossStage == BossStage.First)
        {
            BossStage = BossStage.Second;
        }
        if (hpPersent <= 30 && BossStage == BossStage.Second)
        {
            BossStage = BossStage.Third;
        }
    }
}
public enum BossStage
{
    Sleep,
    First,
    FirstStageAtack,
    Second,
    Third
}
public enum BossFirstStage
{
    First,
    Second,
    Third,
    None
}

public enum BossThirdStage
{
    Atack,
    MoveToPoint
}
