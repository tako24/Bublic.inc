using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsMove : MonoBehaviour
{
    public float time=0;
    public float amp=0.25f;
    public float freq=2; //частота
    public float offset=0; //смещение
    private Vector2 startPos;
    public bool isPicked = false;
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        if (!isPicked)
        {
                time += Time.deltaTime;
                offset = amp * Mathf.Sin(time * freq);
                transform.position = startPos + new Vector2(0, offset);
        }
    }
}
