using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed;
    public float acceleration;
    public float jumpBuffer;
    public float jumpVelocity;
    public float jumpGravityResistance;
    public float slidingFriction;
    public float wallJumpHorizontalVelocity;
    public float wallJumpIgnoreInputDuration;
    public float boundary;

    private Rigidbody2D rb;
    private Transform trans;
    private float jumpBufferTimer;
    private float wallJumpInputIgnoreTimer;
    private bool onGround;
    private bool resistGravity;
    private int sliding;
    private float slidingBoxFallSpeed;
    private float horizontalInput;
    private float lastFrameVertical;
    private bool correctVertical;
    private float squishSpeed;

    public float maxHeight;

    public bool alive;

    private void Start()
    {
        alive = true;
        rb = GetComponent<Rigidbody2D>();
        trans = GetComponent<Transform>();
        onGround = true;
        sliding = 0;
        resistGravity = false;
        jumpBufferTimer = 1.0f;
        lastFrameVertical = 0.0f;
        correctVertical = false;
        wallJumpInputIgnoreTimer = 1.0f;
        squishSpeed = 0.0f;
        slidingBoxFallSpeed = 0.0f;
        maxHeight = 0.0f;
    }

    private void Update()
    {
        jumpBufferTimer += Time.deltaTime;
        wallJumpInputIgnoreTimer += Time.deltaTime;

        if (wallJumpInputIgnoreTimer >= wallJumpIgnoreInputDuration)
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
        }
        else
        {
            horizontalInput = 0.0f;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferTimer = 0.0f;
        }

        if (!alive)
        {
            trans.localScale += new Vector3(squishSpeed * 0.35f * Time.deltaTime, -squishSpeed * Time.deltaTime);
            if (trans.localScale.y <= 0.0f)
            {
                Destroy(gameObject);
            }
        }

        if (transform.position.y > maxHeight)
        {
            maxHeight = transform.position.y;
        }

        //Debug.Log(rb.velocity.y.ToString());
    }

    private void FixedUpdate()
    {
        if (alive)
        {
            if (wallJumpInputIgnoreTimer >= wallJumpIgnoreInputDuration)
            {
                rb.velocity = new Vector2(Mathf.MoveTowards(rb.velocity.x, horizontalInput * speed, acceleration * Time.fixedDeltaTime), rb.velocity.y);
            }

            if (sliding != 0)
            {
                if (correctVertical)
                {
                    rb.velocity = new Vector2(0.0f, lastFrameVertical + Physics2D.gravity.y * Time.fixedDeltaTime);
                    correctVertical = false;
                }
                else
                {
                    rb.velocity = new Vector2(0.0f, rb.velocity.y);
                }

                if (!resistGravity)
                {
                    rb.velocity = new Vector2(0.0f, Mathf.Lerp(rb.velocity.y, -slidingBoxFallSpeed, slidingFriction * Time.fixedDeltaTime));
                }
            }

            if (jumpBufferTimer <= jumpBuffer && onGround)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
                resistGravity = true;
                jumpBufferTimer = 1.0f;
            }
            else if (jumpBufferTimer <= jumpBuffer && sliding != 0)
            {
                if (sliding == 1)
                {
                    rb.velocity = new Vector2(wallJumpHorizontalVelocity, jumpVelocity);
                }
                else
                {
                    rb.velocity = new Vector2(-wallJumpHorizontalVelocity, jumpVelocity);
                }
                resistGravity = true;
                wallJumpInputIgnoreTimer = 0.0f;
                jumpBufferTimer = 1.0f;
            }


            if (resistGravity)
            {
                if (Input.GetButton("Jump") && rb.velocity.y > 0.0f)
                {
                    rb.velocity -= Vector2.up * Physics2D.gravity.y * jumpGravityResistance * Time.fixedDeltaTime;
                }
                else
                {
                    resistGravity = false;
                }
            }

            lastFrameVertical = rb.velocity.y;

            rb.position = new Vector2(Mathf.Clamp(rb.position.x, -boundary, boundary), rb.position.y);
        }
    }

    public void OnGround(bool state)
    {
        onGround = state;
    }

    public void OnWall(int side, float boxSpeed)
    {
        slidingBoxFallSpeed = boxSpeed;

        if (((horizontalInput < 0.0f && side == 1) || (horizontalInput > 0.0f && side == 2)) && wallJumpInputIgnoreTimer >= wallJumpIgnoreInputDuration && !onGround)
        {
            if (sliding == 0)
            {
                correctVertical = true;
            }
            sliding = side;
        }
        else if (horizontalInput != 0.0f || wallJumpInputIgnoreTimer < wallJumpIgnoreInputDuration || onGround)
        {
            sliding = 0;
        }
    }

    public void OffWall()
    {
        sliding = 0;
    }

    public void Squish(float boxSpeed)
    {
        if (onGround && alive)
        {
            alive = false;
            rb.velocity = Vector2.zero;
            squishSpeed = boxSpeed;
        }
    }

    public void hitWater()
    {
        alive = false;
        rb.velocity = -Vector2.up * 2.0f;
        squishSpeed = 1.0f;
    }
}
