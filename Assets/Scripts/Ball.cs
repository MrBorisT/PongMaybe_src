using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    [SerializeField] float padBounceMultiplier = 1f;
    [Header("Launch Data")]
    [SerializeField] float minMaxStartYForce = 10f;
    [SerializeField] float startSpeed = 10f;
    [SerializeField] float minimumXForce = 1f;
    [SerializeField] float minimumYForce = 1f; 

    [Header("Audio Clips")]
    [SerializeField] AudioClip bouncePad1;
    [SerializeField] AudioClip bouncePad2;
    [SerializeField] AudioClip bounceWall;
    [SerializeField] AudioClip failClip;

    [Header("Ready-Set-Go")]
    [SerializeField] AudioClip readyClip;
    [SerializeField] AudioClip setClip;
    [SerializeField] AudioClip goClip;
    [SerializeField] float readySetGoDelay = 0.8f;
    [SerializeField] float readySetGoVolume = 0.4f;

    [Header("Explosion")]
    [SerializeField] float explodeDelayTime = 1f;
    [SerializeField] AudioClip explosionClip;
    [SerializeField] float explodeVolume = 0.2f;
    private bool bounceSoundFlag;
    private RigidbodyConstraints2D initialConstraints2D;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        initialConstraints2D = rb.constraints;
    }
    void Start()
    {
        StartCoroutine(LaunchSequence());
        bounceSoundFlag = false;
        animator.enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Pad")
        {
            if (bounceSoundFlag)
            {
                PlayClip(bouncePad1);
            }
            else
            {
                PlayClip(bouncePad2);
            }
            float contactY = collision.gameObject.transform.position.y - transform.position.y;
            Vector2 YForce = new Vector2(0, -contactY * padBounceMultiplier);
            rb.AddRelativeForce(YForce, ForceMode2D.Impulse);
            
            bounceSoundFlag = !bounceSoundFlag;
        }
        else if (collision.gameObject.tag == "Wall")
        {
            PlayClip(bounceWall);
        }
    }

    private void OnBecameInvisible()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayClip(failClip);
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        Invoke("Explode", explodeDelayTime);
    }

    private void Explode()
    {
        PlayClip(explosionClip, explodeVolume);
        animator.enabled = true;
    }

    private void FixedUpdate()
    {
        if (Mathf.Abs(rb.velocity.x) < minimumXForce)
        {
            Vector2 minVelocity = new Vector2(minimumXForce * Mathf.Sign(rb.velocity.x), rb.velocity.y);
            rb.velocity = minVelocity;
        }
        if (Mathf.Abs(rb.velocity.y) < minimumYForce)
        {
            Vector2 minVelocity = new Vector2(rb.velocity.x, minimumYForce * Mathf.Sign(rb.velocity.y));
            rb.velocity = minVelocity;
        }
    }

    IEnumerator LaunchSequence()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        PlayClip(readyClip, readySetGoVolume);
        yield return new WaitForSeconds(readySetGoDelay);
        PlayClip(setClip, readySetGoVolume);
        yield return new WaitForSeconds(readySetGoDelay);
        PlayClip(goClip, readySetGoVolume);
        yield return new WaitForSeconds(readySetGoDelay / 2f);
        LaunchBall();
    }
    void LaunchBall()
    {
        rb.constraints = initialConstraints2D;
        float yForce = Random.Range(-minMaxStartYForce, minMaxStartYForce);
        Vector2 launchForce = new Vector2(-startSpeed, yForce);
        rb.AddForce(launchForce);
    }

    void PlayClip(AudioClip clip, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, volume);
    }
}
