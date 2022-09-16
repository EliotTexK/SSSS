using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Don't apply this directly to an object that has the OrbitTarget script on it.
// You will end up rotating its whole movement axis. Apply it to a sub-object.
public class FaceTarget : MonoBehaviour
{
    public Transform toFace;

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.updatePhysics) {
            transform.LookAt(toFace, Vector3.forward);
        }
    }
}
