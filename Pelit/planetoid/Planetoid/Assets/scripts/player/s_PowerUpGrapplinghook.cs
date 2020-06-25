using UnityEngine;
using System.Collections;

// Requires a distance joint for the ghook to work
[RequireComponent (typeof (SpringJoint2D))]
public class s_PowerUpGrapplinghook : MonoBehaviour {
	
	private s_PlayerControls playerScript;
	private Rigidbody2D playerRigidbody;
	private SpringJoint2D hookJoint;

	public float maxRopeDistance = 5f;
	public int hookUsesAmount = 5;

	// The child object on the player that draws the rope line
	public GameObject ropeObject;
	private LineRenderer ropeObjectLineRenderer;
	// The 3D hook object
	public GameObject ropeRenderer;

	private GameObject debugCircle;

	// All the layers that can be grabbed
	public LayerMask hookableLayers;

	// The point we are currently hooked to
	public Vector2 hookPoint;
	public Vector2 anchorLocalHookPoint;

	// Script components
	private s_SoundScript soundScript;
	private s_PlayerPowerUpStats statsScript;

	// Check for hooked state
	public bool hooked = false;

	// Use this for initialization
	void Start () {
		// Find the player control script for playerstate checks
		playerScript = GetComponent<s_PlayerControls>();

		// Get player components
		playerRigidbody = GetComponent<Rigidbody2D>();
		hookJoint = GetComponent<SpringJoint2D>();
		hookJoint.enabled = false;	// Disable the distance joint on start

		// Find the rope line renderer
		if (ropeObject != null)
			ropeObjectLineRenderer = ropeObject.GetComponent<LineRenderer>();

		soundScript = s_SoundScript.audioSource;
		statsScript = GetComponent<s_PlayerPowerUpStats>();

		if (statsScript != null) {
			maxRopeDistance = statsScript.currentGhookRange;
			hookUsesAmount = statsScript.currentGhookUses;
		}
	}
	
	// Update is called once per frame
	void Update () {

		// If we are currently hooked to something, update the line renderer
		if (hooked == true) {
			UpdateLineRenderer(transform.position, anchorLocalHookPoint);
			UpdateRopeDistance(transform.position, playerRigidbody.velocity);
			UpdateHookPoint();
		}
	}

	public void LaunchHook(Vector2 pointOnScreen) {
		if (!hooked) {
			// If we are currently not hooked to anything
			if (playerScript.state == PlayerState.Flying || playerScript.state == PlayerState.InSpace) {
				// Cast a ray at touch pos, check if it hits an object that can be grabbed
				ScreenToWorldRay(pointOnScreen);
			}
		} else {
			// We are currently hooked, disable the hook with tap input.
			DisableHook();
		}
	}

	/// <summary>
	/// Updates the end hook point to match the new rotation and position of the hooked object.
	/// </summary>
	public void UpdateHookPoint() {
		// Set the anchor point (which is local to the hit object) to the calculated point
		hookJoint.connectedAnchor =  anchorLocalHookPoint;
	}

	/// <summary>
	/// Cast a ray from the mouse to the target object
	/// Then if that ray hits, cast a ray from player towards the hit point and check for hookable objects in the path
	/// </summary>
	public void ScreenToWorldRay(Vector2 pointOnScreen){
		//Vector3 mousePosition = Input.mousePosition;
		float zDist = -Camera.main.transform.position.z;
		Vector2 v = Camera.main.ScreenToWorldPoint(new Vector3(pointOnScreen.x, pointOnScreen.y, zDist));

		// Check for anything hookable in the area of the press
		Collider2D col = Physics2D.OverlapPoint(v, hookableLayers);

		if(col != null){
			if (hookUsesAmount > 0) {
				// Find the tapped object position
				Vector3 objectPos = col.transform.position;
				Vector3 dir = objectPos - transform.position;

				// Shoot a ray from the player with max rope distance
				RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, maxRopeDistance, hookableLayers);
				if (hit.collider != null && hooked == false) {
					// Our ray hit something on the way to the pressed point and we haven't hooked yet
					//Debug.Log("Raycast in area: " + hit.collider.gameObject.name);

					// Enable the distance joint
					EnableHook(hit);

					// Enable the rope line renderer for the hook
					EnableLineRenderer(transform.position, hit.point);

					// Save the hook point for distance calculation
					hookPoint = hit.point;
					UpdateRopeDistance(transform.position, playerRigidbody.velocity);

					// Remove one use from the hook
					hookUsesAmount--;
				}
			}
		}
	}

	/// <summary>
	/// Enables the 'physical' hook, which is the spring joint component.
	/// </summary>
	/// <param name="hit">The raycast2D hit information.</param>
	private void EnableHook(RaycastHit2D hit) {
		// Hook to the object
		Transform t = hit.transform;
		hookJoint.connectedBody = hit.collider.gameObject.GetComponent<Rigidbody2D>();

		// Set the joint connected anchor point to the raycast hit point
		// Get the hit point as a local point of the hit object
		anchorLocalHookPoint = t.InverseTransformPoint(hit.point);
		// Set the anchor point (which is local to the hit object) to the calculated point
		hookJoint.connectedAnchor =  anchorLocalHookPoint;
		 
		// DEBUG: Instantiate a circle at the hit point
		//Instantiate(debugCircle, hit.point, Quaternion.identity);

		// And set the distance to match the current distance from the planet
		hookJoint.distance = Vector2.Distance(transform.position, hit.point);
		//Debug.Log("Rope distance: " + hookJoint.distance);

		// Enable the joint
		hookJoint.enabled = true;
		hooked = true; 

		// TODO: Play ghook sound
		soundScript.PlaySoundAtPosition(soundScript.grapplingHookSound, transform.position);

	}

	/// <summary>
	/// Disables the 'physical' hook, which is the distance joint component.
	/// </summary>
	private void DisableHook() {
		hooked = false;
		// Reset the whole hookJoint
		hookJoint.connectedAnchor = Vector2.zero;
		hookJoint.anchor = Vector2.zero;
		hookJoint.distance = 0;
		hookJoint.connectedBody = null;
		hookJoint.enabled = false;

		hookPoint = Vector2.zero;

		// Also disable the rope line renderer
		DisableLineRenderer();
	}

	/// <summary>
	/// Enables the line renderer.
	/// </summary>
	/// <param name="startPos">Start position of the line renderer.</param>
	/// <param name="endPos">End position of the line renderer.</param>
	private void EnableLineRenderer (Vector2 startPos, Vector2 endPos) {
		//Debug.Log("Enabling line: " + startPos + " and " + endPos);
		ropeObjectLineRenderer.SetPosition(0, startPos);
		ropeObjectLineRenderer.SetPosition(1, endPos);
		ropeObjectLineRenderer.enabled = true;
	}

	/// <summary>
	/// Updates the line renderer start position to match that of the player's.
	/// </summary>
	/// <param name="newPos">The new position.</param>
	private void UpdateLineRenderer(Vector2 newPos, Vector2 newEndPos) {
		newEndPos = hookJoint.connectedBody.transform.TransformPoint(newEndPos);
		ropeObjectLineRenderer.SetPosition(0, newPos);
		ropeObjectLineRenderer.SetPosition(1, newEndPos);
	}

	/// <summary>
	/// Updates the rope distance according to player velocity.
	/// </summary>
	/// <param name="currentPos">Current player position.</param>
	/// <param name="currentVelocity">Current player velocity.</param>
	private void UpdateRopeDistance(Vector2 currentPos, Vector2 currentVelocity) {
		Vector2 futurePos = currentPos + (currentVelocity) * Time.deltaTime;

		float distance = Vector2.Distance(futurePos, hookPoint);
		//Debug.Log("Player pos: " + currentPos + "\nVelocity: " + currentVelocity + "\nFuturePos: " + futurePos + "\nAnchor point: " + hookPoint + "\nCalculated distance: " + distance);

		// If we're getting closer, the rope lenght should decrease.
		if (distance <= hookJoint.distance)
			hookJoint.distance = distance;
	}

	/// <summary>
	/// Disables the line renderer component.
	/// </summary>
	private void DisableLineRenderer () {
		ropeObjectLineRenderer.SetPosition(0, Vector2.zero);
		ropeObjectLineRenderer.SetPosition(1, Vector2.zero);
		ropeObjectLineRenderer.enabled = false;
	}

	void OnCollisionEnter2D (Collision2D col) {
		// If player enters any collision during a roping sequence, disable the rope
		if (hooked) {
			DisableHook();
		}
	}


/*	public void OnGUI() {
		Vector2 futurePos = (Vector2)transform.position + playerRigidbody.velocity * Time.deltaTime;
		GUI.Label(new Rect(0, 0, 200, 24), "PlayerPos: " + transform.position);
		GUI.Label(new Rect(0, 24, 200, 24), "Future pos: " + futurePos);
		GUI.Label(new Rect(0, 24*2, 200, 24), "Velocity: " + playerRigidbody.velocity);
		GUI.Label(new Rect(0, 24*3, 200, 24), "HookPoint: " + hookPoint);
		GUI.Label(new Rect(0, 24*5, 200, 24), "Distance to hookPoint: " + Vector2.Distance(transform.position, hookPoint) + "\n");

	}*/
	
	
}
