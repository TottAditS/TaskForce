using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneController : MonoBehaviour
{
    [Header("Plane Engine")]
    public float throttleIncrement = 10f;
    public float BrakeIncrement = 10f;
    public float BrakePower = 5f;
    public float maxThrottle = 100f;
    public float maxThrust = 2000f;
    public float responsiveness = 10f;

    [Header("PlaneForce")]
    public float AD = 100f;
    public float WS = 100f;
    public float QE = 100f;
    public float LiftForce = 10f;

    [Header("Plane Stats")]
    public float throttle;
    public float roll;
    public float pitch;
    public float yaw;
    public float speed;
    public float rotorspeed;
    public float rotation_multipier = 100;
    public float rotorspin_multipier = 100;

    [Header("Plane Components")]
    public GameObject rotor;
    public Transform leftFlapWing;
    public Transform rightFlapWing;
    public Transform BLwing;
    public Transform BRwing;
    public Transform rudder;
    public GameObject GroundCheck;

    [Header("Control Parameters")]
    public float flapAngleLimit = 20f;
    public float rudderAngleLimit = 30f;

    private Rigidbody rb;

    private float responsivenessModifier
    {
        get
        {
            return (rb.mass / 10f) * responsiveness;
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        speed = rb.velocity.magnitude * 3.6f;

        GetInput();
        GetSound();
        getAnimation();
    }
    private void getAnimation()
    {
        rotorspeed = speed * throttleIncrement * rotorspin_multipier;
        rotorspeed = Mathf.Repeat(rotorspeed, 720000);
        rotor.transform.Rotate(Vector3.up, rotorspeed * Time.deltaTime);

        AnimateFlapWings();
        AnimateRudder();
    }

    private void FixedUpdate()
    {
        ApplyForces();
        ApplyAerodynamicForces();
        ApplyGravityEffect();
    }
    private void GetSound()
    {
        if (!AudioManager.instance.IsPlaying("Engine-Start"))
        {
            if (Input.GetKey(KeyCode.LeftShift) && speed < 30)
            {
                //Debug.Log("start");
                AudioManager.instance.playS("Engine-Start");
            }
        }

        if (!AudioManager.instance.IsPlaying("Engine-Fly"))
        {
            if (speed > 31)
            {
                //Debug.Log("fly");
                AudioManager.instance.playS("Engine-Fly");
            }
        }

        //if (throttle < 10 && !Input.GetKey(KeyCode.LeftShift))
        //{
        //    AudioManager.instance.StopS();
        //}

    }
    private void GetInput()
    {

        roll = Input.GetAxis("Horizontal") * AD * Time.deltaTime;


        pitch = Input.GetAxis("Vertical") * WS * Time.deltaTime;


        yaw = Input.GetAxis("Yaw") * QE * Time.deltaTime;

        SetFlapWingRotation(pitch, roll);
        SetRudderRotation(yaw);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            throttle += throttleIncrement * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            throttle -= throttleIncrement * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            throttle = throttle - (BrakeIncrement * Time.deltaTime * BrakePower);
        }

        throttle = Mathf.Clamp(throttle, 0f, maxThrottle);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Ground") == true)
        {
            //Debug.Log("tanah");
            if (Input.GetKey(KeyCode.S))
            {
                //Debug.Log("ssssss");
                ApplyLift();
            }
        }
    }
    void ApplyLift()
    {
        rb.AddForce(Vector3.up * LiftForce * speed);
    }

    private void ApplyForces()
    {
        rb.AddForce(transform.forward * maxThrust * throttle, ForceMode.Force);
        rb.AddTorque(transform.up * yaw * responsivenessModifier);
        rb.AddTorque(transform.right * pitch * responsivenessModifier);
        rb.AddTorque(transform.forward * -roll * responsivenessModifier);
    }
    private void AnimateFlapWings()
    {
        // Flap wings bergerak berdasarkan pitch dan roll
        float leftFlapRotation = Mathf.Clamp(-roll + pitch, -flapAngleLimit, flapAngleLimit);
        float rightFlapRotation = Mathf.Clamp(roll + pitch, -flapAngleLimit, flapAngleLimit);

        leftFlapWing.localRotation = Quaternion.Euler(-90 + leftFlapRotation * rotation_multipier, 0, 0);
        rightFlapWing.localRotation = Quaternion.Euler(-90 + rightFlapRotation * rotation_multipier, 0, 0);

        BLwing.localRotation = Quaternion.Euler(-90 + leftFlapRotation, 0, 0);
        BRwing.localRotation = Quaternion.Euler(-90 + rightFlapRotation, 0, 0);
    }
    private void AnimateRudder()
    {
        // Rudder bergerak berdasarkan yaw
        float rudderRotation = Mathf.Clamp(-yaw * rotation_multipier, -rudderAngleLimit, rudderAngleLimit);
        rudder.localRotation = Quaternion.Euler(-90, rudderRotation, 0);
    }
    private void SetFlapWingRotation(float pitch, float roll)
    {
        // Mengatur rotasi flap wings berdasarkan pitch dan roll input
        float leftFlapRotation = Mathf.Clamp((-roll + pitch) * rotation_multipier, -flapAngleLimit, flapAngleLimit);
        float rightFlapRotation = Mathf.Clamp((roll + pitch) * rotation_multipier, -flapAngleLimit, flapAngleLimit);

        leftFlapWing.localRotation = Quaternion.Euler(leftFlapRotation, 0, 0);
        rightFlapWing.localRotation = Quaternion.Euler(rightFlapRotation, 0, 0);
    }
    private void SetRudderRotation(float yaw)
    {
        // Mengatur rotasi rudder berdasarkan yaw input
        float rudderRotation = Mathf.Clamp(-yaw * rotation_multipier, -rudderAngleLimit, rudderAngleLimit);
        rudder.localRotation = Quaternion.Euler(0, rudderRotation, 0);
    }
    private void ApplyAerodynamicForces()
    {
        // Mengurangi gaya angkat (lift) ketika kecepatan menurun
        float lift = Mathf.Clamp(rb.velocity.magnitude / 10f, 0f, 1f);
        rb.AddForce(Vector3.up * lift * rb.mass);
    }
    private void ApplyGravityEffect()
    {
        if (throttle == 0 && rb.velocity.magnitude < 10f)
        {
            // Menambah efek gravitasi ketika pesawat berada dalam kondisi stall
            rb.AddForce(Vector3.down * rb.mass * 2f); // Faktor 2 bisa disesuaikan
        }
    }
}
