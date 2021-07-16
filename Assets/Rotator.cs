using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    private bool CanRotate = false;
    public bool IsRotating { get; private set; }
    private float RotateTimer;
    private float KDtimer;
    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
        var stats = gameObject.GetComponent<JagerStats>();
        if (IsRotating)
        {
            if (RotateTimer <= 0)
            {
                IsRotating = false;
                KDtimer = stats.atackKD;
            }
            else
            {
                gameObject.transform.Rotate(0f, 0f,- stats.rotateSpeed*1000 * Time.deltaTime);
                RotateTimer -= Time.deltaTime;
            }
        }
        if (!IsRotating && !CanRotate )
        {   if (KDtimer > 0)
                KDtimer -= Time.deltaTime;
            else
                CanRotate = true;
        }
    }
    public void StartRotate()
    {
        if (CanRotate)
        {
            var stats = gameObject.GetComponent<JagerStats>();
            RotateTimer = stats.atackTime;
            IsRotating = true;
            CanRotate = false;
        }
    }
}
