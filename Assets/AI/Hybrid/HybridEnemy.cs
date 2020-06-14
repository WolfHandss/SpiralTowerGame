using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using StateEnemy;

public class HybridEnemy : MonoBehaviour
{
    public GameObject[] PatrolPoints;
    Rigidbody rb;

    public StateMachine<HybridEnemy> stateMachine { get; set; }

    public Transform PlayerPosition;
    public float distance;
    public float stopDistance = 15;
    public int speed = 3;

    private void Awake()
    {

    }

    private void Start()
    {
        stateMachine = new StateMachine<HybridEnemy>(this);
        stateMachine.ChangeState(PatrollingState.Instance);
        rb = GetComponent<Rigidbody>();
        PlayerPosition = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        stateMachine.Update();
        //calculates distance between player and enemy
        distance = Vector3.Distance(PlayerPosition.position, transform.position);
    }
    //function for when enemy lands an attack
    public void calming()
    {
        StartCoroutine(CoolDown());
    }
    //coroutine in which move the enemy backwards for x seconds with x speed
    IEnumerator CoolDown()
    {
        float timer = Time.time + 0.6f;
        Vector3 Direction = transform.forward.normalized;
        Direction.y = 0;
        Direction.Normalize();
        while (timer >= Time.time)
        {
            transform.position += -Direction * 8 * Time.deltaTime;
            yield return null;
        }
        stateMachine.ChangeState(PatrollingState.Instance);
    }
}