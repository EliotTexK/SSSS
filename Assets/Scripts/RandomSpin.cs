using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RandomSpin : MonoBehaviour
{
    public Vector3 direction;
    public MeshRenderer toSpin;
    // Start is called before the first frame update
    void Start()
    {
        direction = new Vector3(Random.value * 2f - 1f, Random.value * 2f - 1f,
            Random.value * 2f - 1f) * 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.updatePhysics) {
            toSpin.transform.Rotate(direction);
        }
    }
}
