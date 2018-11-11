using System.Collections;
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
	public float hDirMult = 1f;
	public float vDirMult = 1f;
	public bool useInertia;

    [Header("Change Player Model")]
    public Transform model;
    public PlantHolding plantHolding;

	public Vector3 spawnPosition;
	public Vector3 offsetOnRespawn = new Vector3(0f, 10f, 0f);

    Rigidbody rigid;

	// Use this for initialization
	void Start ()
	{
		rigid = GetComponent<Rigidbody>();
	}

    public Vector3 ResetPosition()
    {
        return transform.position = spawnPosition + offsetOnRespawn;
    }

	void Movement ()
	{
		float hInputValue = Input.GetAxis(hInput);
		float vInputValue = Input.GetAxis(vInput);
		Vector3 movement;
		if (useInertia)
		{
			movement = new Vector3(
			hInputValue * hDirMult * speedMult * acceleration,
			rigid.velocity.y,
			vInputValue * vDirMult * speedMult * acceleration);

			Vector3 deltaV = movement - rigid.velocity;
			Vector3 accel = deltaV / Time.deltaTime;

			if (accel.sqrMagnitude > moveSpeed * moveSpeed)
				accel = accel.normalized * acceleration;

			rigid.AddForce(accel * Time.fixedDeltaTime, ForceMode.Acceleration);
		}
		else
		{
			movement = new Vector3(
			hInputValue * hDirMult * speedMult * moveSpeed,
			rigid.velocity.y,
			vInputValue * vDirMult * speedMult * moveSpeed);

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
