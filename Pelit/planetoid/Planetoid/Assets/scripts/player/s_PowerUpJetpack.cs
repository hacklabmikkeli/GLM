using UnityEngine;
using System.Collections;

public class s_PowerUpJetpack : MonoBehaviour {

	public float jetpackPower = 5;
	public float currentDuration = 0;
	public float durationInSeconds = 4f;

	public GameObject jetpackGameObject;
	private ParticleSystem jetpackParticles;


	private float longPressSimulationTime = 0;
	private float longPressMaxTime = 0.3f;

	public bool activateCall = false;
	public bool jetpackActive = false;

	private s_PlayerPowerUpStats statsScript;
	private s_PlayerControls playerScript;
	private Rigidbody2D playerRigidbody;

	// The position towards which the jetpack will move the player
	public Vector2 touchPosition;

	public bool invertJetpack = true;

	// Infinite jetpack for ship
	public bool infiniteJetpack = false;

	// Use this for initialization
	void Start () {
		// Find the player control script for playerstate checks
		playerScript = GetComponent<s_PlayerControls>();
		statsScript = GetComponent<s_PlayerPowerUpStats>();

		// Get stats from stats script
		/*if (statsScript != null) {
			jetpackPower = statsScript.currentJetpackPower;
			durationInSeconds = statsScript.currentJetpackDuration;
		}*/

		jetpackPower = PlayerData.current.jetpackPower;
		durationInSeconds = PlayerData.current.jetpackDuration;

		playerRigidbody = GetComponent<Rigidbody2D>();

		jetpackParticles = jetpackGameObject.GetComponent<ParticleSystem>();
		jetpackParticles.enableEmission = false;

		activateCall = false;
		jetpackActive = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		// If our jetpack is active, add force towards the direction

		/* - Get activation from playercontrols, which gets a call from controlmanager through longpressrecognizer
		 * - Get deactivation the same way
		 * - Keep updating touch point if long press is still active
		 * */

		if (activateCall) {
			// Player wants to activate jetpack powerup or has already activated it

			if (currentDuration < durationInSeconds) {
				// We still have fuel left

				if (!jetpackActive) {
					// First time activating the jetpack
					// Slower the player velocity in space to allow precise movement
					//playerRigidbody.velocity =  playerRigidbody.velocity / 2;
					// Activate the jetpack so we don't slow the player anymore
					jetpackActive = true;
					// Also activate the particle system
					jetpackParticles.enableEmission = true;
				}

				// We're activating the jetpack currently, so increase the current duration.
				if (!infiniteJetpack) currentDuration += Time.deltaTime;

				Vector3 dir = touchPosition - (Vector2)transform.position;
				dir = new Vector3(dir.x, dir.y, Camera.main.nearClipPlane).normalized; 	// Normalize the vector so the force is always constant
				//Debug.Log("Mouse screen pos: <color=#a52a2aff>" + Input.mousePosition + "</color>\n"
				//          + "ViewportToWorldPoint: <color=#a52a2aff>" + mousePos + "</color>\n"
				//          + "Transform pos: <color=#ffa500ff>" + transform.position + "</color>\n"
				//          + "Direction: <color=#000080ff>" + dir + "</color>\n");
				
				//Debug.Log("Calculate dir: " + mousePos + " - " + transform.position + " = " + dir);

				// The jetpack control can be inverted (push the player to the opposite direction)
				if (invertJetpack) dir = -dir;
				// Add movement force to player transform
				//Debug.Log(playerRigidbody.velocity.magnitude);
				//if (playerRigidbody.velocity.magnitude < 5)
				//{
					playerRigidbody.AddForce(dir * jetpackPower, ForceMode2D.Force);
				//}

				// Rotate the jetpack particles to match the direction
				jetpackGameObject.transform.rotation = Quaternion.LookRotation(-dir);


			} else {
				// We haven't got fuel to use the jetpack
				activateCall = false;
			}

		} else {
			// Player wants to disable the jetpack or something else caused the activate call to disable itself 
			// End Jetpack sequence
			jetpackActive = false;
			jetpackParticles.enableEmission = false;

		}
	}


	/// <summary>
	/// Sets the activate call for the jetpack to specified value.
	/// If true, jetpack will be activated.
	/// If false, nothing happens or jetpack will be disabled.
	/// 
	/// Also updates the wanted direction point for the jetpack.
	/// 
	/// Called for as long as the player is activating the longPressRecognizer in ControlManager.
	/// </summary>
	/// <param name="state">If set to <c>true</c> state.</param>
	/// <param name="newPos">New position in screen coordinates.</param>
	public void ActivateJetpack(bool state, Vector2 newPos) {
		activateCall = state;
		// Calculate the new point
		float zDist = -Camera.main.transform.position.z;
		touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(newPos.x,
		                                                              newPos.y,
		                                                              zDist));
	}
}
