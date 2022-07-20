using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyX : MonoBehaviour
{
    public float speed;

    private Rigidbody enemyRb;
    private GameObject playerGoal;
    

    // Start is called before the first frame update
    void Start()
    {
        playerGoal = GameObject.Find("Player");
        enemyRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Set enemy direction towards player goal and move there
        Vector3 lookDirection = (playerGoal.transform.position - transform.position).normalized;
        enemyRb.AddForce(lookDirection * (speed * Time.deltaTime));

    }

    private void OnCollisionEnter(Collision other)
    {
        // If enemy collides with either goal, destroy it
        //if (other.gameObject.name is "Enemy Goal" or "Player Goal")
        if (other.gameObject.CompareTag("Goal"))
        {
            Destroy(gameObject);
        }
    }

}
