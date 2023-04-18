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

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;
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
}

public class Crouch : MonoBehaviour
{

}
