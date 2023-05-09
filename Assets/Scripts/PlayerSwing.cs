using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwing : MonoBehaviour
{
	public float maxSwingDistance = 20;
	public LayerMask whatIsSwingable;
	public bool swinging = false;
	public Vector3 swingPoint;
	public PlayerMotor motor;

	public float swingExtension = 3.0f;
	public float grappleMax = 5.0f;
	public float grappleMin = 0.5f;
	public float grappleSpeed = 5.0f;
	public float freeEnergy = 1.05f;
	public float motorPercent = 0.05f;
	// Start is called before the first frame update
	public void StartSwing()
	{
		Debug.Log("swing");
		Transform cam = gameObject.transform.GetChild(0);
		Debug.Log(gameObject);
		Debug.Log(cam.gameObject);
		Debug.Log(cam.position);
		RaycastHit hit;
		if (Physics.Raycast(cam.position, cam.forward, out hit, maxSwingDistance, whatIsSwingable))
		{
			swingPoint = hit.point;
			swinging = true;
		}
		if (motor == null)
		{
			motor = gameObject.GetComponent<PlayerMotor>();
		}
	}

	public bool is_swinging()
	{
		return swinging;
	}

	private float remap(float val, float min, float max) // 0-1 -> min - max
	{
		return (1 - 1 / val /*0-1*/) * (max - min) + min /*standard 0-1 -> a-b remap*/ ;
	}

	public void ExecuteSwing()
	{
		Debug.Log("Executing");
		Vector3 delta = swingPoint - gameObject.transform.position;
		float mag = delta.sqrMagnitude;
		Debug.Log(mag.ToString());
		Vector3 dir = delta.normalized;
		Vector3 force = dir * remap(mag, grappleMin, grappleMax) * grappleSpeed;
		if (mag > Math.Pow(swingExtension * maxSwingDistance, 2)) { EndSwing(); }
		Vector3 vel = motor.GetVelocity();
		motor.AddForce(force * Time.fixedDeltaTime); // fixed update function
		motor.SetVelocity(
			Math.Min
				(motor.GetVelocity().magnitude,
				(vel * freeEnergy).magnitude)
			*
				(motor.GetVelocity() * motorPercent +
				force * (1 - motorPercent)).normalized);
	}

	// Update is called once per frame
	public void EndSwing()
	{
		Debug.Log("end");
		swinging = false;
	}
}
