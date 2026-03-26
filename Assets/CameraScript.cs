using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public float mouseSensitivity = 1f;
    public Transform playerCamera;

    private float xRotation = 0f;
    public float yRotation = 0f;

    public float wallrunTilt = 15f;
    public float tiltSpeed = 5f;
    private float currentTilt = 0f;


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

        transform.Rotate(Vector3.up * mouseX);

        float targetTilt = 0f;

        if (PlayerMovement.aan_het_walrunnen || PlayerMovement.zijdeling_walrunnen)
        {
            targetTilt = wallrunTilt * PlayerMovement.wallSide;
        }

        currentTilt = Mathf.Lerp(currentTilt, targetTilt, Time.deltaTime * tiltSpeed);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, currentTilt);
    }


}