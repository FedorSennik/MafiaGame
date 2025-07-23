using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameUIManager gameUIManager;
    public float speed = 5f;
    public float jumpForce = 4f;
    public float mouseSensitivity = 2f;

    [Header("Camera Settings")]
    [SerializeField] private Transform cameraFollowTarget;

    private KeyCode _forwardKey = KeyCode.W;
    private KeyCode _backwardKey = KeyCode.S;
    private KeyCode _leftKey = KeyCode.A;
    private KeyCode _rightKey = KeyCode.D;
    private KeyCode _jumpKey = KeyCode.Space;
    private KeyCode _sprintKey = KeyCode.LeftShift;

    private Rigidbody rb;
    private bool isGrounded;
    private float xRotation = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

        //UpdateKeybinds();
    }

    void Update()
    {
        //UpdateKeybinds();

        HandleMovement();
        HandleJump();
        HandleRotation();
        HandleCursor();
    }

    /* void UpdateKeybinds()
    {
        if (KeybindManager.Instance != null)
        {
            _forwardKey = KeybindManager.Instance.GetKey("Forward");
            _backwardKey = KeybindManager.Instance.GetKey("Backward");
            _leftKey = KeybindManager.Instance.GetKey("Left");
            _rightKey = KeybindManager.Instance.GetKey("Right");
            _jumpKey = KeybindManager.Instance.GetKey("Jump");
            _sprintKey = KeybindManager.Instance.GetKey("Sprint");
        }
    }*/

    private void HandleMovement()
    {
        float currentSpeed = speed;
        if (Input.GetKey(_sprintKey))
        {
            currentSpeed *= 1.5f;
        }

        float moveX = 0;
        if (Input.GetKey(_leftKey)) moveX -= 1;
        if (Input.GetKey(_rightKey)) moveX += 1;

        float moveZ = 0;
        if (Input.GetKey(_forwardKey)) moveZ += 1;
        if (Input.GetKey(_backwardKey)) moveZ -= 1;

        Vector3 moveDirection = transform.right * moveX + transform.forward * moveZ;
        rb.velocity = new Vector3(moveDirection.x * currentSpeed, rb.velocity.y, moveDirection.z * currentSpeed);
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(_jumpKey) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Поворот персонажа по горизонтали
        transform.Rotate(Vector3.up * mouseX);

        // Поворот камеры по вертикали
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cameraFollowTarget.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    private void HandleCursor()
    {
        if (gameUIManager != null)
        {
            if (gameUIManager.AllUIIsClosed && !KeybindManager.Instance.isRebinding)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
        else
        {
            if (KeybindManager.Instance != null && KeybindManager.Instance.isRebinding)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }

    void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
}
