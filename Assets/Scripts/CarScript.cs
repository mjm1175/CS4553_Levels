using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarScript : MonoBehaviour
{
    private NavMeshAgent nma;

    public Transform pointA;
    public Transform pointB;

    private bool path = false; //false = pointb dest, true = pointa dest
    // Start is called before the first frame update
    void Start()
    {
        nma = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!path){
            if (!nma.pathPending)
            {
                if (nma.remainingDistance <= nma.stoppingDistance)
                {
                    if (!nma.hasPath || nma.velocity.sqrMagnitude == 0f)
                    {
                        this.nma.destination = pointA.position;
                        path = true;                    
                    }
                }              
            } else {
                this.nma.destination = pointB.position;
            }
        } else {
            if (!nma.pathPending)
            {
                if (nma.remainingDistance <= nma.stoppingDistance)
                {
                    if (!nma.hasPath || nma.velocity.sqrMagnitude == 0f)
                    {
                        this.nma.destination = pointB.position;
                        path = false;
                    }
                }
            } else {
                this.nma.destination = pointA.position;
            }
        }
    }
}
