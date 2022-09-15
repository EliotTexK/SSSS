using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //singleton
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        // singleton
        if (Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
        }
    }
    public GameObject sunPrefab;
    // List of different planets to add (randomly)
    public List<GameObject> planetPrefabs;
    // Max distance before out-of-bounds
    public GameObject humanMothershipPrefab;
    public float maxDistance = 50f;
    // Minimum and maximum for deciding how far to space planets
    private LineRenderer lineRenderer;
    public Material lineRendererMaterial;
    public float minPlanetSpacing = 6f;
    public float maxPlanetSpacing = 11f;
    // Update this to false so that players can take aim
    public bool updatePhysics = true;
    public float turnLength = 5f;
    public float turnTimer = 0;
    void Start()
    {
        // Add in the sun and some planets
        GameObject sun = Instantiate(sunPrefab);
        for (float i = 2f * Random.Range(minPlanetSpacing, maxPlanetSpacing);
             i < maxDistance * 0.85f; i += Random.Range(minPlanetSpacing, maxPlanetSpacing))
        {
            // give the planets random scale and position (within boundaries)
            GameObject planet = Instantiate(planetPrefabs[Random.Range(0, planetPrefabs.Count)]);
            planet.transform.position = new Vector2(Random.value * 2f - 1f, Random.value * 2f - 1f).normalized * i;
            planet.transform.localScale += Vector3.one * Random.Range(-0.25f, 0.5f);
            planet.GetComponent<OrbitTarget>().target = sun.transform;
        }
        Vector2 randV2 = new Vector2(Random.value * 2f - 1f, Random.value * 2f - 1f).normalized;
        float placement = Random.Range(maxDistance * 0.25f, maxDistance * 0.75f);
        GameObject hMotherShip = GameObject.Instantiate(humanMothershipPrefab, randV2 * placement, Quaternion.identity);
        hMotherShip.GetComponent<OrbitTarget>().target = sun.transform;
        // Add radius
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = lineRendererMaterial;
        lineRenderer.startColor = Color.blue;
        lineRenderer.startWidth = 0.75f;
        lineRenderer.positionCount = 100;
        {
            float theta_scale = 0.02f;  // Circle resolution
            float theta = 0f;
            for(int i = 0; i < 100; i++){          
                theta += (2.0f * Mathf.PI * theta_scale);         
                float x = maxDistance * Mathf.Cos(theta);
                float y = maxDistance * Mathf.Sin(theta);          
                x += gameObject.transform.position.x;
                y += gameObject.transform.position.y;
                var pos = new Vector3(x, y, 0);
                lineRenderer.SetPosition(i, pos);
            }
        }
    }

    void OnGUI()
    {

        //GUI.Box(new Rect(0, 0, 100, 50), player1.name);
        //GUI.Box(new Rect(Screen.width - 100, 0, 100, 50), player2.name);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // wait turns
        // if (updatePhysics) {
        //     turnTimer -= Time.fixedDeltaTime;
        //     if (turnTimer <= 0) {
        //         updatePhysics = false;
        //         turnTimer = turnLength;
        //     }
        // }
    }
}
