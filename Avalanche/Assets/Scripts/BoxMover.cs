using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMover : MonoBehaviour {

    public float fallSpeed;
    public float modifiedSpeed;
    public float destroyTime;

    private Rigidbody2D rb;
    private GameController gameController;
    private GameObject player;
    private float correctPosition;
    private float destroyTimer;
    private bool toDestroy;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        player = GameObject.FindWithTag("Player");
        correctPosition = 0.0f;
        modifiedSpeed = fallSpeed;
        destroyTimer = 0.0f;
        toDestroy = false;
    }

    private void Update()
    {
        modifiedSpeed = fallSpeed * gameController.timeRate;

        if (player == null && fallSpeed != 0.0f)
        {
            toDestroy = true;
        }

        if (toDestroy)
        {
            destroyTimer += Time.deltaTime;

            if (destroyTimer >= destroyTime)
            {
                Destroy(gameObject);
            }
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position - Vector2.up * modifiedSpeed * Time.fixedDeltaTime);

        if (correctPosition != 0.0f)
        {
            rb.MovePosition(new Vector2(rb.position.x, correctPosition));
            correctPosition = 0.0f;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Ground")
        {
            float otherSpeed = other.gameObject.GetComponent<BoxMover>().fallSpeed;

            if (otherSpeed < fallSpeed)
            {
                correctPosition = other.gameObject.transform.position.y + (other.gameObject.transform.localScale.y + gameObject.transform.localScale.y) / 2.0f - otherSpeed * Time.fixedDeltaTime * gameController.GetComponent<GameController>().timeRate;
            }

            fallSpeed = otherSpeed;
        }
    }
}
