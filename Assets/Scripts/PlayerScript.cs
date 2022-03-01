using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    public float speed = 10;
    public float shrinkingScale;
    public GameObject bulletPrefab;
    public Transform spawnPoint;
    private NavMeshAgent nma;
    private Animator animator;

    private float size = 1.0f;

    private int color_id_sz = 5;
    private int curr_color_id = 0;     // corresponds to 'r'
    private char curr_color_color = 'r';        // used for pickup
    private char[] color_id = {'r', 'g', 'b', 'y', 'p'};
    //                                             'r'                                          'g'                                 'b'                                     'y'                                         'p'
    private Color[] color_colors = {new Color(0.706f, 0.059f, 0.059f, 1.0f), new Color(0.416f, 0.659f, 0.204f, 1.0f), new Color(0.145f, 0.498f, 0.737f, 1.0f), new Color(0.867f, 0.745f, 0.0f, 1.0f), new Color(0.659f, 0.4f, 0.953f, 1.0f)};

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
        if (Input.GetKeyDown(KeyCode.C)){
            curr_color_id++;
            if (curr_color_id >= color_id_sz) curr_color_id = 0;
            curr_color_color = color_id[curr_color_id];
            GetComponentInChildren<Renderer>().material.color = color_colors[curr_color_id];
        }

        if (size <= 0.01) {
            SceneManager.LoadScene("Game_Over");
        }


        size = transform.localScale.x;
        Debug.Log("size: " + size);
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
            GameObject newBullet = Instantiate(bulletPrefab, spawnPoint.position, transform.rotation);
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
        if (other.gameObject.CompareTag("Pickup") && other.transform.localScale.magnitude <= transform.localScale.magnitude){
            // only if same color
            if (other.GetComponent<PickupScript>().color == curr_color_color){
                // increasing size by half of absorbed object's size
                size += other.transform.localScale.magnitude / transform.localScale.magnitude;
                Vector3 newSize = new Vector3(size, size, size);
                transform.localScale = newSize;

                // destroying the object we collected
                Destroy(other.gameObject);                
            }
        }
    }
}
