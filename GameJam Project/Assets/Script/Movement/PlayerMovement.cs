using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float horizontalModifier;
    public float verticalModifier;
    public float lineBuffer;

    private Rigidbody2D rb2D;
    private RaycastHit2D hit2D;
    private Transform feet;
    private LayerMask groundLayer;
    private bool grounded; // Determines if the player is grounded.

	// Use this for initialization
	void Start () {
        grounded = true;

        rb2D = GetComponent<Rigidbody2D>();
        feet = GetComponentInChildren<Transform>();
        groundLayer = LayerMask.GetMask("Ground");

        if (feet == null)
            Debug.LogWarning("There are no feet");
	}

    private void Update()
    {
        grounded = isGrounded();
        Debug.Log(grounded);
    }

    // Update is called once per frame
    void FixedUpdate () {

        // Vector2 displacement =  new Vector2(XMovement(), 0.0f);

        XMovement();

       if (grounded == true)
            YMovement();
           // rb2D.AddForceAtPosition(YMovement() * verticalModifier * Time.deltaTime, feet.position, ForceMode2D.Impulse);

	}

    // Moves the player in the x direction
    // CONSIDER: The player seems to be jittering.
    // POSSIBLE SOLUTION: It could simply be because of my computer.
    private void XMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");

        Vector3 horizontalDisplacement = new Vector2(horizontal * horizontalModifier, 0.0f);

        //rb2D.position += horizontalDisplacement * Time.deltaTime;

        transform.Translate(horizontalDisplacement * Time.deltaTime);

        //return horizontal * horizontalModifier;
    }

    // Moves the player in the y direction
    private void YMovement()
    {
        float vertical = Input.GetAxisRaw("Vertical");

        Vector2 verticalDisplacement = new Vector2(0.0f, vertical * verticalModifier);

        rb2D.AddForceAtPosition(verticalDisplacement * Time.deltaTime, feet.position, ForceMode2D.Impulse);

        //return vertical * verticalModifier;
    }

    // Checks if the player is on the ground
    private bool isGrounded()
    {
        Vector2 lineStartBuffer = new Vector2(feet.position.x, feet.position.y - 1.5f);
        Vector2 lineEndBuffer = new Vector2(feet.position.x, feet.position.y - lineBuffer);

        hit2D = Physics2D.Linecast(feet.position, lineEndBuffer, groundLayer);

        // Debug.Log(hit2D.collider.name);
        if (hit2D == false)
            return false;

        if (hit2D.collider.CompareTag("Ground"))
            return true;
        else
            return false;
    }

    private void OnDrawGizmos()
    {
        Vector2 lineStartBuffer = new Vector2(feet.position.x, feet.position.y - 1.5f);
        Vector2 lineEndBuffer = new Vector2(feet.position.x, feet.position.y - lineBuffer);

        Gizmos.DrawLine(lineStartBuffer, lineEndBuffer);
    }
}
