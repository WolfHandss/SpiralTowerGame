using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform PlayerTransform;
    private Vector3 _cameraOffset;

    [Range(0.01f, 1.0f)]
    public float SmoothFactor = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        _cameraOffset = transform.position - PlayerTransform.position;
    }

    private void Update()
    {
        //look at player
        Vector3 relativePos = PlayerTransform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(relativePos);
    }

}
