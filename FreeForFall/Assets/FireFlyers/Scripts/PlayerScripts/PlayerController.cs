﻿using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float JumpCooldown;
	public float TimeSincePreviousJump;
	public float speed;
	public float maxSprintSpeed;
	private float sprintMultiplier;
	public float jumpforce;
	private bool airbone;
	private Rigidbody rigidBody;
	private Quaternion rotation;
	private Quaternion camera_rotation;
	public float rotateSpeed = 1f;
	private Rigidbody rb;
	public Transform playerCamera;
	private Quaternion startrotation;
	private Vector3 initial_Camera;
    public Transform spawn;
    public bool ground_powerup = true;

	// Use this for initialization
	void Start ()
	{
		TimeSincePreviousJump = 0f;
		this.sprintMultiplier = 1f;
		this.airbone = false;
		this.rigidBody = this.GetComponent<Rigidbody> ();
		rb = GetComponent<Rigidbody> ();
	}

	void OnCollisionEnter (Collision other)
	{
		if (other.gameObject.name == "ground")
		{
			this.airbone = false;
		}
	}

	void OnCollisionExit (Collision other)
	{
		if (other.gameObject.name == "ground")
		{
			this.airbone = true;
		}
	}

	private void jump ()
	{
		this.rigidBody.AddForce (new Vector3 (0f, jumpforce, 0f));
		Invoke ("jumpDown", 0.3f);
	}

	private void jumpDown ()
	{
		this.rigidBody.AddForce (new Vector3 (0f, -jumpforce * 1.5f, 0f));
	}

	private void doMovement ()
	{
		if ((Input.GetKeyDown (KeyCode.Joystick1Button0)
		    || Input.GetKeyDown (KeyCode.Space))
		    && !this.airbone
		    && JumpCooldown < TimeSincePreviousJump)
		{
			TimeSincePreviousJump = 0f;
			jump ();
		}
		#if UNITY_STANDALONE_WIN
		if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.Joystick1Button8))
		#endif
		#if UNITY_STANDALONE_LINUX
		if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.JoystickButton9))
		#endif
		{
			this.sprintMultiplier = this.maxSprintSpeed;
		}
		else
		{
			this.sprintMultiplier = 1f;
		}
		float h = Input.GetAxis ("Horizontal");
		float v = Input.GetAxis ("Vertical");
		if (h > 0 || h < 0 || v > 0 || v < 0)
		{
			float rotate = rotateSpeed;
			Vector3 direction = new Vector3 (h, 0, v);
			rotation = Quaternion.LookRotation (direction, Vector3.up);
			Quaternion cam = new Quaternion (0, playerCamera.rotation.y, 0, playerCamera.rotation.w);
			rb.rotation = Quaternion.RotateTowards (transform.rotation, cam * rotation, 800 * Time.deltaTime);
			Vector3 Moveto = new Vector3 (0, transform.position.y, 1);
			transform.position = Vector3.MoveTowards (transform.position, transform.position + transform.forward, 
				speed * sprintMultiplier * Time.deltaTime);
		}
        if (Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.Joystick1Button6))
        {
            if (spawn != null)
            transform.position = spawn.position;
        }
    }

    // Update is called once per frame

    void Update ()
	{
		TimeSincePreviousJump += Time.deltaTime;
		doMovement ();
	}
}
