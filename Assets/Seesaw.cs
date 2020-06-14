using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seesaw : MonoBehaviour
{
    RaycastHit seesawHit;
    private float speed = 6;
    public GameObject seesaw;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lineStart = transform.position;
        Vector3 vectorToSearch = new Vector3(lineStart.x, lineStart.y - 0.6f, lineStart.z);

        Debug.DrawLine(lineStart, vectorToSearch);
        
        if (Physics.Linecast(lineStart, vectorToSearch, out seesawHit))
        {
            if(seesawHit.transform.gameObject.tag == ("Left"))
            {
                seesaw.transform.Rotate(Vector3.forward * speed * Time.deltaTime);
            }
            if (seesawHit.transform.gameObject.tag == ("RightCollider"))
            {
                seesaw.transform.Rotate(Vector3.forward * speed * Time.deltaTime * -1);
            }
        }
    }

}
