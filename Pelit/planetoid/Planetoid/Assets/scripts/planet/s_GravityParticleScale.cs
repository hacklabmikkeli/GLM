using UnityEngine;
using System.Collections;

/// <summary>
/// S_ gravity particle scale.
/// Controls how the gravity particle effect is shown on the planet.
/// Depending on the pull direction (pulling, pushing) changes the size and behavior of the particle effect.
/// </summary>
public class s_GravityParticleScale : MonoBehaviour {

	[Tooltip("In Unity units. This is added to the size of the particle.")]
	public float addedSize;

	public GameObject particles;

	public ParticleSystem atmosphereParticles;
	public ParticleSystem pulseParticles;
	private CircleCollider2D col;

	private Vector3 colliderSize;
	private float startSize;

	public bool reversePulse = false;

	public Color particleColor;
	 
	// Use this for initialization
	void Start () {
		col = GetComponent<CircleCollider2D>();
		colliderSize = col.bounds.size;

		// Instantiate the particles
		GameObject particleGameObject = Instantiate(particles, transform.position, Quaternion.identity) as GameObject;
		particleGameObject.transform.SetParent(transform);

		// Get the particle system children
		atmosphereParticles = particleGameObject.transform.GetChild(0).GetComponent<ParticleSystem>();
		pulseParticles = particleGameObject.transform.GetChild(1).GetComponent<ParticleSystem>();

		// Set the child size & position
		atmosphereParticles.startSize = colliderSize.x + addedSize;
		particleGameObject.transform.localPosition = Vector2.zero;

		// Randomize the pulse start time a little
		float rnd = Random.Range(0f, 2f);
		pulseParticles.startDelay = rnd;


		if (!reversePulse) {
			pulseParticles.startSize = atmosphereParticles.startSize + addedSize;
		} else {
			// The gravity pulse is reversed
			pulseParticles.startSize = 0;
		}

		// Hide the sprite renderer. It is only useful when deciding the size of the planet in the editor.
		GetComponent<SpriteRenderer>().enabled = false;


		// Set the color of the particles
		atmosphereParticles.startColor = particleColor;
		pulseParticles.startColor = particleColor;
	}
}
