using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(OrbitTarget))]
public class Control : MonoBehaviour
{
    OrbitTarget myOrbitTarget;
    public float controlIntensity = 0.01f;
    void Start() {
        myOrbitTarget = GetComponent<OrbitTarget>();
    }
    void Update()
    {
        float forceX = Input.GetAxisRaw("Horizontal") * Time.deltaTime * controlIntensity;
        float forceY = Input.GetAxisRaw("Vertical") * Time.deltaTime * controlIntensity;

        myOrbitTarget.velocity += new Vector2(forceX, forceY) * Time.deltaTime;
    }
}
