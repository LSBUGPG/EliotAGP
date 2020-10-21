using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;

    public Transform cam;

    public float speed = 5f;
    public float jumpHeight = 2f;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    public float gravity = -10f;

    public Transform groundCheck;
    public float groundDistance = 0.3f;
    public LayerMask groundMask;


    Vector3 velocity;
    bool isGrounded;
    MovingStones touchingStone = null;
    Vector3 pushed = Vector3.zero;

    public void Push(Vector3 push)
    {
        pushed += push;
    }

    void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        if (Input.GetButtonDown("Jump") && (isGrounded || touchingStone != null))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime + pushed);

        pushed = Vector3.zero;
    }

    void OnTriggerEnter(Collider collider)
    {
        MovingStones stone = collider.gameObject.GetComponent<MovingStones>();
        if (stone != null)
        {
            if (touchingStone != null && touchingStone != stone)
            {
                touchingStone.StopPushingPlayer();
            }

            stone.PushPlayer(this);
            touchingStone = stone;
            Debug.LogFormat("Set stone {0} to push player", stone.gameObject.name);
        }
    }
    
    void OnTriggerExit(Collider collider)
    {
        MovingStones stone = collider.gameObject.GetComponent<MovingStones>();
        if (stone != null)
        {
            if (touchingStone == stone)
            {
                Debug.LogFormat("Set stone {0} to stop pushing player", stone.gameObject.name);
                touchingStone.StopPushingPlayer();
                touchingStone = null;
            }
        }
    }
}
