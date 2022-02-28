using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerScript : MonoBehaviour
{
    public float speed = 10;

    private NavMeshAgent nma;
    private Animator animator;

    private float size = 1.0f;

    // Start is called before the first frame update
    // Create event
    void Start()
    {
        nma = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
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
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Pickup") && other.transform.localScale.magnitude <= size){
            // increasing size by half of absorbed object's size
            size += other.transform.localScale.magnitude / 2;
            Vector3 newSize = new Vector3(size, size, size);
            transform.localScale = newSize;

            // destroying the object we collected
            Destroy(other.gameObject);
        }
    }
}
