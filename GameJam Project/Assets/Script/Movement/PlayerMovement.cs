using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public Transform feet;
    public float gravityModifier;
    public float horizontalModifier;
    public float verticalModifier;
    public float climbingModifier;
    public float jumpMultipler;
    public float methodTime;
    public float lerpTime;
    public float lineBuffer;

    private Rigidbody2D rb2D;
    private LayerMask groundLayer;
    private LayerMask wallLayer; 

    private int direction;
    private int storedDirection;
    private int flipArms;
    private int launchLeg;

    private Vector2 velocity;

    private float xSizeHalf;
    private float ySizeHalf;
    private float horizontal;

    private bool grounded; // Determines if the player is grounded.
    private bool wall; // States if the player is next to a wall

	// Use this for initialization
	void Start () {
        xSizeHalf = transform.localScale.x / 2;
        ySizeHalf = transform.localScale.y / 2;
        horizontal = 0;
        direction = 1;
        flipArms = 2;
        launchLeg = 1;

        velocity = Vector2.up;

        grounded = true;
        wall = false;

        rb2D = GetComponent<Rigidbody2D>();
        groundLayer = LayerMask.GetMask("Ground");
        wallLayer = LayerMask.GetMask("Wall");

        if (feet == null)
            Debug.LogWarning("There are no feet");
	}

    private void Update()
    {
        isGrounded();

        if (grounded || wall)
        {
            storedDirection = direction;
            XMovement();
            YMovement();
        }
        else
        { 
            Flip();

            if (storedDirection == -2 && !IsInvoking("StartGravity"))
                Invoke("StartGravity", methodTime);
            else if (!IsInvoking("StartGravity"))
                rb2D.gravityScale = gravityModifier;

            Propel();

            if(storedDirection != -2)
                FallDown();
        }

        nearWall();
    }

    //FIXME: The jump doesnt have a middle ground
    private void FallDown()
    {

        if (horizontal == 0)
        {
            velocity = new Vector2(0.0f, rb2D.velocity.y);
            velocity = Vector2.Lerp(velocity, Vector2.zero, lerpTime);
        }
        else
        {
            velocity = new Vector2(horizontalModifier * direction, rb2D.velocity.y);
            velocity = Vector2.Lerp(velocity, Vector2.zero, lerpTime);
        }

        rb2D.position += velocity * jumpMultipler * Time.deltaTime;
    }

    // Moves the player in the x direction
    private void XMovement()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (horizontal == 1f)
            direction = 1;
        else if (horizontal == -1f)
            direction = -1;

        Vector2 horizontalDisplacement = new Vector2(horizontal * horizontalModifier, 0.0f);

        transform.Translate(horizontalDisplacement);
        //rb2D.position += horizontalDisplacement * Time.fixedDeltaTime;
    }

    // Moves the player in the y direction
    private void YMovement()
    {
        if (Input.GetKey(KeyCode.W) && wall)
        {
            rb2D.gravityScale = 0.0f;
            rb2D.velocity = Vector2.zero;
            Climb();
            return;
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            Jump();
        }
    }

    private void Flip()
    {
        //Debug.Log("Flip");
        if (direction == 1 && flipArms > 0 && Input.GetKeyDown(KeyCode.A))
        {
            ChangeDirection();
        }
        else if (direction == -1 && flipArms > 0 && Input.GetKeyDown(KeyCode.D))
        {
            ChangeDirection();
        }
    }

    private void ChangeDirection()
    {
        storedDirection = -2;
        direction *= -1;
        --flipArms;
        velocity = Vector2.zero;
        rb2D.velocity = Vector2.zero;

        rb2D.gravityScale = 0;
    }

    private void Propel()
    {
        float horizontalPropel = Input.GetAxisRaw("Horizontal");
        float verticalPropel = Input.GetAxis("Vertical");

        Vector2 propelVelocity = Vector2.zero;

        if(Input.GetKeyDown(KeyCode.Space) && launchLeg == 1)
        {
            --launchLeg;
            //Debug.Log("Propel");
            if (direction == horizontal)
            {
                propelVelocity.x = horizontalPropel * 750;
            }
            propelVelocity.y = verticalPropel * 750;

            rb2D.velocity = Vector2.zero;

            rb2D.AddForceAtPosition(propelVelocity, feet.transform.position);
        }
    }

    private void StartGravity()
    {
        rb2D.gravityScale = gravityModifier;
    }

    private void Climb()
    {
        float vertical = Input.GetAxisRaw("Vertical");

        Debug.Log("Begin Climbing"); 

        Vector2 climbing = new Vector2(0.0f, vertical * climbingModifier);

        rb2D.position += climbing * Time.deltaTime;
    }

    private void Jump()
    {
        //Debug.Log("Jump");
        rb2D.velocity = Vector2.zero;
        Vector2 verticalDisplacement = new Vector2(0.0f, verticalModifier);
        rb2D.AddForceAtPosition(verticalDisplacement * Time.fixedDeltaTime, feet.position, ForceMode2D.Impulse);
    }

    // Checks if the player is on the ground
    private void isGrounded()
    {
        Vector2 overLapA = new Vector2(transform.position.x - xSizeHalf, transform.position.y - ySizeHalf);
        Vector2 overLapB = new Vector2(transform.position.x + xSizeHalf, transform.position.y - ySizeHalf);

        grounded = Physics2D.OverlapArea(overLapA, overLapB, groundLayer);

    }

    private void nearWall()
    {
        Vector2 overLapA = new Vector2(transform.position.x + (xSizeHalf * direction), transform.position.y - ySizeHalf);
        Vector2 overLapB = new Vector2(transform.position.x + (xSizeHalf * direction), transform.position.y + ySizeHalf);

        wall = Physics2D.OverlapArea(overLapA, overLapB, wallLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(new Vector2((transform.position.x - xSizeHalf), (transform.position.y)),
            new Vector2(0.01f, 1f));
    }
}
