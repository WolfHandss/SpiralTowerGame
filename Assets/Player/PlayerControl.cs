using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    Color colorStartMajorDamage = Color.red;
    Color colorNoDamage = Color.blue;
    Color colorStartMediumDamage = Color.yellow;
    float damageDisplayDuration = 0.8f;
    Renderer rend;

    float currentMajorDamageTimer = 0.0f;
    float currentMediumDamageTimer = 0.0f;

    public float mediumDamageCollisionForce = 200.0f;
    public float majorDamageCollisionForce = 350.0f;

    KinematicInput IK;

    //health
    [SerializeField] private int health = 100;
    public Slider healthSlider;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        colorNoDamage = rend.material.color;

        currentMajorDamageTimer = 0.0f;
        currentMediumDamageTimer = 0.0f;

        IK = GetComponent<KinematicInput>();
        healthSlider.value = health;
    }

    // Update is called once per frame
    void Update()
    {
        //for flashing when damaged
        if (currentMajorDamageTimer > 0.0f )
        {
            float lerp = Mathf.PingPong(Time.time, currentMajorDamageTimer) / currentMajorDamageTimer;
            rend.material.color = Color.Lerp(colorStartMajorDamage, colorNoDamage, lerp);

            currentMajorDamageTimer -= Time.deltaTime;
        }
        else if ( currentMediumDamageTimer > 0.0f )
        {
            float lerp = Mathf.PingPong(Time.time, currentMediumDamageTimer) / currentMediumDamageTimer;
            rend.material.color = Color.Lerp(colorStartMediumDamage, colorNoDamage, lerp);

            currentMediumDamageTimer -= Time.deltaTime;
        }
        else
        {
            rend.material.color = colorNoDamage;
        }

        killEnemy();
    }

    private void OnCollisionEnter(Collision collision)
    {

        float collisionForce = collision.impulse.magnitude / Time.fixedDeltaTime;

        //do damage based on collision force
        if (collisionForce < 100.0F)
        {
            // This collision has not damaged anyone...
        }
        else if (collisionForce < 200.0F)
        {
            currentMediumDamageTimer = damageDisplayDuration;
            rend.material.color = colorStartMediumDamage;
            ChangeHealth(-10);
        }
        else
        {
            currentMajorDamageTimer = damageDisplayDuration;
            rend.material.color = colorStartMajorDamage;
            ChangeHealth(-15);
        }

        
    }
    //fall damage based on distance fell
    public void fallDamage(float distance)
    {

        if (IK.damageDistance + 1.0f > distance && distance >= IK.damageDistance)
        {
            print("small fall");
            currentMediumDamageTimer = damageDisplayDuration;
            rend.material.color = colorStartMediumDamage;
            ChangeHealth(-5);
        }
        else if (distance >= IK.damageDistance + 1.0f)
        {
            print("big fall");
            currentMajorDamageTimer = damageDisplayDuration;
            rend.material.color = colorStartMajorDamage;
            ChangeHealth(-10);
        }
    }
    //function to modify health and check if player is dead
    public void ChangeHealth(int damage)
    {
        if(health <= 100)
        {
            currentMajorDamageTimer = damageDisplayDuration;
            rend.material.color = colorStartMajorDamage;
            health += damage;
            healthSlider.value = health;
        }
        if(health <= 0)
        {
            Death();
        }

    }
    //function to call when player dies to enable gameover UI
    public void Death()
    {
        GameObject.FindGameObjectWithTag("Canvas").GetComponent<UI>().GameOver();
    }

    //function to check if the player is on top of an enemy and if so destroy the enemy
    private void killEnemy()
    {
        Vector3 lineStart = transform.position;
        Vector3 vectorToSearch = new Vector3(lineStart.x, lineStart.y - 0.6f, lineStart.z);

        Debug.DrawLine(lineStart, vectorToSearch);
        RaycastHit enemyHit;
        if (Physics.Linecast(lineStart, vectorToSearch, out enemyHit))
        {
            if (enemyHit.transform.gameObject.CompareTag("Kill"))
            {
                Destroy(enemyHit.transform.gameObject);
            }
        }
    }
}
