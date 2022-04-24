using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController : MonoBehaviour
{
    [SerializeField] PhysicsSettings settings;
    Rigidbody body;
    new Collider collider;

    void Awake()
    {
        body = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }

    public virtual void AddForce(Vector3 force, ForceMode mode = ForceMode.Force)
    {
        body.AddForce(force, mode);
    }

    public virtual void AddPosition(Vector3 positionAddition)
    {
        transform.position += positionAddition;
    }

    public virtual void MultiplyVelocity(float multiplier)
    {
        SetVelocity(body.velocity * multiplier);
    }

    public virtual void SetVelocity(Vector3 newVelocity)
    {
        body.velocity = newVelocity;
    }

    #region Physics Check

    public RaycastHit2D RayCast(Collider2D origin, Vector3 direction, float distance, LayerMask mask)
    {
        float extentsAxis = Mathf.Abs(direction.x) > Mathf.Abs(direction.y) ? origin.bounds.extents.x : origin.bounds.extents.y;
        RaycastHit2D hit = Physics2D.Raycast(origin.bounds.center, direction, extentsAxis + distance, mask);
        return hit;
    }

    #endregion

    void OnEnable()
    {

    }

    void OnDisable()
    {

    }


    public Rigidbody Body { get => body; }
    public PhysicsSettings Settings { get => settings; }
    public Collider Collider { get => collider; set => collider = value; }
    public Vector3 Velocity { get => body.velocity; }
}
