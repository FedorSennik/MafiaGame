using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public UIManager UIManager;
    public float speed = 5f;
    public float jumpForce = 4f;
    public float mouseSensitivity = 2f;

    private KeyCode _forwardKey = KeyCode.W;
    private KeyCode _backwardKey = KeyCode.S;
    private KeyCode _leftKey = KeyCode.A;
    private KeyCode _rightKey = KeyCode.D;
    private KeyCode _jumpKey = KeyCode.Space;


    public KeyCode ForwardKey
    {
        get { return _forwardKey; }
        set { _forwardKey = value; }
    }

    public KeyCode BackwardKey
    {
        get { return _backwardKey; }
        set { _backwardKey = value; }
    }

    public KeyCode LeftKey
    {
        get { return _leftKey; }
        set { _leftKey = value; }
    }

    public KeyCode RightKey
    {
        get { return _rightKey; }
        set { _rightKey = value; }
    }

    public KeyCode JumpKey
    {
        get { return _jumpKey; }
        set { _jumpKey = value; }
    }


    private Rigidbody rb;
    private bool isGrounded;
    private Transform cameraTransform;
    private float xRotation = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;

        // Заблокувати нахил персонажа
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        // Рух персонажа зі змінними кнопками введення
        float moveX = (Input.GetKey(_leftKey) ? -1 : 0) + (Input.GetKey(_rightKey) ? 1 : 0);
        float moveZ = (Input.GetKey(_forwardKey) ? 1 : 0) + (Input.GetKey(_backwardKey) ? -1 : 0);
        Vector3 movement = transform.right * moveX + transform.forward * moveZ;
        rb.velocity = new Vector3(movement.x * speed, rb.velocity.y, movement.z * speed);

        // Стрибок
        if (Input.GetKeyDown(_jumpKey) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        // Управління камерою
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        if (UIManager.AllUIIsClosed)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
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