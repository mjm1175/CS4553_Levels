using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent nma;
    private Animator animator;
    private GameObject player;
    float detectionDistance;
    bool startedMoving = false;

    void Start()
    {
        detectionDistance = 10;
        nma = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(LookForPlayer());
        
    }

    IEnumerator LookForPlayer()
    {
        while(true)
        {
            yield return new WaitForSeconds(.5f);
            float distance = Vector3.Distance (player.transform.position, nma.transform.position);
            //Debug.Log(distance);
            if(distance <= detectionDistance){
                if (!startedMoving) animator.SetBool("moving", true);
                startedMoving = true;
                nma.destination = player.transform.position;
            } else {
                startedMoving = false;
            }
        }
    }
    
    public void Die(){
        StopAllCoroutines();
        animator.SetBool("dead", true);
        Destroy(gameObject, 3f);
    }



    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Player")){
            animator.SetBool("moving", false);
            animator.SetBool("colliding", true);
        }
    }

    private void OnCollisionExit(Collision other) {
        if (other.gameObject.CompareTag("Player")){
            animator.SetBool("colliding", false);
            animator.SetBool("moving", true);
        }
    }
}
