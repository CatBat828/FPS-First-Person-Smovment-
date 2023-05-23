using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerGrappling : MonoBehaviour
{
    private PlayerMotor motor;

    public Transform cam;
    public Transform gunTip;
    public LayerMask whatIsGrappleable;
    public LineRenderer lr;

    public float maxGrappleDistance = 100f;
    public float grappleDelayTime = 0.25f;
    public float overshootYAxis;
    public float speed = 5f;

    private Vector3 grapplePoint;

    public float grapplingCd = 1f;
    private float grapplingCdTimer;

    private bool grappling;
    // Start is called before the first frame update
    void Start()
    {
        motor = GetComponent<PlayerMotor>(); 
    }

    // Update is called once per frame
    public void Update()
    {
        if (grapplingCdTimer > 0)
            grapplingCdTimer -= Time.deltaTime;
    }

    private void LateUpdate()
    {
        if(grappling)
            lr.SetPosition(0, gunTip.position);
    }

    public void StartGrapple()
    {
        if (grapplingCdTimer > 0) return;

        grappling = true;

        // motor.freeze = true;

        RaycastHit hit;
        if(Physics.Raycast(cam.position, cam.forward, out hit, maxGrappleDistance, whatIsGrappleable))
        {
            grapplePoint = hit.point;

            Invoke(nameof(ExecuteGrapple), grappleDelayTime);
        }
        else
        {
            grapplePoint = cam.position + cam.forward * maxGrappleDistance;

            Invoke(nameof(StopGrapple), grappleDelayTime);
        }

        lr.enabled = true;
        lr.SetPosition(1, grapplePoint);
    }
    
    public void ExecuteGrapple()
    {
        motor.freeze = false;

        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);

        float grapplePointRelativeYPos = grapplePoint.y - lowestPoint.y;
        float highestPointOnArk = grapplePointRelativeYPos + overshootYAxis;

        if (grapplePointRelativeYPos < 0) highestPointOnArk = overshootYAxis;

        motor.JumpToPosition(grapplePoint, highestPointOnArk);

        Invoke(nameof(StopGrapple), 1f);
    }

    public void StopGrapple()
    {
        Debug.Log("hello");

        motor.freeze = false;

        grappling = false;

        grapplingCdTimer = grapplingCd;

        lr.enabled = false;

        speed = 0;
    } 
}