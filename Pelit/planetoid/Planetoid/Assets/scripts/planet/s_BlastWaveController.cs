using UnityEngine;
using System.Collections;

public class s_BlastWaveController : MonoBehaviour
{
    public float explosionDelay;
    public float explosionForce;
    public float explosionRate;
    public float explosionSpeed;
    public float explosionMaxSize;
    public float currentRadius;

    bool exploded = false;
    CircleCollider2D explosionRadius;

    // Use this for initialization
    void Start ()
    {
        explosionRadius = GetComponent<CircleCollider2D>();
        //exploded = true;
    }

    // Update is called once per frame
    void Update () {
        if (explosionDelay>=Time.deltaTime)
        {
            exploded = true;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            exploded = true;
        }
    }

    void FixedUpdate()
    {
        if (exploded == true)
        {
            if (currentRadius < explosionMaxSize)
            {
                currentRadius = currentRadius + explosionRate;
            }
            else
            {
                Destroy(gameObject);
            }

            explosionRadius.radius = currentRadius;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (exploded == true)
        {
            Rigidbody2D objectInExplosionRadius = col.GetComponent<Rigidbody2D>();
            if (objectInExplosionRadius != null && (objectInExplosionRadius.gameObject.tag == "Player" || objectInExplosionRadius.gameObject.tag == "Meteor"))
            {
                if (objectInExplosionRadius.velocity.magnitude <= 0.1f)
                {
                    //TODO: Planet adds power even if player is not on the planet anymore.
                    Vector2 target = col.gameObject.transform.position;
                    Vector2 bomb = gameObject.transform.position;

                    Vector2 direction = explosionForce * (target - bomb);

                    objectInExplosionRadius.AddForce(direction);
                }
            }
        }
    }
}
