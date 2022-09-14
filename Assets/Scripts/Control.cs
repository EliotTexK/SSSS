using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NewtonianPhysics))]
public class Control : MonoBehaviour
{
    NewtonianPhysics myNewtonianPhysics;
    public float controlIntensity = 10f;
    void Start() {
        myNewtonianPhysics = GetComponent<NewtonianPhysics>();
    }
    void FixedUpdate()
    {
        float forceX = Input.GetAxisRaw("Horizontal") * Time.fixedDeltaTime * controlIntensity;
        float forceY = Input.GetAxisRaw("Vertical") * Time.fixedDeltaTime * controlIntensity;

        myNewtonianPhysics.velocity += new Vector2(forceX, forceY) * Time.deltaTime;
    }
}
