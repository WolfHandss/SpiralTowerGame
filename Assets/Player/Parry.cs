using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parry : MonoBehaviour
{
    public float BlockTimer;
    public float ParryCoolDown;
    public float ParryTimer;

    private float Ptimer;
    private float ParryCoolDowntimer;
    private float Blocktimer;
    private float Btimer;

    public GameObject ParryBarrier;
    public GameObject BlockBarrier;

    // Start is called before the first frame update
    void Start()
    {
        ParryBarrier.SetActive(false);
        BlockBarrier.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //if not in cooldown on E press activate parry barier
        if (ParryCoolDowntimer <= Time.time)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Ptimer = Time.time + ParryTimer;
                ParryCoolDowntimer = Time.time + ParryCoolDown;
                Blocktimer = Time.time + BlockTimer;
                ParryBarrier.SetActive(true);
            }
        }
        //if parrybarrier timer is up
        if (Time.time >= Ptimer)
        {
            //if block is activated
            if (Time.time <= Blocktimer)
            {
                //E is held down
                if (Input.GetKey(KeyCode.E))
                {
                    ParryBarrier.SetActive(false);
                    BlockBarrier.SetActive(true);
                }
            }
            else
            {
                ParryBarrier.SetActive(false);
                BlockBarrier.SetActive(false);
            }
        }
        //E is let go
        if (Input.GetKeyUp(KeyCode.E))
        {
            ParryBarrier.SetActive(false);
            BlockBarrier.SetActive(false);

            Blocktimer = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("Bullet"))
        {
            if (Time.time <= Ptimer)
            {
                //if the bullet colides with parry barier reverse the velocity of the bullet
                other.gameObject.GetComponent<Bullet>().ReverseDirection();
            }
            //if block barrier is activated just destroy bullet
            if(BlockBarrier.activeSelf)
            {
                Destroy(other.gameObject);
            }
        }
    }
}
