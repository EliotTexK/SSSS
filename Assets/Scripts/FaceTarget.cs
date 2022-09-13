using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Don't apply this directly to an object that has the OrbitTarget script on it.
// You will end up rotating its whole movement axis. Apply it to a sub-object.
public class FaceTarget : MonoBehaviour
{
    public Vector3 direction;
    public Transform toFace;
    void Start()
    {
        direction = new Vector3(Random.value * 2f - 1f, Random.value * 2f - 1f,
            Random.value * 2f - 1f) * 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.updatePhysics) {
            transform.LookAt(toFace);
        }
    }
}
