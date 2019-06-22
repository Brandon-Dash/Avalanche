using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterController : MonoBehaviour {

    public GameController gameController;
    public GameObject player;
    public float speed;
    public float gameOverFadeTime;

    private bool gameOver;
    private float gameOverTimer;

    private void Start()
    {
        gameOver = false;
        gameOverTimer = 0.0f;
    }

    private void Update()
    {
        if (player == null)
        {
            gameOver = true;
        }

        if (gameOver)
        {
            gameOverTimer += Time.deltaTime;

            if (gameOverTimer >= gameOverFadeTime)
            {
                Destroy(gameObject);
            }
        }
    }

    private void FixedUpdate()
    {
        if (gameController.timeRate != 1.0f)
        {
            transform.Translate(Vector3.up * speed * gameController.difficulty * (gameController.slowdownWaterRatePenalty + 1.0f) * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.up * speed * gameController.difficulty * Time.deltaTime);
        }
    }
}
