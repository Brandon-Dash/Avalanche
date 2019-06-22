using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

    public PlayerController player;
    public float speedH;
    public float speedV;
    public float offsetV;
    public float maxOffsetH;
    public float minDistFromBoundary;
    public float gameOverTime;
    public float endGameMaxScrollSpeed;
    public float endGameSize;

    private float playerBoundary;
    private float offsetH;
    private float gameOverTimer;
    private bool endGameScrolling;
    private float endHeight;
    private Camera camera;

    private void Start()
    {
        playerBoundary = player.GetComponent<PlayerController>().boundary;
        camera = GetComponent<Camera>();
        camera.orthographicSize = 8.0f;
        gameOverTimer = 0.0f;
        endGameScrolling = false;
        endHeight = 0.0f;
    }

    private void LateUpdate()
    {
        if (player != null)
        {
            offsetH = player.transform.position.x - transform.position.x;

            if (Mathf.Abs(offsetH) > maxOffsetH)
            {
                transform.Translate(Vector3.right * speedH * Mathf.Sign(offsetH) * Time.deltaTime);
            }



            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -playerBoundary + minDistFromBoundary, playerBoundary - minDistFromBoundary),
                Mathf.Lerp(transform.position.y, player.transform.position.y + offsetV, speedV * Time.deltaTime), transform.position.z);

            endHeight = player.maxHeight;
        }
        else
        {
            gameOverTimer += Time.deltaTime;

            if (gameOverTimer >= gameOverTime && !endGameScrolling)
            {
                transform.position = new Vector3(0.0f, 0.0f, -10.0f);
                camera.orthographicSize = endGameSize;
                endGameScrolling = true;
            }
        }

        if (endGameScrolling)
        {
            if (transform.position.y < endHeight)
            {
                transform.position += Vector3.up * Mathf.Clamp(Mathf.Sqrt(endHeight - transform.position.y), 0.0f, endGameMaxScrollSpeed) * Time.deltaTime;
            }
        }
    }
}
