using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    CharacterController controller;
    Animator animator;

    [SerializeField] float speed = 5f;
    [SerializeField] float turnSmoothTime = 0.2f;

    public Transform camera;
    public Transform groundCheck;

    float turnSmoothVelocity;
    public float gravity = -9.81f;
    public float groundDistance = 0.4f;
    public float jumHeight = 3f;

    LayerMask groundMask;
    Vector3 velocity;
    bool isGrounded;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
        groundMask = LayerMask.GetMask("Ground");
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        GroundCheck();
        RegularMovement();
        //Jump();
    }

    private void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded) { animator.SetBool("Jump", false); }
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }

    /*private void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            animator.SetBool("Jump", true);
            velocity.y = Mathf.Sqrt(jumHeight * -2 * gravity);
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }*/

    private void RegularMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical);
        //print("Position X:" + horizontal + "  Position Z:" + vertical);

        animator.SetFloat("VelocityX", horizontal);
        animator.SetFloat("VelocityZ", vertical);

        if (direction.magnitude >= 0.5f)
        {

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, camera.eulerAngles.y, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDirection * speed * direction.magnitude * Time.deltaTime);
        }
    }
}
