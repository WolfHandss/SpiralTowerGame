using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    private float speed;
    public float overTime;
    public GameObject final;

    public UI canvas;

    // Start is called before the first frame update
    void Start()
    {
        CalculateVelocity();
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UI>();
    }

    // Update is called once per frame
    void Update()
    {
        //moves gameobject
        transform.position += Vector3.up * speed * Time.deltaTime;
    }

    //calculate the speed of lava based on the time
    private void CalculateVelocity()
    {
        speed = (final.transform.position - transform.position).magnitude / overTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        //player touches lava call the death function
        if(other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerControl>().Death();
        }
    }
}
