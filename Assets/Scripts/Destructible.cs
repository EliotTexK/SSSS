using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// collide with other physics objects, lose health, explode!
[RequireComponent(typeof(NewtonianPhysics))]
[RequireComponent(typeof(Collider2D))]
public class Destructible : MonoBehaviour
{
    // health is a percentage
    public const float maxHealth = 100;
    public float health = maxHealth;
    private Collider2D myCollider;
    private NewtonianPhysics myPhysics;
    public GameObject damageExplosion;
    public GUIStyle healthBarStyle;

    [SerializeField]
    private RectTransform HealthBar;
    [SerializeField]
    private Image HealthBarColor;

    void Start()
    {
        myCollider = GetComponent<Collider2D>();
        myPhysics = GetComponent<NewtonianPhysics>();
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.updatePhysics)
        {
            // collide with other objects
            CircleCollider2D[] collisions = new CircleCollider2D[1];
            myCollider.OverlapCollider(new ContactFilter2D(), collisions);
            if (collisions[0])
            {
                if (collisions[0].tag == "Sun") {
                    Destroy(this.gameObject);
                }
                var otherPhysics = collisions[0].gameObject.GetComponent<NewtonianPhysics>();
                if (otherPhysics)
                {
                    // lose health based on ratio of masses and the "damage multiplier"
                    health -= otherPhysics.mass * otherPhysics.damageMultiplier * Time.fixedDeltaTime;
                    Vector3 randomV3 = new Vector3(Random.value * 2f - 1f, Random.value * 2f - 1f, Random.value * 2f - 1f);
                    GameObject.Instantiate(damageExplosion, (transform.position + otherPhysics.transform.position)/2 + randomV3 * transform.localScale.magnitude/4, Quaternion.identity);
                    // too lazy to implement proper two-way collisions
                    var otherDest = collisions[0].gameObject.GetComponent<Destructible>();
                    if (otherDest) {
                        otherDest.health -= myPhysics.mass * myPhysics.damageMultiplier * Time.fixedDeltaTime;
                    }
                    // add small explosion effect
                }
            }
            if (health <= 0) {
                Destroy(this.gameObject,0.05f);
            }
            if (transform.position.magnitude > GameManager.Instance.maxDistance) {
                health -= Time.fixedDeltaTime * (transform.position.magnitude - GameManager.Instance.maxDistance);
                Vector3 randomV3 = new Vector3(Random.value * 2f - 1f, Random.value * 2f - 1f, Random.value * 2f - 1f);
                GameObject.Instantiate(damageExplosion, transform.position + randomV3 * transform.localScale.magnitude/3, Quaternion.identity);
            }

            UpdateHealthBar(health);
        }
    }
    void OnGUI()
    {
        Vector2 targetPos;
        targetPos = Camera.main.WorldToScreenPoint(transform.position * new Vector2(1f, -1f));
        //GUI.Box(new Rect(targetPos.x - 32, targetPos.y - 32, 64, 64), (Mathf.Ceil(health)).ToString());
    }

    void UpdateHealthBar(float health)
    {
        HealthBar.localScale = new Vector3(health / maxHealth, 0.63f, 1f);

        if (health <= 50)
        {
            HealthBarColor.color = Color.yellow;
        }

        if (health <= 25)
        {
            HealthBarColor.color = Color.red;
        }
        
    }
}
