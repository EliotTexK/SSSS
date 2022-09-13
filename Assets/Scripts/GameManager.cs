using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject sunPrefab;
    public GameObject planetPrefab;
    public float maxPlanetDistance = 30f;

    public float minPlanetSpacing = 3f;
    public float maxPlanetSpacing = 5f;
    public static bool updatePhysics = false;
    // Start is called before the first frame update
    void Start()
    {
        // Add in the sun and some planets
        GameObject sun = Instantiate(sunPrefab);
        for (float i = maxPlanetSpacing; i < maxPlanetDistance; i += Random.Range(minPlanetSpacing, maxPlanetSpacing)) {
            // give the planets random scale and position (within boundaries)
            GameObject planet = Instantiate(planetPrefab);
            planet.transform.position = new Vector2(Random.value * 2f - 1f, Random.value * 2f - 1f).normalized * i;
            planet.transform.localScale += Vector3.one * Random.Range(-0.25f,0.5f);
            planet.GetComponent<OrbitTarget>().target = sun.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
