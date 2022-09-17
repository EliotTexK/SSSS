using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// orbits a target, adjusts orbit to be circular and stable
[RequireComponent(typeof(NewtonianPhysics))]
public class OrbitTarget : MonoBehaviour
{
    public Transform target;
    NewtonianPhysics myPhysics;
    public float adjustOrbitIntensity = 2f;
    // the adjustment value decreases as time goes on
    public float adjustActual;
    // Start is called before the first frame update
    void Start() {
        myPhysics = GetComponent<NewtonianPhysics>();
        // give an initial velocity that will stabilize orbit
        applyInitialForce();
        adjustActual = adjustOrbitIntensity * 4;
    }

    void FixedUpdate() {
        if (GameManager.Instance.updatePhysics) {
            if (target)
            {
                // apply gravitational acceleration
                Vector2 targetDir = (target.position - transform.position);
                float distance = targetDir.magnitude;
                float gravScalar = NewtonianPhysics.gravConstant * myPhysics.mass / (distance * distance);
                Vector2 gravAccel = targetDir.normalized * gravScalar;
                myPhysics.velocity += gravAccel * Time.fixedDeltaTime;
                // adjust orbit
                Vector2 perpendicularNorm = Vector2.Perpendicular(targetDir).normalized;
                Vector2 velCentrip = Vector2.Dot(myPhysics.velocity, perpendicularNorm) * perpendicularNorm;
                Vector2 velTowards = Vector2.Dot(myPhysics.velocity, targetDir.normalized) * targetDir.normalized;
                // add forces
                myPhysics.velocity += velCentrip * velTowards.magnitude * Time.fixedDeltaTime * adjustActual;
                myPhysics.velocity -= velTowards * Time.fixedDeltaTime * adjustActual;
                transform.Translate(targetDir * Time.fixedDeltaTime * distance * distance * 0.000005f);
            }
            // adjust orbit more during the first few seconds of gameplay
            if (adjustActual > adjustOrbitIntensity/4) {
                adjustActual -= Time.fixedDeltaTime;
            }
        }
    }

    public void applyInitialForce() {
        Vector2 targetDir = (target.position - transform.position);
        float distance = targetDir.magnitude;
        Vector2 perpendicularNorm = Vector2.Perpendicular(targetDir).normalized;
        float gravForce = NewtonianPhysics.gravConstant * myPhysics.mass / (distance * distance);
        myPhysics.velocity = perpendicularNorm * (0.1f + 0.05f * gravForce);
    }

    private void DrawPhysicsVector(Vector2 vector, float scale) {
        Debug.DrawLine(transform.position, (Vector2) transform.position + vector * scale);
    }

    void OnDrawGizmos() {
    }
}
