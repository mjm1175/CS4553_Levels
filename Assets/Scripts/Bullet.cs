using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    void Start(){
        StartCoroutine(SelfDestruct());
    }
    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Enemy"))
        {
            //do something
            Enemy es = other.gameObject.GetComponent<Enemy>();
            es.Die();
            //Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }

}
