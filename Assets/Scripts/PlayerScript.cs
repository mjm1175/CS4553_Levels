using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private float speed = 10;

    // Start is called before the first frame update
    // Create event
    void Start()
    {
        
    }

    // Update is called once per frame
    // Step event
    void Update()
    {
        if(Input.GetKey("space")){
            transform.Translate(Vector3.left * Time.deltaTime *speed, Space.World);
        }
    }
}
