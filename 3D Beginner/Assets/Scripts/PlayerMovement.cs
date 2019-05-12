using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;
    public float turnSpeed = 20;     // Angle in radians for character to turn per second
    Animator m_Animator;
    Rigidbody m_Rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    // Instead of being called before every rendered frame, FixedUpdate is called before the physics system solves any collisions and other interactions that have happened.  By default it is called exactly 50 times every second.
    void FixedUpdate()
    {
        float horizonal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        m_Movement.Set(horizonal, 0f, vertical);
        m_Movement.Normalize(); // Prevents character from moving faster diagonally than it will along a single axis

        bool hasHorizontalInput = !Mathf.Approximately(horizonal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool("IsWalking", isWalking);

        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);  // Character rotation
        m_Rotation = Quaternion.LookRotation(desiredForward);

    }

    // This method allows you to apply root motion as you want, which means that movement and rotation can be applied separately.
    private void OnAnimatorMove()
    {
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude);
        m_Rigidbody.MoveRotation(m_Rotation);
    }
}
