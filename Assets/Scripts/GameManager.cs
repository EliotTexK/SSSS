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
    public float maxDistance = 40f;
    // Minimum and maximum for deciding how far to space planets

    public float minPlanetSpacing = 3f;
    public float maxPlanetSpacing = 5f;
    // Update this to false so that players can take aim
    public bool updatePhysics = false;
    struct playerStats {
        public string name; // name your empire!
        public bool isAlien;
        public int QSB; // "quadrillion spaceBucks": the economy is very inflated
        public playerStats(string name, bool type) {
            this.name = name;
            this.isAlien = type;
            if (type) {
                QSB = 10; // alien starting money
            } else {
                QSB = 15; // human starting money
            }
        }
    }
    playerStats player1, player2;
    void Start()
    {
        player1 = new playerStats("Player 1", true);
        player2 = new playerStats("Player 2", false);
        // Add in the sun and some planets
        GameObject sun = Instantiate(sunPrefab);
        for (float i = Random.Range(minPlanetSpacing, maxPlanetSpacing);
             i < (maxDistance * 0.75); i += Random.Range(minPlanetSpacing, maxPlanetSpacing))
        {
            // give the planets random scale and position (within boundaries)
            GameObject planet = Instantiate(planetPrefabs[Random.Range(0, planetPrefabs.Count)]);
            planet.transform.position = new Vector2(Random.value * 2f - 1f, Random.value * 2f - 1f).normalized * i;
            planet.transform.localScale += Vector3.one * Random.Range(-0.25f, 0.5f);
            planet.GetComponent<OrbitTarget>().target = sun.transform;
        }
    }

    void OnGUI()
    {
        GUI.Box(new Rect(0, 0, 100, 50), player1.name);
        string QSBString = "Q$B: " + player1.QSB.ToString();
        GUI.Box(new Rect(0,50,100,50), QSBString);
        
        GUI.Box(new Rect(Screen.width - 100, 0, 100, 50), player2.name);
        QSBString = "Q$B: " + player2.QSB.ToString();
        GUI.Box(new Rect(Screen.width - 100, 50, 100, 50), QSBString);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
