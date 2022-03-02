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
            Debug.Log(distance);
            if(distance <= detectionDistance){
                nma.destination = player.transform.position;
            }
        }
    }


}
