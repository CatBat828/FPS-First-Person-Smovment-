using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwing : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public Vector3 grapplePoint;
    public LayerMask whatIsGrappleable;
    public Transform gunTip,
        camera,
        player;
    private float maxDistance = 100f;
    public bool grappling = false;
    public float grapplingDistance = 0f;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (!grappling)
            return;
		float dist = Math.Min(Vector3.Distance(player.position,grapplePoint),grapplingDistance);
		Vector3 dir = (player.position-grapplePoint).normalized;
		player.GetComponent<CharacterController>().Move(dir*dist+grapplePoint-player.position);
    }

    //Called after Update
    void LateUpdate()
    {
        DrawRope();
    }

    /// <summary>
    /// Call whenever we want to start a grapple
    /// </summary>
    public void StartSwing()
    {
		grappling = false;
        RaycastHit hit;
        if (
            Physics.Raycast(
                camera.position,
                camera.forward,
                out hit,
                maxDistance,
                whatIsGrappleable
            )
        )
        {
            grapplePoint = hit.point;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);
            if (distanceFromPoint > maxDistance)
                return;
			grappling = true;
            grapplingDistance = distanceFromPoint;
            lineRenderer.positionCount = 2;
            currentGrapplePosition = gunTip.position;
        }
    }

    /// <summary>
    /// Call whenever we want to stop a grapple
    /// </summary>
    public void EndSwing()
    {
		grappling = false;
        lineRenderer.positionCount = 0;
    }

    private Vector3 currentGrapplePosition;

    void DrawRope()
    {
        currentGrapplePosition = Vector3.Lerp(
            currentGrapplePosition,
            grapplePoint,
            Time.deltaTime * 8f
        );

        lineRenderer.SetPosition(0, gunTip.position);
        lineRenderer.SetPosition(1, currentGrapplePosition);
    }
}
