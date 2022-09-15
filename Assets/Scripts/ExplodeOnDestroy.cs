using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeOnDestroy : MonoBehaviour
{
    public GameObject explosionPrefab;
    public float size;
    // Start is called before the first frame update
    void OnDestroy() {
        if (gameObject.scene.isLoaded) {
            var e = GameObject.Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            e.transform.localScale = Vector3.one * size;
        }
    }
}
