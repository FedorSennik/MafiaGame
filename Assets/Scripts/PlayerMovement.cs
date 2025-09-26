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

    private KeyCode _forwardKey;
    private KeyCode _backwardKey;
    private KeyCode _leftKey;
    private KeyCode _rightKey;
    private KeyCode _jumpKey;
    private KeyCode _sprintKey;

    private Rigidbody rb;
    private bool isGrounded;
    private float xRotation = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

        if (gameUIManager == null)
        {
            gameUIManager = FindObjectOfType<GameUIManager>();
            if (gameUIManager == null)
            {
                Debug.LogError("PlayerMovement: GameUIManager �� �������� �� ����! ���� ������� ������ ������ ��������� ����������.");
            }
        }

        UpdateKeybinds();
    }

    void Update()
    {
        // ��������� ����'���� ����� ������� �����
        UpdateKeybinds();

        HandleMovement();
        HandleJump();
        HandleMouseLook();
        HandleCursor();

        // ��������� �������� ��� ���������� ������������ ������
        if (transform.position.y < -10f)
        {
            Debug.LogWarning("������� ����. ���������� �� ��������� �������.");
            // ����� ������ ���� ������� ����� ���������������� ��� ���������
        }
    }

    public void UpdateKeybinds()
    {
        if (KeybindManager.Instance != null)
        {
            _forwardKey = KeybindManager.Instance.keyBindings.ContainsKey("Forward") ? KeybindManager.Instance.keyBindings["Forward"] : KeyCode.W;
            _backwardKey = KeybindManager.Instance.keyBindings.ContainsKey("Backward") ? KeybindManager.Instance.keyBindings["Backward"] : KeyCode.S;
            _leftKey = KeybindManager.Instance.keyBindings.ContainsKey("Left") ? KeybindManager.Instance.keyBindings["Left"] : KeyCode.A;
            _rightKey = KeybindManager.Instance.keyBindings.ContainsKey("Right") ? KeybindManager.Instance.keyBindings["Right"] : KeyCode.D;
            _jumpKey = KeybindManager.Instance.keyBindings.ContainsKey("Jump") ? KeybindManager.Instance.keyBindings["Jump"] : KeyCode.Space;
            _sprintKey = KeybindManager.Instance.keyBindings.ContainsKey("Sprint") ? KeybindManager.Instance.keyBindings["Sprint"] : KeyCode.LeftShift;
        }
        else
        {
            // �������� �� �������������, ���� KeybindManager �������
            _forwardKey = KeyCode.W;
            _backwardKey = KeyCode.S;
            _leftKey = KeyCode.A;
            _rightKey = KeyCode.D;
            _jumpKey = KeyCode.Space;
            _sprintKey = KeyCode.LeftShift;
        }
    }

    private void HandleMovement()
    {
        if (gameUIManager != null && gameUIManager.AnyManagerUIPanelActive)
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0); // �������� �������������� ���, ���� UI ��������
            return;
        }

        float currentSpeed = speed;
        if (Input.GetKey(_sprintKey))
        {
            currentSpeed *= 1.5f; // ����������� �� ��� ���
        }

        float moveX = 0f;
        float moveZ = 0f;

        if (Input.GetKey(_forwardKey)) moveZ = 1f;
        if (Input.GetKey(_backwardKey)) moveZ = -1f;
        if (Input.GetKey(_rightKey)) moveX = 1f;
        if (Input.GetKey(_leftKey)) moveX = -1f;

        Vector3 moveDirection = transform.right * moveX + transform.forward * moveZ;
        moveDirection.Normalize();

        Vector3 targetVelocity = moveDirection * currentSpeed;

        // �������� ����������� ��������
        targetVelocity.y = rb.velocity.y;

        // ����������� ���� ������������� ��������
        rb.velocity = targetVelocity;
    }

    private void HandleJump()
    {
        if (gameUIManager != null && gameUIManager.AnyManagerUIPanelActive) return;

        if (Input.GetKeyDown(_jumpKey) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void HandleMouseLook()
    {
        if (gameUIManager != null && gameUIManager.AnyManagerUIPanelActive) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // ������� ��������� �� ����������
        transform.Rotate(Vector3.up * mouseX);

        // ������� ������ �� ���������
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cameraFollowTarget.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    private void HandleCursor()
    {
        if (gameUIManager != null)
        {
            if (!gameUIManager.AnyManagerUIPanelActive && (KeybindManager.Instance == null || !KeybindManager.Instance.isRebinding))
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

    // �������� �������� ��� ����������, �� ������� �� ����
    void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (Vector3.Dot(contact.normal, Vector3.up) > 0.7f) // ����������, �� ������� �������� ���������� �����
            {
                isGrounded = true;
                return;
            }
        }
        isGrounded = false;
    }

    void OnCollisionExit(Collision collision)
    {
        // ������'������: ����� ������� isGrounded ��� �����, ��� OnCollisionStay ��������
        // isGrounded = false;
    }
}