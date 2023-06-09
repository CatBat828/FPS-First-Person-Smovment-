using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
	private CharacterController controller;
	private Vector3 playerVelocity;
	private bool isGrounded;
	public float speed = 5f;
	public float gravity = -9.8f;
	public float jumpHeight = 3f;
	// public CharacterController PlayerHeight;
	public CapsuleCollider playerCol;
	// public Transform player;
	public Vector3 offset;
	public float originalYScale;
	public float crouchHeight;
	public bool crouched;

	public Rigidbody rb;

	public MovementState state;
	public float airDrag = 0.01f;
	public float groundDrag = 0.02f;

	public enum MovementState
	{
		freeze,
	}

	public bool freeze;

	public bool activeGrapple;

	void FixedUpdate()//50 times a frame
    {
		if (crouched)//handle sliding
        {
			float oldy = playerVelocity.y;//store y speed
            if (isGrounded)
            {
                playerVelocity = playerVelocity.normalized*10.5f;//slide
            }
            else
            {
				playerVelocity = playerVelocity.normalized * 10.5f;
            }
			playerVelocity.y = oldy;//dont fly
        }
    }

	// Start is called before the first frame update
	void Start()
	{
		controller = GetComponent<CharacterController>();
	}

	// Update is called once per frame
	void Update()
	{
		isGrounded = controller.isGrounded;

		if (freeze)//dont move if we cant
		{
			playerVelocity = Vector3.zero;
			speed = 0;
		}
		else//move if we can
		{
			speed = 5;
		}

		Cursor.visible = false;//dont show mouse
		Cursor.lockState = CursorLockMode.Confined;
	}

	public void ProcessMove(Vector2 input)//process for movement, gravity, and friction
	{
		Vector3 moveDirection = Vector3.zero;
		moveDirection.x = input.x;//move driection input
		moveDirection.z = input.y;
		Vector3 temporal = transform.TransformDirection(moveDirection) * speed * Time.deltaTime;//calulating the movement
		controller.Move(temporal);//move unlaggy
		playerVelocity.y += gravity * Time.deltaTime;//gravity
		playerVelocity += temporal;//gain speed
		if (isGrounded && playerVelocity.y < 0)
		{
			playerVelocity.y = -2f;//dont fall through flor
		}
		playerVelocity *= 1 - (isGrounded ? groundDrag : airDrag);//slowing down
		controller.Move(playerVelocity * Time.deltaTime);//move
	}

	public void Crouch()
	{
		crouched = true;
		offset = transform.localScale;//dont mess with camer that much
		originalYScale = offset.y;
		offset.y = 0.6f * offset.y;
		transform.localScale = offset;//scale the player
	}

	public void Uncrouch()
	{
		crouched = false;
		Debug.Log("hello");
		offset = transform.localScale;//fix camera
		offset.y = originalYScale;
		transform.localScale = offset;//unscale the player
	}

	public void Jump()
	{
		if (!isGrounded)//don't jump mid air
			return;
		playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);//gravity big jump to make easier editing
	}

	public void JumpToPosition(Vector3 targetPosition, float trajectoryHeight)
	{
		activeGrapple = true;

		velocityToSet = CalculateJumpVelocity(transform.position, targetPosition, trajectoryHeight);
		Invoke(nameof(SetVelocity), 0.1f);//set volcity after 0.1s
	}

	public void AddForce(Vector3 velocity)
	{
		playerVelocity += velocity;//player speed
	}

	public Vector3 GetVelocity() //velocity pruvate
	{
		return playerVelocity;
	}

	public void SetVelocity(Vector3 velocity)
	{
		playerVelocity = velocity;
	}

	private Vector3 velocityToSet;

	private void SetVelocity() // old dont use
	{
		playerVelocity = velocityToSet;
	}

	public Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)//calcuation for jump vvelocity
	{
		float gravity = Physics.gravity.y;//get gravity
		float displacementY = endPoint.y - startPoint.y;//get distance
		Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);//get change

		Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);//do height
		Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity)//do not height
			+ Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));

		return velocityXZ + velocityY; // add together
	}
}

