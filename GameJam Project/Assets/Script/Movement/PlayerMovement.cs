using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float jumpModifier;

    private Rigidbody2D rb2D;
    private Transform feet;
    private bool grounded; // Determines if the player is grounded.

	// Use this for initialization
	void Start () {
        grounded = true;

        rb2D = GetComponent<Rigidbody2D>();
        feet = GetComponentInChildren<Transform>();

        if (feet == null)
            Debug.LogWarning("There are no feet");
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if (Input.GetKeyDown(KeyCode.W))
            rb2D.AddForceAtPosition(Vector2.up * jumpModifier * Time.deltaTime, feet.position, ForceMode2D.Impulse);

	}

    // Moves the player in the x direction
    private void XMovement()
    {

    }

    // Moves the player in the y direction
    private void YMovement()
    {

    }

    private void OnDrawGizmosSelected()
    {
        Vector2 lineEndBuffer = new Vector2(feet.position.x, feet.position.y - 5);


        Gizmos.DrawLine(feet.position, lineEndBuffer);
    }
}
