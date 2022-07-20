using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    private Rigidbody playerRb;
    public float speed = 500;
    private GameObject focalPoint;

    public bool hasPowerup;
    public GameObject powerupIndicator;
    public int powerUpDuration = 5;

    public KeyCode turboKey;
    public ParticleSystem turboParticleSystem;

    private bool canUseTurbo;
    public int multilplerSpeedTurbo = 2;
    public float stopTurboSpeed = 2.0f;
    
    public float normalStrength = 10; // how hard to hit enemy without powerup
    public float powerupStrength = 25; // how hard to hit enemy with powerup

    private void Awake()
    {
        canUseTurbo = true;
    }

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    void Update()
    {
        // Add force to player in direction of the focal point (and camera)
        float verticalInput = Input.GetAxis("Vertical");
        
        if (Input.GetKeyDown(turboKey) && canUseTurbo)
        {
            turboSpeed();
        }
        playerRb.AddForce(focalPoint.transform.forward * (verticalInput * speed * Time.deltaTime)); 

        // Set powerup indicator position to beneath player
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.6f, 0);

    }

    private void turboSpeed()
    {
        canUseTurbo = false;
        speed *= multilplerSpeedTurbo;
        turboParticleSystem.Play();
        Invoke("turboSpeedStop", stopTurboSpeed);
    }

    private void turboSpeedStop()
    {
        speed /= multilplerSpeedTurbo;
        turboParticleSystem.Stop();
        canUseTurbo = true;
    }

    // If Player collides with powerup, activate powerup
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Powerup"))
        {
            Destroy(other.gameObject);
            hasPowerup = true;
            powerupIndicator.SetActive(true);
            StartCoroutine(PowerupCooldown());
        }
    }

    // Coroutine to count down powerup duration
    IEnumerator PowerupCooldown()
    {
        yield return new WaitForSeconds(powerUpDuration);
        hasPowerup = false;
        powerupIndicator.SetActive(false);
    }

    // If Player collides with enemy
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Rigidbody enemyRigidbody = other.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = other.gameObject.transform.position - transform.position;
            
            // if have powerup hit enemy with powerup force
            // if no powerup, hit enemy with normal strength 
            
            float strength = hasPowerup ? powerupStrength : normalStrength;
            enemyRigidbody.AddForce(awayFromPlayer * strength, ForceMode.Impulse);
        }
    }



}
