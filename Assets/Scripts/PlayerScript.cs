using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerScript : MonoBehaviour
{
    public float speed = 10;
    public float shrinkingScale;
    public GameObject bulletPrefab;
    public Transform spawnPoint;
    private NavMeshAgent nma;
    private Animator animator;

    private float size = 1.0f;

    // Start is called before the first frame update
    // Create event
    void Start()
    {
        nma = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        shrinkingScale = -1;
    }

    // Update is called once per frame
    // Step event
    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        //shrinking
        if(transform.localScale.x >.01){
            transform.localScale  += new Vector3(0.01F, .01f, .01f) * shrinkingScale * Time.deltaTime;
        }
        if (input.magnitude <= 0){
            animator.SetBool("moving", false);
            shrinkingScale = -1;
            return;
        }
        if(Input.GetKeyDown(KeyCode.Space)){
            transform.localScale  += new Vector3(0.01F, .01f, .01f) * transform.localScale.x/10f;
            GameObject newBullet = Instantiate(bulletPrefab, spawnPoint.position, transform.rotation);
            newBullet.transform.localScale *= transform.localScale.x;
            newBullet.GetComponent<Rigidbody>().AddForce(transform.forward * 200);
        }

        if (Mathf.Abs(input.y) > 0.01f){
            animator.SetBool("moving", true);
            shrinkingScale = -3;
            Vector3 destination = transform.position + transform.right * input.x + transform.forward * input.y;
            nma.destination = destination;
        } else {
            nma.destination = transform.position;
            animator.SetBool("moving", false);
            transform.Rotate(0, input.x * nma.angularSpeed * Time.deltaTime, 0);
        }
    }

    private void OnTriggerEnter(Collider other) {
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
