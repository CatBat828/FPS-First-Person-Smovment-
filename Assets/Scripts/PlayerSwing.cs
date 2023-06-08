using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwing : MonoBehaviour
{
    private LineRenderer GunlineRenderer;
    public Vector3 SwingPoint;
    public LayerMask whatIsGrappleable;
    public Transform GunTip,
        camera,
        player;
    private float MaxDistance = 100f;
    public bool Swining = false;
    public float SwiningDistance = 0f;

    void Awake() //get linerender
    {
        GunlineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (!Swining)
            return; //if arn't swing stop the swing
		float dist = Math.Min(Vector3.Distance(player.position,SwingPoint),SwiningDistance);
		Vector3 dir = (player.position-SwingPoint).normalized;
		player.GetComponent<CharacterController>().Move(dist * dir + SwingPoint - player.position);//moving the player in a swining motion
    }

    //Called after Update
    void LateUpdate()
    {
        DrawRope();
    }

    /// <summary>
    /// Call whenever we want to start a swing
    /// </summary>
    public void StartSwing()
    {
        Debug.Log("hello");
        Swining = false;
        RaycastHit hit;
        if (
            Physics.Raycast(
                camera.position,
                camera.forward,
                out hit,
                MaxDistance,
                whatIsGrappleable
            )
        )
        {
            SwingPoint = hit.point;

            float distanceFromPoint = Vector3.Distance(player.position, SwingPoint);
            if (distanceFromPoint > MaxDistance)
                return;
			Swining = true;
            SwiningDistance = distanceFromPoint;
            GunlineRenderer.positionCount = 2;
            currentGrapplePosition = GunTip.position;
        }
    }

    /// <summary>
    /// Call whenever we want to stop a swing
    /// </summary>
    public void EndSwing()
    {
		Swining = false;
        GunlineRenderer.positionCount = 0;
    }

    private Vector3 currentGrapplePosition;

    void DrawRope()
    {
        currentGrapplePosition = Vector3.Lerp(
            currentGrapplePosition,
            SwingPoint,
            Time.deltaTime * 8f
        );

        GunlineRenderer.SetPosition(0, GunTip.position);
        GunlineRenderer.SetPosition(1, currentGrapplePosition);
    }
}
