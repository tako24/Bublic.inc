using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondStageController : MonoBehaviour
{
    private bool IsStarted=false;
    public GameObject boss;
    public int damageInPersent;
    // Update is called once per frame
    public void StartStage()
    {
        IsStarted = true;
    }
    void Update()
    {
        if (IsStarted)
        {
            var flyes = gameObject.GetComponentsInChildren<EnemyLogic>();
            if (flyes.Length == 0)
            {
                IsStarted = false;
                boss.SetActive(true);
                var HP = boss.GetComponent<HP>();
                HP.TakeDamage((HP._maxHP * damageInPersent) / 100);
            }
            else
            {
                foreach(var e in flyes)
                {
                    e.Atack();
                }
            }
        }
        
    }
}
