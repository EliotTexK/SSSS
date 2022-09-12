using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// gravitates toward other objects by the inverse square law
[RequireComponent(typeof(CircleCollider2D))]
public class OrbitTarget : MonoBehaviour
{
    public Transform target;
    public float density = 0.1f;
    private float mass;
    public Vector2 velocity;
    const float gravConstant = 4f;
    // adjust orbit to be stable and circular
    public float adjustOrbitIntensity = 10f;
    // Start is called before the first frame update
    void Start() {
        CircleCollider2D myCollider = GetComponent<CircleCollider2D>();
        mass = density * Mathf.PI * myCollider.radius * myCollider.radius;
        // give an initial velocity that will stabilize orbit
        Vector2 targetDir = (target.position - transform.position);
        float distance = targetDir.magnitude;
        Vector2 perpendicularNorm = Vector2.Perpendicular(targetDir).normalized;
        velocity = perpendicularNorm * Mathf.Sqrt(gravConstant * mass / distance) * 0.01f;
        // apply gravitational acceleration, too
        float gravScalar = gravConstant * mass / (distance * distance);
        Vector2 gravAccel = targetDir.normalized * gravScalar;
        velocity += gravAccel;
    }

    void FixedUpdate() {
        if (target)
        {
            // apply gravitational acceleration
            Vector2 targetDir = (target.position - transform.position);
            float distance = targetDir.magnitude;
            float gravScalar = gravConstant * mass / (distance * distance);
            Vector2 gravAccel = targetDir.normalized * gravScalar;
            velocity += gravAccel * Time.fixedDeltaTime;
            // adjust orbit
            Vector2 perpendicularNorm = Vector2.Perpendicular(targetDir).normalized;
            Vector2 velCentrip = Vector2.Dot(velocity, perpendicularNorm) * perpendicularNorm;
            Vector2 velTowards = Vector2.Dot(velocity, targetDir.normalized) * targetDir.normalized;
            // add centripetal
            velocity += velCentrip * velTowards.magnitude * Time.fixedDeltaTime * adjustOrbitIntensity;
            velocity -= velTowards * Time.fixedDeltaTime * adjustOrbitIntensity;
            DrawPhysicsVector(velCentrip, 200f);
            DrawPhysicsVector(velTowards, 200f);
        }
        transform.Translate(velocity);
    }

    private void DrawPhysicsVector(Vector2 vector, float scale) {
        Debug.DrawLine(transform.position, (Vector2) transform.position + vector * scale);
    }

    void OnDrawGizmos() {
    }
}
