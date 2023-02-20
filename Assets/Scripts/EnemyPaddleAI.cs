using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPaddleAI : MonoBehaviour
{
    private Ball ball;
    [SerializeField] private float precision = 1f;
    [SerializeField] private float maxPrecision = 10f;
    [SerializeField] private float minDistanceToSweep = 10f;
    [SerializeField] private Transform topLimit;
    [SerializeField] private Transform bottomLimit;
    private int difficulty = 0;

    [Header("Escalation with each win")]
    [SerializeField] float EscalationPrecision = 0.2f;

    private void Awake()
    {
        ball = FindObjectOfType<Ball>();
    }

    private void Start()
    {
        int gmDifficulty = FindObjectOfType<GameManager>().GetDifficulty();
        SetDifficulty(gmDifficulty);
    }

    void Update()
    {
        Move();
    }

    public void SetDifficulty(int difficulty)
    {
        this.difficulty = difficulty;
        precision += EscalationPrecision * difficulty;
        if (precision > maxPrecision)
        {
            precision = maxPrecision;
        }
    }

    private void Move()
    {
        Vector3 newPosition = new Vector3(transform.position.x, ball.transform.position.y, transform.position.z);
        if (OutsideLimits(transform.position))
            return;
        transform.position = Vector3.Lerp(transform.position, newPosition, precision * Time.deltaTime);

        if (difficulty > 10)
        {
            TryToSweep();
        }
    }

    private bool OutsideLimits(Vector3 newPosition)
    {
        return newPosition.y > topLimit.position.y || newPosition.y < bottomLimit.position.y;
    }

    private void TryToSweep()
    {
        if (ball.transform.position.x - transform.position.x < minDistanceToSweep)
        {

        }
    }
}
