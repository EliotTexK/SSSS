using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DrawCircle))]
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
    public GameObject humanMothershipPrefab, alienMothershipPrefab, nukePrefab;
    public float maxDistance = 50f;
    // Minimum and maximum for deciding how far to space planets
    public Material lineRendererMaterial;
    public float minPlanetSpacing = 6f;
    public float maxPlanetSpacing = 11f;
    // Update this to false so that players can take aim
    public bool updatePhysics = true;
    public float turnLength = 5f;
    public float turnTimer = 0;
    public bool isHumanTurn;
    public GameObject alienMothership, humanMothership;
    public float alienEnergy, humanEnergy;
    public GameObject controlledUnit;
    void Start()
    {
        // randomly decide who goes first
        isHumanTurn = (Random.Range(0, 2) == 1);
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
        // add the motherships
        Vector2 randV2 = new Vector2(Random.value * 2f - 1f, Random.value * 2f - 1f).normalized;
        if (humanMothershipPrefab)
        {
            float placement = Random.Range(maxDistance * 0.25f, maxDistance * 0.75f);
            humanMothership = GameObject.Instantiate(humanMothershipPrefab, randV2 * placement, Quaternion.identity);
            humanMothership.GetComponent<OrbitTarget>().target = sun.transform;
        }
        if (alienMothershipPrefab)
        {
            float placement = Random.Range(maxDistance * 0.25f, maxDistance * 0.75f);
            alienMothership = GameObject.Instantiate(alienMothershipPrefab, -randV2 * placement, Quaternion.identity);
            alienMothership.GetComponent<OrbitTarget>().target = sun.transform;
        }
        // draw circle around game bounds
        DrawCircle drawCircle = GetComponent<DrawCircle>();
        drawCircle.radius = maxDistance;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // wait turns
        if (updatePhysics) {
            turnTimer -= Time.fixedDeltaTime;
            if (turnTimer <= 0) {
                isHumanTurn = !isHumanTurn; // switch turns
                updatePhysics = false;      // pause time for decision-making
                turnTimer = turnLength;     // reset turn timer
                // give control to the correct mothership unit
                if (isHumanTurn) controlledUnit = humanMothership;
                else controlledUnit = alienMothership;
            }
        }
    }
    public void summonUnit(int type, Vector2 prospectiveCoords) {
        ControlMothership motherShip;
        if (GameManager.Instance.isHumanTurn) {
            motherShip = humanMothership.GetComponent<ControlMothership>();
        } else {
            motherShip = alienMothership.GetComponent<ControlMothership>();
        }
        if (((Vector2) motherShip.transform.position - prospectiveCoords).magnitude < motherShip.summoningRadius) {
            motherShip.addUnit(type, prospectiveCoords);
        }
    }

    public void upgradeWeapon() {
        ControlMothership motherShip;
        if (GameManager.Instance.isHumanTurn) {
            motherShip = humanMothership.GetComponent<ControlMothership>();
        } else {
            motherShip = alienMothership.GetComponent<ControlMothership>();
        }
        motherShip.bulletPrefab = nukePrefab;
    }
}
