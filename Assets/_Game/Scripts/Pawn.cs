using Cinemachine.Utility;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Windows;

[RequireComponent(typeof(Rigidbody))]
public class Pawn : MonoBehaviour
{
    public float moveSpeed = 1.0f;

    [Range(1.0f, 80.0f)]
    public float maxGroundSlope = 45.0f;

    public bool IsGrounded { get { return groundCollisions.Any(); } }

    public float downForce = 1.0f;

    public float airControlMultiplier = 0.5f;

    public float rotationSlerpFactor = 1.0f;

    public Vector2 Velocity { get; private set; } = Vector2.zero;
    public Vector2 RawInput { get; private set; } = Vector2.zero;

    private new Rigidbody rigidbody = null;

    private List<Collider> groundCollisions = new List<Collider>();
    private List<ContactPoint> contacts = new List<ContactPoint>(10);

    private Vector2 targetRotation = Vector2.zero;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    public void Move(float horizontalInput, float verticalInput)
    {
        //Calculate normals
        Vector3 groundNormal = Vector3.up;
        if (IsGrounded)
        {
            groundNormal = Vector3.zero;
            contacts.ForEach(x => groundNormal += x.normal);
            groundNormal.Normalize();
        }
        contacts.Clear();
        Quaternion groundNormalModifier = Quaternion.FromToRotation(Vector3.up, groundNormal);

        Vector3 oldPos = transform.position;

        //Air Control
        if (!IsGrounded)
        {
            horizontalInput *= airControlMultiplier;
            verticalInput *= airControlMultiplier;
        }

        Vector3 newPos = transform.position + groundNormalModifier * new Vector3(horizontalInput, 0.0f, verticalInput) * moveSpeed * Time.fixedDeltaTime;

        Velocity = (newPos - oldPos) * (1.0f / Time.deltaTime);
        RawInput = new Vector2(horizontalInput, verticalInput);

        //Gravity
        if (!IsGrounded)
            rigidbody.AddForce(Physics.gravity * downForce);
        else
            rigidbody.velocity = Vector3.zero;

        //Apply
        rigidbody.MovePosition(newPos);

        //Rotation
        /*targetRotation = Vector3.ProjectOnPlane(newPos - oldPos, Vector3.up).normalized;

        if (velocityChangeDirection.magnitude != 0.0f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(velocityChangeDirection);

            Quaternion newRot = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotationSlerpFactor);

            rigidbody.MoveRotation(newRot);
        }*/

        Debug.DrawLine(transform.position, transform.position + (newPos - oldPos).normalized, Color.red);
        Debug.DrawLine(transform.position, transform.position - groundNormal, Color.blue);
    }

    public void LookTo(Vector2 direction)
    {
        if (direction.magnitude == 0.0f)
            return;

        targetRotation = direction.normalized;
    }

    private void FixedUpdate()
    {
        if (targetRotation.magnitude == 0.0f)
            return;

        Quaternion qTargetRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(new Vector3(targetRotation.x, transform.position.y, targetRotation.y), Vector3.up).normalized);

        Quaternion newRot = Quaternion.Slerp(transform.rotation, qTargetRotation, Time.fixedDeltaTime * rotationSlerpFactor);

        rigidbody.MoveRotation(newRot);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!groundCollisions.Contains(collision.collider))
        {
            if (IsGround(collision.contacts[0].normal))
                groundCollisions.Add(collision.collider);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (groundCollisions.Contains(collision.collider))
        {
            contacts.AddRange(collision.contacts);
            Debug.DrawLine(transform.position, transform.position + collision.contacts[0].normal, Color.green, 0.5f);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (groundCollisions.Contains(collision.collider))
            groundCollisions.Remove(collision.collider);
    }

    private bool IsGround(Vector3 normal)
    {
        float dot = Vector3.Dot(normal, Vector3.up);
        return dot >= maxGroundSlope / 90.0f;
    }
}
