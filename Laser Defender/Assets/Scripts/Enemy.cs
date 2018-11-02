using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [Header("Enemy Stats")]
    [SerializeField] float health = 100f;
    [SerializeField] int scoreValue = 150;

    [Header("Shooting")]
    [SerializeField] float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] GameObject projectile;
    [SerializeField] float projectileSpeed = 10f;

    [Header("Sound Effects")]
    [SerializeField] GameObject deathVFX;
    [SerializeField] float durationOfExplosion = 1f;
    [SerializeField] AudioClip enemyLaserAudio;
    [SerializeField] AudioClip enemyDeathAudio;
    [SerializeField] [Range(0, 1)] float enemyLaserAudioVolume = 1f;
    [SerializeField] [Range(0, 1)] float enemyDeathAudioVolume = 1f;


    // Use this for initialization
    void Start ()
    {
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
	}
	
	// Update is called once per frame
	void Update ()
    {
        CountDownAndShoot();
	}

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if( shotCounter <= 0)
        {
            Fire();
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void Fire()
    {
        GameObject laser = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
        AudioSource.PlayClipAtPoint(enemyLaserAudio, projectile.transform.position, enemyLaserAudioVolume);
    }

    private void OnTriggerEnter2D(Collider2D other)                     //other : the things which are bumped into enemy.
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return;  }
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            FindObjectOfType<GameSession>().AddToScore(scoreValue);
            Destroy(gameObject);
            GameObject explosion = Instantiate(deathVFX, transform.position, Quaternion.identity);
            Destroy(explosion, durationOfExplosion);
            AudioSource.PlayClipAtPoint(enemyDeathAudio, Camera.main.transform.position, enemyDeathAudioVolume);
        }
    }
}
