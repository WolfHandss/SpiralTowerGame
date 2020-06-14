using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicInput : MonoBehaviour
{
    // Horizontal movement parameters
    public float speed = 10.0f;
    Vector3 movement;
    Vector3 targetPosition;
    Vector3 movementDirection;
    public GameObject Cam;

    // Jump and Fall parameters
    public float maxJumpSpeed = 0.5f;
    public float maxFallSpeed = -2.2f;
    public float timeToMaxJumpSpeed = 0.2f;
    public float deccelerationDuration = 0.0f;
    public float maxJumpDuration = 1.2f;

    // Jump and Fall helpers
    bool jumpStartRequest = false;
    bool jumpRelease = false;
    bool isMovingUp = false;
    bool isFalling = false;
    float currentJumpDuration = 0.0f;
    float gravityAcceleration = -9.8f;
    float hoverDuration;

    // take damage from fall;
    private float startFallPos;
    private float endFallPos;
    private float fallDistance;
    [SerializeField] public float damageDistance = 4;

    public float groundSearchLength = 0.6f;
    RaycastHit currentGroundHit;

    // Rotation Parameters
    float angleDifferenceForward = 0.0f;

    // Components and helpers
    Rigidbody rigidBody;
    Vector2 input;
    Vector3 playerSize;

    // Debug configuration
    public GUIStyle myGUIStyle;

    // bouncy platform
    private Vector3 lastFrameVelocity;
    private bool isOnBounceGround = false;

    //redfloor
    private float ceilingSearchLength = 0.6f;
    RaycastHit currentCeilingHit;

    //Walls
    public float WallSearchLength = 0.6f;
    RaycastHit currentWallHitRight;
    RaycastHit currentWallHitLeft;
    RaycastHit currentWallHit;
    Vector3 worldHitResualt = Vector3.zero;
    Vector3 worldHitNormalResualt = Vector3.zero;

    //double jump
    private bool doubleJump = false;
    float doublejumpTimer = 0;
    private bool comingdown = false;



    //dash
    private float dashTimer;
    public float DashDuration;
    public int dashSpeed;
    private float dashCDTimer;
    public float dashCoolDownDuration;

    PlayerControl PC;
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        playerSize = GetComponent<Collider>().bounds.size;
        PC = GetComponent<PlayerControl>();
    }

    void Start()
    {
        jumpStartRequest = false;
        jumpRelease = false;
        isMovingUp = false;
        isFalling = false;
        doubleJump = false;
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        input = new Vector2();
        input.x = horizontal;
        input.y = vertical;
        //if player pressed the jump button
        if (Input.GetButtonDown("Jump"))
        {
            jumpStartRequest = true;
            jumpRelease = false;
            if (doubleJump)
            {
                StopFalling();
            }
        }//if player released jump button
        else if (Input.GetButtonUp("Jump"))
        {
            jumpRelease = true;
            jumpStartRequest = false;
        }
        //if dash button is hit and dash cooldown is over
        if (dashCDTimer < Time.time)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                dashTimer = Time.time + DashDuration;
                dashCDTimer = Time.time + dashCoolDownDuration;
            }
        }
    }
    //function to call when player starts falling
    void StartFalling()
    {
        isMovingUp = false;
        isFalling = true;
        currentJumpDuration = 0.0f;
        jumpRelease = false;
        startFallPos = transform.position.y;
        hoverDuration = Time.time + 0.3f;
    }
    //function to call when player stops falling
    void StopFalling()
    {
        isFalling = false;
        jumpRelease = false;

    }

    void FixedUpdate()
    {
        // Calculate horizontal movement
        if (dashTimer <= Time.time)
        {
            movement = Cam.transform.right * input.x * speed * Time.fixedDeltaTime;
            movement += Cam.transform.forward * input.y * speed * Time.fixedDeltaTime;
            movement.y = 0.0f;
        }

        //dash
        if(dashTimer > Time.time)
        {
            movement = Cam.transform.right * input.x * dashSpeed * Time.fixedDeltaTime;
            movement += Cam.transform.forward * input.y * dashSpeed * Time.fixedDeltaTime;
            movement.y = 0;
        }

        // wall collision
        if (isHittingWall() && movement.sqrMagnitude > Mathf.Epsilon)
        {
            Vector3 PlaneNormal = currentWallHit.normal;
            Vector3 playerV = movement;
            playerV.y = 0;
            PlaneNormal.y = 0;
            PlaneNormal.Normalize();

            Vector3 resaultProjection = Vector3.Project(playerV, PlaneNormal);
            playerV -= resaultProjection * 2;
            movement.x = playerV.x;
            movement.z = playerV.z;
        }

        targetPosition = rigidBody.position + movement;


        // Calculate Vertical movement
        float targetHeight = 0.0f;
        //jumping
        if (!isMovingUp && ((jumpStartRequest && (doubleJump || isOnGround())) || isOnBounceGround))
        {
            //check for double jump
            if (!isOnGround())
            {
                doubleJump = false;
                maxJumpDuration -= 0.1f;
            }

            isMovingUp = true;
            jumpStartRequest = false;
            currentJumpDuration = 0.0f;
        }

        //if moving up
        if ((isMovingUp) && !isHittingCeiling())
        {

            //check if jump button is released or jump duration is over
            if (jumpRelease || currentJumpDuration >= maxJumpDuration)
            {
                StartFalling();
            }
            else
            {
                //if the platform is bounce able or not
                if (!isOnBounceGround)
                {
                    float currentYpos = rigidBody.position.y;
                    float newVerticalVelocity = maxJumpSpeed + gravityAcceleration * Time.deltaTime;
                    targetHeight = currentYpos + (newVerticalVelocity * Time.deltaTime) + (maxJumpSpeed * Time.deltaTime * Time.deltaTime);

                    currentJumpDuration += Time.deltaTime;
                }
                else
                {
                    isOnBounceGround = false;
                }
            }
        }//ifnot moving up or on ground or falling, start falling
        else if (!isOnGround() && !isFalling)
        {
            StartFalling();

        }

        if (isFalling)
        {
            //if on ground
            if (isOnGround())
            {

                lastFrameVelocity = rigidBody.velocity;
                //check if landed platform is bouncing floor or not
                if (currentGroundHit.transform.gameObject.tag == ("BounceFloor"))
                {
                    isOnBounceGround = true;
                }
                else
                {
                    isOnBounceGround = false;
                }
                // End of falling state. No more height adjustments required, just snap to the new ground position
                isFalling = false;
                targetHeight = currentGroundHit.point.y + (0.5f * playerSize.y) /* Time.fixedDeltaTime*/;
                endFallPos = transform.position.y;
                PC.fallDamage(startFallPos - endFallPos);

                if (!doubleJump)
                    maxJumpDuration += 0.1f;
                doubleJump = true;
            }
            else//if not on ground
            {
                float currentYpos = rigidBody.position.y;
                float currentYvelocity = rigidBody.velocity.y;

                float newVerticalVelocity = maxFallSpeed + gravityAcceleration * Time.deltaTime;
                //if hover duration is not over and jump key is held
                if (hoverDuration >= Time.time && Input.GetKey(KeyCode.Space))
                {
                    targetHeight = currentYpos + (-1 * Time.deltaTime) + (0.5f * maxFallSpeed * Time.deltaTime * Time.deltaTime);
                }
                else
                {
                    targetHeight = currentYpos + (newVerticalVelocity * Time.deltaTime) + (0.5f * maxFallSpeed * Time.deltaTime * Time.deltaTime);
                }
            }
        }

        if (targetHeight > Mathf.Epsilon)
        {
            // Only required if we actually need to adjust height
            targetPosition.y = targetHeight;
        }

        // Calculate new desired rotation
        movementDirection = targetPosition - rigidBody.position;
        movementDirection.y = 0.0f;
        movementDirection.Normalize();


        Vector3 currentFacingXZ = transform.forward;
        currentFacingXZ.y = 0.0f;

        angleDifferenceForward = Vector3.SignedAngle(movementDirection, currentFacingXZ, Vector3.up);
        Vector3 targetAngularVelocity = Vector3.zero;
        targetAngularVelocity.y = angleDifferenceForward * Mathf.Deg2Rad;

        Quaternion syncRotation = Quaternion.identity;
        syncRotation = Quaternion.LookRotation(movementDirection);



        // Finally, update RigidBody    
        rigidBody.MovePosition(targetPosition);

        if (movement.sqrMagnitude > Mathf.Epsilon)
        {
            // Currently we only update the facing of the character if there's been any movement
            rigidBody.MoveRotation(syncRotation);
        }
    }

    //linecast to check if the player is one the ground
    private bool isOnGround()
    {
        Vector3 lineStart = transform.position;
        Vector3 vectorToSearch = new Vector3(lineStart.x, lineStart.y - groundSearchLength, lineStart.z);

        Debug.DrawLine(lineStart, vectorToSearch);

        return Physics.Linecast(lineStart, vectorToSearch, out currentGroundHit);
    }

    //linecast to check if there is a wall infront of the player
    private bool isHittingCeiling()
    {
        Vector3 lineStart = transform.position;
        Vector3 vectorToSearch = new Vector3(lineStart.x, lineStart.y + ceilingSearchLength, lineStart.z);

        Debug.DrawLine(lineStart, vectorToSearch);

        if (Physics.Linecast(lineStart, vectorToSearch, out currentCeilingHit))
        {

            if (currentCeilingHit.transform.gameObject.tag == ("RedFloor"))
                return true;
            else
                return false;
        }
        else
        {
            return false;
        }
    }

    //linecast to check if there is a wall infront of the player
    private bool isHittingWall()
    {
        Debug.DrawLine(rigidBody.position, rigidBody.position + movementDirection);

        if (Physics.Linecast(rigidBody.position, rigidBody.position + movementDirection * 2.0f, out currentWallHit))
        {
            if (currentWallHit.transform.gameObject.tag == ("Wall"))
                return true;
            else
                return false;
        }
        else
        {
            return false;
        }
    }

    void OnGUI()
    {
        // Add here any debug text that might be helpful for you
        GUI.Label(new Rect(10, 10, 100, 20), "Angle " + angleDifferenceForward.ToString(), myGUIStyle);
    }

    private void OnDrawGizmos()
    {
        // Debug Draw last ground collision, helps visualize errors when landing from a jump
        if (currentGroundHit.collider != null)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(currentGroundHit.point, 0.25f);
        }
    }

    void OnCollisionStay(Collision collisionInfo)
    {
        // Debug-draw all contact points and normals, helps visualize collisions when the physics of the RigidBody are enabled (when is NOT Kinematic)
        foreach (ContactPoint contact in collisionInfo.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal * 10, Color.white);
        }
    }
}
