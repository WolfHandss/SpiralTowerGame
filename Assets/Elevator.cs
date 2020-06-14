using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    //elevator parts
    public GameObject End;
    public GameObject EObject;
    public GameObject start;

    private Vector3 goTo;
    [SerializeField] private float speed;

    private float toEnd;
    private float toStart;

    // Update is called once per frame
    void Update()
    {
        //get the distance between elevator and destination
        toEnd = Vector3.Distance(End.transform.position, EObject.transform.position);
        toStart = Vector3.Distance(start.transform.position, EObject.transform.position);

        //check if destination reached switch dest
        if (toEnd < 0.5f)
        {
            SetDestination(start);
        }
        else if(toStart <0.5f)
        {
            SetDestination(End);
        }
    }

    private void FixedUpdate()
    {
        //moves the elevator towards the direction of the objective
        EObject.transform.position += goTo * speed * Time.deltaTime;
    }

    //function that takes in the gameobject it should move towards and sets direction
    void SetDestination(GameObject E)
    {
        goTo = E.transform.position - EObject.transform.position;
        goTo.Normalize();
    }

}
