﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{
	private Rigidbody2D body;
	private Vector2 jumpVelocity = new Vector2(0, 300);
	private Vector3 groundedVector = new Vector3(0, -0.7f, 0);
	private Vector2 velocity = new Vector2(0,0);

	private Transform startTransform;

	// Use this for initialization
	void Start () 
	{
		startTransform = transform;
		body = this.gameObject.GetComponent<Rigidbody2D>();
		body.velocity = velocity;
	}

	// Update is called once per frame
	void Update () 
	{
		//TODO: This can probably be implemented better from an OOP perspective
		if (Input.GetKeyDown(KeyCode.Escape) && isGrounded())
		{
			if(GameController.Instance.gameState.GetState() == GameState.State.Playing)
				GameController.Instance.SetPaused();
			else if (GameController.Instance.gameState.GetState() == GameState.State.Paused)
				GameController.Instance.SetPlaying();
		}

		if (GameController.Instance.gameState.GetState() != GameState.State.Playing)
			return;
		
		Debug.DrawLine(transform.position, transform.position + groundedVector, Color.yellow);

		// Jump
		if (Input.GetKeyDown(KeyCode.Space) && isGrounded())
		{
			body.AddForce(jumpVelocity);
		}

		// Move forward at a constant speed
		body.velocity = new Vector2(velocity.x,  body.velocity.y);

		// Respawn if fallen
		if (transform.position.y < -7)
		{
			GameController.Instance.RestartLevel();
		}
	}

	public bool isGrounded()
	{
		int blockedMask = (1 << LayerMask.NameToLayer("Impassable"));
		return Physics2D.Linecast(transform.position, transform.position + groundedVector, blockedMask);
	}
}
