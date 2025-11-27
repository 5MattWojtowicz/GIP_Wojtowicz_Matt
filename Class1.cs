using Unity.Mathematics;
using UnityEngine;

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


    private Rigidbody rb;

    public Transform playerCamera;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {

        float moveX = 0;
        float moveZ = 0;

        if (Input.GetKey(KeyCode.W))
            moveZ = 1f;
        if (Input.GetKey(KeyCode.S))
            moveZ = -1f;
        if (Input.GetKey(KeyCode.A))
            moveX = -1f;
        if (Input.GetKey(KeyCode.D))
            moveX = 1f;

        if (Input.GetKeyDown(KeyCode.C))
        {

        }


        Vector3 movement = new Vector3(moveX, 0f, moveZ).normalized;
        rb.MovePosition(rb.position + transform.TransformDirection(movement) * snelheid * Time.fixedDeltaTime);
    }
    void OnCollisionStay(Collision collision)
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (collision.gameObject.CompareTag("Grond"))
            {
                raakt_grond = true;
                kracht = 1f;
                rb.AddForce(Vector3.up * (spring_kracht), ForceMode.Impulse);
            }
        }
        if (kan_walrunnen == true)
        {
            if (collision.gameObject.CompareTag("Muur"))
            {
                Debug.Log("Gestart met Walrunnen");
                kan_walrunnen = false;
                aan_het_walrunnen = true;
                rb.constraints = RigidbodyConstraints.FreezePositionY |
                                 RigidbodyConstraints.FreezeRotation |
                                 RigidbodyConstraints.FreezePositionX;
            }
        }
    }
    void OnCollisionExit(Collision collision)
    {

        if (collision.gameObject.CompareTag("Grond"))
        {
            raakt_grond = false;
        }

    }
    void Update()
    {
        if (aan_het_walrunnen == true && Input.GetKeyDown(KeyCode.Space))
        {
            transform.rotation = Quaternion.Euler(0f, playerCamera.eulerAngles.y, 0f);

            Vector3 springhoek = playerCamera.forward;

            rb.AddForce(springhoek * wallrun_springkracht_voorwaarts, ForceMode.Impulse);
            rb.AddForce(Vector3.up * wallrun_springkracht_omhoog, ForceMode.Impulse);
        }
        if (afkoelen_walrunnen == true)
        {
            timer += Time.deltaTime;
            if (timer >= afkoeltijd_walrunnen)
            {
                kan_walrunnen = true;
                timer = 0f;
                afkoelen_walrunnen = false;
                Debug.Log("Je kan terug walrunnen");
            }
        }
        if (aan_het_walrunnen == true)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                Debug.Log("gestopt met walrunnen");
                afkoelen_walrunnen = true;
                aan_het_walrunnen = false;
                rb.constraints &= ~RigidbodyConstraints.FreezePositionX;
                rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
            }
        }
    }
}





using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public float mouseSensitivity = 1f;
    public Transform playerCamera;

    private float xRotation = 0f;
    private float yRotation = 0f;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        yRotation += mouseX;
        yRotation = Mathf.Clamp(-90f, yRotation, 90f);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        if (PlayerMovement.aan_het_walrunnen == true)
        {
            playerCamera.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);


        }
        else if (PlayerMovement.aan_het_walrunnen == false)
        {
            yRotation = 0f;
            transform.Rotate(Vector3.up * mouseX);
        }
    }
}

