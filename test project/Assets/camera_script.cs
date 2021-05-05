using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_script : MonoBehaviour
{
    public GameObject follow;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, follow.transform.position.y +10,transform.position.z);
        //transform.Translate(Vector3.right/4);
        //transform.LookAt(follow.transform);
    }
}
