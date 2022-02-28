using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerScript : MonoBehaviour
{
    //private Rigidbody rb;
    public float speed = 10;

    private NavMeshAgent nma;
    private Animator animator;

    // Start is called before the first frame update
    // Create event
    void Start()
    {
        nma = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        //rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    // Step event
    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (input.magnitude <= 0){
            animator.SetBool("moving", false);
            return;
        }

        if (Mathf.Abs(input.y) > 0.01f){
            animator.SetBool("moving", true);
            Vector3 destination = transform.position + transform.right * input.x + transform.forward * input.y;
            nma.destination = destination;
        } else {
            nma.destination = transform.position;
            animator.SetBool("moving", false);
            transform.Rotate(0, input.x * nma.angularSpeed * Time.deltaTime, 0);
        }

        //Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        //rb.AddForce(input * speed * Time.fixedDeltaTime);

        /*float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");     
        gameObject.transform.Translate(new Vector3(x,y,0) * speed * Time.deltaTime);*/
    }
}
