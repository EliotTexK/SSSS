using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NewtonianPhysics))]
public class Control : MonoBehaviour
{
    NewtonianPhysics myNewtonianPhysics;
    public float controlIntensity = 0.01f;
    void Start() {
        myNewtonianPhysics = GetComponent<NewtonianPhysics>();
    }
    void Update()
    {
        float forceX = Input.GetAxisRaw("Horizontal") * Time.deltaTime * controlIntensity;
        float forceY = Input.GetAxisRaw("Vertical") * Time.deltaTime * controlIntensity;

        myNewtonianPhysics.velocity += new Vector2(forceX, forceY) * Time.deltaTime;
    }
}
