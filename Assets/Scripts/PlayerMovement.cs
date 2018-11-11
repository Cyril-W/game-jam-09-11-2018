﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[Header("Movement")]
	public float moveSpeed = 3f;
	public float acceleration = 2f;
	public float maxSqrVelocity = 5f;
	public string hInput;
	public string vInput;

	[Header("Movement Modifications")]
	public float speedMult = 1f;
	public float dirMultH = 1f;
	public float dirMultV = 1f;
	public bool useInertia;

    [Header("Change Player Model")]
    public Transform model;
    public PlantHolding plantHolding;

    Rigidbody rigid;

	// Use this for initialization
	void Start ()
	{
		rigid = GetComponent<Rigidbody>();
	}

	void Movement ()
	{
		float hInputValue = Input.GetAxis(hInput);
		float vInputValue = Input.GetAxis(vInput);
		Vector3 movement;
		if (useInertia)
		{
			movement = new Vector3(
			hInputValue * dirMultH * speedMult * acceleration,
			rigid.velocity.y,
			vInputValue * dirMultV * speedMult * acceleration);

			Vector3 deltaV = movement - rigid.velocity;
			Vector3 accel = deltaV / Time.deltaTime;

			if (accel.sqrMagnitude > moveSpeed * moveSpeed)
				accel = accel.normalized * acceleration;

			rigid.AddForce(accel * Time.fixedDeltaTime, ForceMode.Acceleration);
		}
		else
		{
			movement = new Vector3(
			hInputValue * dirMultH * speedMult * moveSpeed,
			rigid.velocity.y,
			vInputValue * dirMultV * speedMult * moveSpeed);

			rigid.velocity = movement;
		}

        if (movement.x != 0 || movement.z != 0)
        {
            model.localRotation = Quaternion.LookRotation(movement);
            plantHolding.SetIsWalking(true);
        } else
        {
            plantHolding.SetIsWalking(false);
        }
	}

	private void FixedUpdate ()
	{
		Movement();
	}
}
