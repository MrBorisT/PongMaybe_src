using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class FailArea : MonoBehaviour
{
    [SerializeField] float restartTime = 2f;
    [SerializeField] float shakeDuration = 0.8f;
    CameraShake cs;
    PlayerControl playerControl;
    GameManager gameManager;
    private void Awake()
    {
        cs = FindObjectOfType<CameraShake>();
        playerControl = FindObjectOfType<PlayerControl>();
        gameManager = FindObjectOfType<GameManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ball")
        {
            playerControl.StopControl();
            cs.SetShakeDuration(shakeDuration);
            cs.enabled = true;
            Invoke("StartRestartSequence", restartTime);
            if (tag == "Player")
            {
                gameManager.AddToEnemyScore();
            } else
            {
                gameManager.AddToPlayerScore();
            }
            
        }
    }

    void StartRestartSequence()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
