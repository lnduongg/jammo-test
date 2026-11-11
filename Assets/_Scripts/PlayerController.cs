using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotateSpeed = 15f;
    public float gravity = -9.8f;
    private float verticalVelocity = 0f;
    private CharacterController controller;
    private Animator animator;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 moveInput = new Vector3(h, 0f, v).normalized;

        if(moveInput.magnitude >= 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveInput);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
            animator.SetFloat("Blend", 1f, 0.1f, Time.deltaTime);
        }
        else
        {
            animator.SetFloat("Blend", 0f, 0.1f, Time.deltaTime);
        }

        if(controller.isGrounded)
        {
            verticalVelocity = -2f;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
        Vector3 finalVelocity = new Vector3(moveInput.x * moveSpeed, verticalVelocity, moveInput.z * moveSpeed);

        controller.Move(finalVelocity * Time.deltaTime);
    }
}