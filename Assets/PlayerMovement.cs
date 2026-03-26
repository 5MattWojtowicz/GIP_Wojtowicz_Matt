using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    public float spring_kracht = 1f;
    float kracht = 0f;
    bool raakt_grond = true;
    bool kan_walrunnen = true;
    public static bool aan_het_walrunnen = false;
    public float snelheid = 5f;
    public float wallrun_springkracht_voorwaarts = 3f;
    public float wallrun_springkracht_omhoog = 3f;
    bool afkoelen_walrunnen = false;
    float timer = 0f;
    public float afkoeltijd_walrunnen = 2f;
    public float hoogte_persoon = 1f;
    public static int wallSide = 0;
    private Vector3 wallRunDirection;

    public float maxSpeed = 8f;
    public float acceleration = 20f;
    public float stopFriction = 40f;

    public float slopeAngle;
    public Vector3 slopeNormal;
    public bool wilstartenmetsliden = false;
    public bool aan_het_sliden = false;
    public Vector3 slideDirection;
    public float afstandTotSlope;
    private Vector3 startSlideVelocity;
    public float VooruitKrachtSliden = 0.25f;
    public float slideAcceleration = 2f;    
    public float maxSlideSpeed = 5f;     
    public float slideStrafeMultiplier = 0.7f;
    public float springkracht_naSliden = 2f;
    public float sliden_springkracht_voorwaarts = 2f;
    public float sliden_springkracht_omhoog = 5f;


    public static bool zijdeling_walrunnen = false;
    public bool kantelen_rechts = false;
    public float zijdeling_springkracht_zijkant = 3f;
    public float zijdeling_springkracht_omhoog = 3f;
    public float afkoeltijd_zijdelings_walrunnen = 2f;
    public bool kan_zijdelings_walrunnen = true;
    public bool afkoelen_zijdelings_walrunnen = false;

    private Rigidbody rb;

    public Transform capsule;
    public Transform playerCamera;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        //rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    void FixedUpdate()
    {
        float moveX = 0;
        float moveZ = 0;

        if (Input.GetKey(KeyCode.W)) moveZ = 1f;
        if (Input.GetKey(KeyCode.S)) moveZ = -1f;
        if (Input.GetKey(KeyCode.A)) moveX = -1f;
        if (Input.GetKey(KeyCode.D)) moveX = 1f;

        Vector3 input = new Vector3(moveX, 0f, moveZ);
        Vector3 moveDir;

        if (aan_het_walrunnen || zijdeling_walrunnen)
            moveDir = wallRunDirection * moveZ;
        else
            moveDir = transform.TransformDirection(input);

        if (aan_het_sliden)
        {

            if (Input.GetKey(KeyCode.W)) moveZ = 1f;
            if (Input.GetKey(KeyCode.S)) moveZ = -1f;
            if (Input.GetKey(KeyCode.A)) moveX = -1f;
            if (Input.GetKey(KeyCode.D)) moveX = 1f;

            Vector3 slopeDir = slideDirection.normalized;

            Vector3 inputDir = transform.TransformDirection(new Vector3(moveX, 0f, moveZ));
            inputDir *= slideStrafeMultiplier;

            Vector3 slideMove = (slopeDir + inputDir).normalized;

            rb.AddForce(slideMove * slideAcceleration, ForceMode.Acceleration);

            if (aan_het_sliden == false)
            {
                Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
                if (flatVel.magnitude > maxSlideSpeed)
                {
                    Vector3 limited = flatVel.normalized * maxSlideSpeed;
                    rb.linearVelocity = new Vector3(limited.x, rb.linearVelocity.y, limited.z);
                }
            }

            return;
        }

        if (Input.GetKey(KeyCode.C) && afstandTotSlope < 2f)
        {
            rb.AddForce(slideDirection.normalized * (slideAcceleration * 0.5f), ForceMode.Impulse);
            aan_het_sliden = true;
            return;
        }

        if (input.magnitude > 0.1f)
        {
            rb.AddForce(moveDir.normalized * acceleration, ForceMode.Acceleration);

            Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            if (flatVel.magnitude > maxSpeed)
            {
                Vector3 limited = flatVel.normalized * maxSpeed;
                rb.linearVelocity = new Vector3(limited.x, rb.linearVelocity.y, limited.z);
            }
        }
        else
        {
            Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

            Vector3 stop = Vector3.Lerp(flatVel, Vector3.zero, stopFriction * Time.fixedDeltaTime);
            rb.linearVelocity = new Vector3(stop.x, rb.linearVelocity.y, stop.z);
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (kan_zijdelings_walrunnen == true)
        {
            if (collision.gameObject.CompareTag("zijdelings_muur"))
            {
                zijdeling_walrunnen = true;

                Vector3 wallNormal = collision.contacts[0].normal;

                if (Vector3.Dot(wallNormal, transform.right) > 0)
                {
                    wallSide = -1;
                    kantelen_rechts = true;

                }
                else
                {
                    wallSide = 1;
                    kantelen_rechts = false;

                }

                wallRunDirection = Vector3.Cross(wallNormal, Vector3.up);

                if (Vector3.Dot(wallRunDirection, transform.forward) < 0)
                    wallRunDirection = -wallRunDirection;

                kan_zijdelings_walrunnen = false;
                zijdeling_walrunnen = true;

                rb.constraints = RigidbodyConstraints.FreezePositionY |
                                    RigidbodyConstraints.FreezeRotation;
            }
        }

        if (Input.GetKey(KeyCode.C))
        {
            aan_het_sliden = true;
            Vector3 vel = rb.linearVelocity;
            startSlideVelocity = new Vector3(vel.x, 0f, vel.z);

            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.5f))
            {
                afstandTotSlope = hit.distance;

                if (collision.gameObject.CompareTag("Slope"))
                    slopeNormal = hit.normal;

                slopeAngle = Vector3.Angle(slopeNormal, Vector3.up);

                if (aan_het_sliden)
                {
                    slideDirection = Vector3.ProjectOnPlane(Vector3.down, slopeNormal).normalized;
                }
            }
        }

        if (collision.gameObject.CompareTag("Grond"))
        {
            raakt_grond = true;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            if (collision.gameObject.CompareTag("Grond"))
            {
                raakt_grond = true;
                kracht = 1f;
                rb.AddForce(Vector3.up * spring_kracht, ForceMode.Impulse);
            }
        }

        if (kan_walrunnen)
        {
            if (collision.gameObject.CompareTag("Muur"))
            {
                Vector3 wallNormal = collision.contacts[0].normal;

                if (Vector3.Dot(wallNormal, transform.right) > 0)
                    wallSide = -1;
                else
                    wallSide = 1;

                wallRunDirection = Vector3.Cross(wallNormal, Vector3.up);

                if (Vector3.Dot(wallRunDirection, transform.forward) < 0)
                    wallRunDirection = -wallRunDirection;

                kan_walrunnen = false;
                aan_het_walrunnen = true;

                rb.constraints = RigidbodyConstraints.FreezePositionY |
                                 RigidbodyConstraints.FreezeRotation;
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Slope_collider"))
        {
            if (aan_het_sliden == true && Input.GetKey(KeyCode.Space))
            {
                transform.rotation = Quaternion.Euler(0f, playerCamera.eulerAngles.y, 0f);

                Vector3 springhoek = playerCamera.forward;

                rb.AddForce(springhoek * sliden_springkracht_voorwaarts, ForceMode.Impulse);
                rb.AddForce(Vector3.up * sliden_springkracht_omhoog, ForceMode.Impulse);
            }

        }
    }


    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Grond"))
        {
            raakt_grond = false;
        }
        if (collision.gameObject.CompareTag("Muur"))
        {
            afkoelen_walrunnen = true;
            aan_het_walrunnen = false;
            rb.constraints &= ~RigidbodyConstraints.FreezePositionX;
            rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
        }
        if (collision.gameObject.CompareTag("Slope"))
        {
            aan_het_sliden = false;
        }

        if (collision.gameObject.CompareTag("zijdelings_muur"))
        {
            afkoelen_zijdelings_walrunnen = true;
            zijdeling_walrunnen = false;
            rb.constraints &= ~RigidbodyConstraints.FreezePositionX;
            rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
        }
    }

    void Update()
    {
        if (aan_het_walrunnen && Input.GetKeyDown(KeyCode.Space))
        {
            transform.rotation = Quaternion.Euler(0f, playerCamera.eulerAngles.y, 0f);

            Vector3 springhoek = playerCamera.forward;

            rb.AddForce(springhoek * wallrun_springkracht_voorwaarts, ForceMode.Impulse);
            rb.AddForce(Vector3.up * wallrun_springkracht_omhoog, ForceMode.Impulse);
        }

        if (afkoelen_walrunnen)
        {
            timer += Time.deltaTime;
            if (timer >= afkoeltijd_walrunnen)
            {
                kan_walrunnen = true;
                timer = 0f;
                afkoelen_walrunnen = false;
            }
        }
        if(afkoelen_zijdelings_walrunnen == true)
        {
            timer += Time.deltaTime;
            if (timer >= afkoeltijd_zijdelings_walrunnen)
            {
                kan_zijdelings_walrunnen = true;
                timer = 0f;
                afkoelen_zijdelings_walrunnen = false;
            }
        }

        if (aan_het_walrunnen)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                afkoelen_walrunnen = true;
                aan_het_walrunnen = false;
                rb.constraints &= ~RigidbodyConstraints.FreezePositionX;
                rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
            }
        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            aan_het_sliden = false;
        }

        if (zijdeling_walrunnen && Input.GetKeyDown(KeyCode.Space))
        {

            transform.rotation = Quaternion.Euler(0f, playerCamera.eulerAngles.y, 0f);

           

            if (kantelen_rechts == true)
            {

                Vector3 springhoek = playerCamera.forward;
                rb.AddForce(springhoek * zijdeling_springkracht_zijkant, ForceMode.Impulse);
                rb.AddForce(Vector3.up * zijdeling_springkracht_omhoog, ForceMode.Impulse);
            }
            else if(kantelen_rechts == false)
            {

                Vector3 springhoek = playerCamera.forward;
                rb.AddForce(springhoek * zijdeling_springkracht_zijkant, ForceMode.Impulse);
                rb.AddForce(Vector3.up * zijdeling_springkracht_omhoog, ForceMode.Impulse);
            }
        }
        if (zijdeling_walrunnen)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                afkoelen_zijdelings_walrunnen = true;
                zijdeling_walrunnen = false;
                rb.constraints &= ~RigidbodyConstraints.FreezePositionX;
                rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
            }
        }
    }

    public void Crouchen()
    {
        Vector3 scale = capsule.localScale;
        scale.y = 0.5f;
        capsule.localScale = scale;
    }

    public void Uncrouchen()
    {
        Vector3 scale = capsule.localScale;
        scale.y = hoogte_persoon;
        capsule.localScale = scale;
    }
}