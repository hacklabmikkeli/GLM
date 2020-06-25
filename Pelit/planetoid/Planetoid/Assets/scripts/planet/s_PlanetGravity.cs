using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Handles the planet gravity field effect on GameObjects.
/// 
/// </summary>

public class s_PlanetGravity : MonoBehaviour {

	public delegate void GravityAreaEvent();
	public static event GravityAreaEvent OnEventBegan;

	public List<Transform> pulledTransforms;

	private Transform pgTransform;
	private Rigidbody2D pgBody;

	public bool attract = true;
	//private bool distanceAffectsPullForce = false;	// This does not currently function properly!
	public bool useDebugMenuValues = false;
	public float distanceMultiplier = 100;
	public float pullForce = -1000f;	// Needs to be negative for the planet to pull
	public float turnSpeed = 20f;
	public Vector2 pullDir;

	public Transform ghostTransform;

	public bool useVer2 = true;

	public LayerMask whatToPull; 

	public bool hasPlayerTouched = false;

	public bool removeGhostTransform = false;

	// Update is called once per frame
	void FixedUpdate () {
		if (pulledTransforms != null && attract) {
			foreach ( Transform t in pulledTransforms ) {
				if (t != null)
				{
					PullObject(t);
				}
			}
		}

		// Handle player ghost differently
		if (ghostTransform != null) {
			PullObject(ghostTransform);
		}
	}

	void PullObject(Transform t) {
		// Set the pull direction towards the planet
		pullDir = (t.position - transform.position).normalized;
		
		// Distance can affect the pullforce.
/*		float distance = 1;
		if (distanceAffectsPullForce)
			distance = Vector3.Distance(t.position, transform.position);

		// Add force to the gameobject being pulled
		t.GetComponent<Rigidbody2D>().AddForce(pullDir * (pullForce + distance * distanceMultiplier) * Time.deltaTime);*/

		pgBody = t.GetComponent<Rigidbody2D>();
		
		pgBody.AddForce(pullDir * pullForce * pgBody.mass * Time.deltaTime);


		// Rotate the pulled gameobject
		Quaternion targetRotation = Quaternion.FromToRotation(t.up, pullDir) * t.rotation;
		Quaternion playerRotation = t.rotation;
		t.rotation = Quaternion.Slerp(playerRotation, targetRotation, turnSpeed * Time.deltaTime);
	}
	
	void OnTriggerEnter2D (Collider2D other) {

		if (other.gameObject.tag == "Player") {

			// Play sound
			s_SoundScript.audioSource.PlaySoundAtPosition(s_SoundScript.audioSource.enterGravityField, transform.position);

			//Debug.Log("Gameobject entered: " + other.gameObject.name);
			// Change the player's state to flying since we are in a gravity field
			other.gameObject.GetComponent<s_PlayerControls>().ChangePlayerState(PlayerState.Flying);

			if (hasPlayerTouched == false && OnEventBegan != null && pullForce > 0)
			{
				OnEventBegan();
				hasPlayerTouched = true;
			}

		} else if (other.gameObject.tag == "PlayerGhost") {
			// The other object is the player ghost, add it to the transform variable
			ghostTransform = other.transform;
		
		}
		if (!pulledTransforms.Contains(other.transform) && whatToPull == (whatToPull | (1 << other.gameObject.layer))) {
			// If the collided object is the player ghost object, don't add it to the list.
			// The player ghost is handled differently.
			if (other.gameObject.tag != "PlayerGhost") {
				Transform t = other.transform;
				// Add the transform to the pulled objects list
				pulledTransforms.Add(t);
			}
		}
	}

	void OnTriggerExit2D (Collider2D other) {
		// When player exits the trigger area, stop pulling the player gameobject.
		if (other.gameObject.tag == "Player") {
			// Change the player's state to InSpace since we left a gravity field
			other.gameObject.GetComponent<s_PlayerControls>().ChangePlayerState(PlayerState.InSpace);
		} else if (other.gameObject.tag == "PlayerGhost") {
			ghostTransform = null;
		}

		if (pulledTransforms.Contains(other.transform)) {
			// For every other gameobject just remove them from the list
			Transform t = other.transform;
			pulledTransforms.Remove(t);
		}
	}
}
