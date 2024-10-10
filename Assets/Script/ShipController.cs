using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    private Rigidbody rb;
    public float throttle;
    public float throttleincrement;
    public float maxthrottle;

    public float shipspeed;
    public float shipturn;

    public float brakeincrement;
    public float brakepower;

    public float thrust;

    public float AD;
    public float WS;
    public float responsive;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        shipspeed = rb.velocity.magnitude * 3.6f;

        GetInput();
        ApplyForce();
    }

    private void ApplyForce()
    {
        rb.AddForce(transform.forward * thrust * throttle, ForceMode.Acceleration);
        rb.AddTorque(transform.up * shipturn * responsive);
    }

    private void GetInput()
    {
        shipturn = Input.GetAxis("Horizontal") * AD * Time.deltaTime;

        throttle = Input.GetAxis("Vertical") * WS * Time.deltaTime;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            throttle += throttleincrement * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            throttle -= throttleincrement * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            throttle = throttle - (brakeincrement * Time.deltaTime * brakepower);
        }

        throttle = Mathf.Clamp(throttle, 0f, maxthrottle);
    }
}
