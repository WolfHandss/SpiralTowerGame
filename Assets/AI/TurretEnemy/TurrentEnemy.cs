using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateEnemy;

public class TurrentEnemy : MonoBehaviour
{

    public GameObject Player;
    public GameObject Bullet;
    public GameObject[] muzzle;

    public int bulletSpeed = 20;
    public int secondsBetweenShots = 1;
    

    public StateMachine<TurrentEnemy> stateMachine { get; set; }

    private void Awake()
    {
        
    }

    private void Start()
    {
        stateMachine = new StateMachine<TurrentEnemy>(this);
        stateMachine.ChangeState(TurretIdleEnemy.Instance);
    }

    private void Update()
    {
        stateMachine.Update();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == ("Player"))
        {
            Player = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == ("Player"))
        {
            Player = null;
        }
    }

    public void ShootBullet()
    {
        for(int i = 0; i < muzzle.Length; i ++)
        {
            //instantiate game object
            GameObject B = Instantiate(Bullet, muzzle[i].transform.position, muzzle[i].transform.rotation);
            //give the bullet the direction
            B.GetComponent<Bullet>().SetDirection(muzzle[i].transform.forward.normalized);
            //destroy after 5 seconds
            Destroy(B, 5);
        }
    }
}
