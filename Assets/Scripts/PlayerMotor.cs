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

    public Rigidbody rb;

    public MovementState state;

    public enum MovementState
    {
        freeze,
    }

    public bool freeze;

    public bool activeGrapple;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;

        if (freeze)
        {
            playerVelocity = Vector3.zero;
            speed = 0;
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
        playerVelocity.y += gravity * Time.deltaTime;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }
        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void Crouch()
    {
        offset = transform.localScale;
        originalYScale = offset.y;
        offset.y = 0.6f * offset.y;
        transform.localScale = offset;
    }

    public void Uncrouch()
    {
        offset = transform.localScale;
        offset.y = originalYScale;
        transform.localScale = offset;
    }

    public void Jump()
    {
        playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
    }

    public void JumpToPosition(Vector3 targetPosition, float trajectoryHeight)
    {
        activeGrapple = true;

        velocityToSet = CalculateJumpVelocity(transform.position, targetPosition, trajectoryHeight);
        Invoke(nameof(SetVelocity), 0.1f);
    }

    private Vector3 velocityToSet;

    private void SetVelocity()
    {
        playerVelocity = velocityToSet;
    }

    public Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        float gravity = Physics.gravity.y;
        float displacementY = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity)
            + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));

        return velocityXZ + velocityY;
    }
}

