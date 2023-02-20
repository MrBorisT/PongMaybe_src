using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private float forceMultiplier = 5f;
    bool IsPlaying;
    public void StopControl()
    {
        IsPlaying = false;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        IsPlaying = true;
    }

    void Update()
    {
        if (!IsPlaying)
            return;
        Vector2 scrollForce = new Vector2(0, Input.mouseScrollDelta.y * forceMultiplier);
        if (shouldChangeDirection(rb.velocity.y, scrollForce.y))
        {
            rb.velocity = Vector2.zero;
        }
        rb.AddForce(scrollForce);
    }

    bool shouldChangeDirection(float velocity, float scrollForce)
    {
        if (velocity > 0 && scrollForce < 0
            || velocity < 0 && scrollForce > 0)
        {
            return true;
        }
        return false;
    }

}
