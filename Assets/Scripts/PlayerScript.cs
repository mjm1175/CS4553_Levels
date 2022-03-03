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
    private bool dying = false;

    private float size = 1.0f;
    private float maxSize = 100f;
    public SizeBar sizeBar;

    private Renderer rnd;

    //private int color_id_sz = 5;
    private int curr_color_id = 0;     // corresponds to 'r'
    private char curr_color_color = 'r';        // used for pickup
    private char[] color_id = {'r', 'g', 'b', 'y', 'p'};
    //                                             'r'                                          'g'                                 'b'                                     'y'                                         'p'
    private Color[] color_colors = {new Color(0.706f, 0.059f, 0.059f, 1.0f), new Color(0.416f, 0.659f, 0.204f, 1.0f), new Color(0.216f, 0.384f, 0.945f, 1.0f), new Color(0.867f, 0.745f, 0.0f, 1.0f), new Color(0.659f, 0.4f, 0.953f, 1.0f)};

    private Coroutine thisCollisionCrt;

    // Start is called before the first frame update
    // Create event
    void Start()
    {
        nma = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        shrinkingScale = -1;
        rnd = GetComponentInChildren<Renderer>();
    }

    // Update is called once per frame
    // Step event
    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        size = transform.localScale.x;
        sizeBar.SetSize(size);
        nma.speed = size * size + 5;
        Debug.Log("size: " + size);

        // color change
        switch (Input.inputString){
            // r
            case "1":
                curr_color_id = 0;
                curr_color_color = color_id[curr_color_id];
                rnd.material.color = color_colors[curr_color_id];
                break;
            // g 
            case "2":
                curr_color_id = 1;
                curr_color_color = color_id[curr_color_id];
                rnd.material.color = color_colors[curr_color_id];
                break;                
            // b
            case "3":
                curr_color_id = 2;
                curr_color_color = color_id[curr_color_id];
                rnd.material.color = color_colors[curr_color_id];
                break;                
            // y
            case "4":
                curr_color_id = 3;
                curr_color_color = color_id[curr_color_id];
                rnd.material.color = color_colors[curr_color_id];
                break;                
            // p
            case "5":
                curr_color_id = 4;
                curr_color_color = color_id[curr_color_id];
                rnd.material.color = color_colors[curr_color_id];
                break;                
        }
        /*if (Input.GetKeyDown(KeyCode.C)){
            curr_color_id++;
            if (curr_color_id >= color_id_sz) curr_color_id = 0;
            curr_color_color = color_id[curr_color_id];
            GetComponentInChildren<Renderer>().material.color = color_colors[curr_color_id];
        }*/

        //death
        if (transform.localScale.x < 0.2){
            if (!dying){
                StopAllCoroutines();
                StartCoroutine(DeathScene());
            } 
            dying = true;
            return;
        }

        // shooting
        if(Input.GetKeyDown(KeyCode.Space)){
            transform.localScale  += new Vector3(0.01F, .01f, .01f) * transform.localScale.x/10f;
            GameObject newBullet = Instantiate(bulletPrefab, spawnPoint.position, transform.rotation);
            newBullet.GetComponent<Renderer>().material.color = color_colors[curr_color_id];
            newBullet.transform.localScale *= transform.localScale.x;
            newBullet.GetComponent<Rigidbody>().AddForce(transform.forward * 200 *size);
        }

        //shrinking
        if(transform.localScale.x >.01){
            transform.localScale  += new Vector3(0.01F, .01f, .01f) * shrinkingScale * Time.deltaTime;
        }
        if (input.magnitude <= 0){
            animator.SetBool("moving", false);
            shrinkingScale = -1;
            return;
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

    IEnumerator DeathScene(){
        int i = 0;
        while (i < 1){
            animator.SetBool("moving", false);
            animator.SetBool("dead", true);
            i++;
            yield return new WaitForSeconds(3f);
        }
        SceneManager.LoadScene("Game_Over");
        yield return null;
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Enemy")){
            animator.SetBool("moving", false);
            animator.SetBool("colliding", true);

            // if you're bigger and blue (enemy color)
            if (other.transform.localScale.magnitude <= transform.localScale.magnitude && curr_color_color == 'b'){
                // increasing size by half of absorbed object's size
                size += other.transform.localScale.magnitude / transform.localScale.magnitude;
                Vector3 newSize = new Vector3(size, size, size);
                transform.localScale = newSize;

                // destroying the object we collected
                Destroy(other.gameObject);        
            } else {
                //maybe we can add some bullet like objects spawning to indicate that losing health
                thisCollisionCrt = StartCoroutine(GetHurt(other.transform.localScale.magnitude, size));                
            }
        }
    }

    IEnumerator GetHurt(float otherMag, float this_size){
        while (true){
            this_size -= 0.1f;
            Vector3 newSize = new Vector3(this_size, this_size, this_size);
            transform.localScale = newSize;
            yield return new WaitForSeconds(3f);
        }
    }

    private void OnCollisionExit(Collision other) {
        if (other.gameObject.CompareTag("Enemy")){
            StopCoroutine(thisCollisionCrt);
            animator.SetBool("colliding", false);
        }
    }
}
