using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public UIManager UIManager;
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

    public KeyCode ForwardKey { get => _forwardKey; set => _forwardKey = value; }
    public KeyCode BackwardKey { get => _backwardKey; set => _backwardKey = value; }
    public KeyCode LeftKey { get => _leftKey; set => _leftKey = value; }
    public KeyCode RightKey { get => _rightKey; set => _rightKey = value; }
    public KeyCode JumpKey { get => _jumpKey; set => _jumpKey = value; }

    private Rigidbody rb;
    private bool isGrounded;
    private float xRotation = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Заблокировать наклон персонажа
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        HandleRotation();
        HandleCursor();
    }

    private void HandleMovement()
    {
        float moveX = (Input.GetKey(_leftKey) ? -1 : 0) + (Input.GetKey(_rightKey) ? 1 : 0);
        float moveZ = (Input.GetKey(_forwardKey) ? 1 : 0) + (Input.GetKey(_backwardKey) ? -1 : 0);

        Vector3 moveDirection = transform.right * moveX + transform.forward * moveZ;
        rb.velocity = new Vector3(moveDirection.x * speed, rb.velocity.y, moveDirection.z * speed);
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
        if (UIManager.AllUIIsClosed)
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

    void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }

    void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
}