using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class player : MonoBehaviour {

    [Header("Player")]
    /*
    [SerializeField] float moveSpeed = 100f;
    [SerializeField] float xPadding = 0.5f;
    [SerializeField] float yBottomPadding = 0.5f;
    [SerializeField] float yTopPadding = 5f;
    */
    [SerializeField] int health = 200;

    [Header("Projectile")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileFiringPeriod = 0.1f;
    Coroutine firingCoroutine;

    [Header("Audio")]
    [SerializeField] AudioClip playerLaserAudio;
    [SerializeField] AudioClip playerDeathAudio;
    [SerializeField] [Range(0, 1)] float playerShootAudioVolume = 0.75f;
    [SerializeField] [Range(0, 1)] float playerDeathAudioVolume = 0.5f;

    float xMin;
    float xMax;
    float yMin;
    float yMax;


    // Use this for initialization
   
    void Start ()
    {
        //SetUpMoveBoundaries();
        NewFire();
    }
    
	
	// Update is called once per frame
	void Update ()
    {
        //Fire();
        MovePlayer();
        //Move();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if(!damageDealer) { return; }
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            Die();
        }
    }

    public int GetHealth()
    {
        return Mathf.Max(health, 0);
    }

    private void Die()
    {
        Destroy(gameObject);
        AudioSource.PlayClipAtPoint(playerDeathAudio, Camera.main.transform.position, playerDeathAudioVolume);
        FindObjectOfType<Level>().LoadGameOver();
    }

    /*
    private void Fire()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            firingCoroutine = StartCoroutine(FireContinuously());
        }
        if(Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }
    }
    */


    private void NewFire()
    {
        StartCoroutine(FireContinuously());
    }

    IEnumerator FireContinuously()
    {
        while (true)
        {
            GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            AudioSource.PlayClipAtPoint(playerLaserAudio, laserPrefab.transform.position, playerShootAudioVolume);
            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    }

    /*
    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);

        transform.position = new Vector2(newXPos ,newYPos);
    }
    */

    public void MovePlayer()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            touchPosition.z = 0f;
            transform.position = touchPosition;
        }
    }

    /*
    private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + xPadding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - xPadding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + yBottomPadding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - yTopPadding;
    }
    */
}
