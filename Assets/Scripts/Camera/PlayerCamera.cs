using UnityEngine;

/// <summary>
/// Handles first-person camera look and rotation.
/// Includes mouse sensitivity and vertical look clamping.
/// </summary>
public class PlayerCamera : MonoBehaviour
{
    [Header("Mouse Settings")]
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float maxLookUp = 90f;
    [SerializeField] private float maxLookDown = 90f;

    private float xRotation = 0f;
    private Transform playerBody;

    private void Start()
    {
        playerBody = transform.parent;

        // Lock and hide cursor
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        HandleMouseLook();
        HandleCursorToggle();
    }

    private void HandleMouseLook()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotate player body left/right
        playerBody.Rotate(Vector3.up * mouseX);

        // Rotate camera up/down with clamping
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -maxLookUp, maxLookDown);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    private void HandleCursorToggle()
    {
        // Toggle cursor lock with ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

    public void SetMouseSensitivity(float sensitivity)
    {
        mouseSensitivity = sensitivity;
    }
}
