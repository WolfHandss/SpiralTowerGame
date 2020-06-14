using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 Direction;
    [SerializeField] private float speed;

    private GameObject Player;
    private GameObject turretEnemy;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //calculate movement
        transform.position += Direction * speed * Time.deltaTime;
    }

    //set direction
    public void SetDirection(Vector3 d)
    {
        d.Normalize();
        Direction = d;
    }

    //reverse direction
    public void ReverseDirection()
    {
        Direction = -Direction;
    }

    // on collision with player do damage and get destroyed
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerControl>().ChangeHealth(-10);
            Destroy(gameObject);
        }
    }
}
