using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private Playerinput playerInput;
    private Playerinput.OnFootActions onFoot;

    private PlayerMotor motor;
    private PlayerLook look;
    private PlayerGrappling grapple;
	private PlayerSwing swing;

    // Start is called before the first frame update
    void Awake()
    {
        playerInput = new Playerinput();
        onFoot = playerInput.OnFoot;
        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();
        grapple = GetComponent<PlayerGrappling>();
		swing = GetComponent<PlayerSwing>();
        
        onFoot.Jump.performed += ctx => motor.Jump();

        onFoot.Crouch.performed += ctx => motor.Crouch();
        onFoot.Crouch.canceled += ctx => motor.Uncrouch();

        onFoot.Grappling.performed += ctx => grapple.StartGrapple();

		onFoot.Swing.performed += ctx => swing.StartSwing();
		onFoot.Swing.canceled += ctx => swing.EndSwing();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // add some text.
        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
    }
    private void LateUpdate()
    {
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }
    private void OnEnable()
    {
        // console.log("on enable");
        onFoot.Enable();
    }

    private void OnDisable()
    {
        onFoot.Disable();
    }
}