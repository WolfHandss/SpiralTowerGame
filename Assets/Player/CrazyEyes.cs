using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrazyEyes : MonoBehaviour
{
    public bool increase;
    private float timer;
    public float x;
    public float y;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(timer <= Time.time)
        {
            timer = Time.time + 1;
            increase = !increase;
        }
        if (increase)
            transform.localScale += new Vector3(x, y, 0) * Time.deltaTime *6;
        else
            transform.localScale += new Vector3(-x, -y, 0) * Time.deltaTime *6;
    }
}
