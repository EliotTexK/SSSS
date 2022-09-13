using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// gravitates toward other objects by the inverse square law
[RequireComponent(typeof(CircleCollider2D))]
public class OrbitTarget : MonoBehaviour
{
    public bool updatePhysics = false;
    public Transform target;
    private float mass;
    public Vector2 velocity;
    const float gravConstant = 15f;
    // adjust orbit to be stable and circular
    public float adjustOrbitIntensity = 2f;
    // the adjustment value decreases as time goes on
    public float adjustActual;
    // Start is called before the first frame update
    void Start() {
        CircleCollider2D myCollider = GetComponent<CircleCollider2D>();
        mass = Mathf.PI * myCollider.radius * myCollider.radius;
        // give an initial velocity that will stabilize orbit
        applyInitialForce();
        adjustActual = adjustOrbitIntensity * 4;
    }

    void FixedUpdate() {
        if (updatePhysics) {
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
                velocity += velCentrip * velTowards.magnitude * Time.fixedDeltaTime * adjustActual;
                velocity -= velTowards * Time.fixedDeltaTime * adjustActual;
            }
            // adjust orbit more during the first few seconds of gameplay
            if (adjustActual > adjustOrbitIntensity/4) {
                adjustActual -= Time.fixedDeltaTime;
            }
            transform.Translate(velocity);
        }
    }

    private void applyInitialForce() {
        Vector2 targetDir = (target.position - transform.position);
        float distance = targetDir.magnitude;
        Vector2 perpendicularNorm = Vector2.Perpendicular(targetDir).normalized;
        float gravForce = gravConstant * mass / (distance * distance);
        velocity = perpendicularNorm * (0.1f + 0.05f * gravForce);
    }

    private void DrawPhysicsVector(Vector2 vector, float scale) {
        Debug.DrawLine(transform.position, (Vector2) transform.position + vector * scale);
    }

    void OnDrawGizmos() {
    }
}
