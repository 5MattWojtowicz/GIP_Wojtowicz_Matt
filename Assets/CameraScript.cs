using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public float mouseSensitivity = 1f;
    public Transform playerCamera;    

    private float xRotation = 0f;
    public float yRotation = 0f;

    public PlayerMovement playerMovement;


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
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        if (PlayerMovement.aan_het_walrunnen == true)
        {
            playerCamera.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);


        }
        else if (PlayerMovement.aan_het_walrunnen == false)
        {
            transform.Rotate(Vector3.up * mouseX);
        }

    }
}
