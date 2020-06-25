using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class s_PlayerFlightPath : MonoBehaviour {

	public GameObject flightVectorGameobject;
	private Rigidbody2D flightVectorBody;
	private Transform flightVectorTransform;
	private TrailRenderer flightVectorTrail;
	private Collider2D col;

	private float ghostMaxTime = 2f;
	private float ghostTime = 0f;
	public Vector2 lastDirection = Vector2.zero;

	[HideInInspector]
	public List<s_PlanetGravity> gravityScriptList;

	private bool ghostFlying = false;
	[Header("Do we display the ghost?")]
	public bool ghostActive = false;

	// Use this for initialization
	void Start () {
		flightVectorGameobject = transform.FindChild("PlayerGhost").gameObject;
		flightVectorTransform = flightVectorGameobject.transform;
		flightVectorBody = flightVectorGameobject.GetComponent<Rigidbody2D>();
		flightVectorTrail = flightVectorGameobject.GetComponent<TrailRenderer>();
		col = flightVectorGameobject.GetComponent<CircleCollider2D>();
		ResetPlayerGhost();

		Physics2D.IgnoreCollision(col, transform.GetComponent<CircleCollider2D>());
		Physics2D.IgnoreCollision(col, transform.GetComponent<BoxCollider2D>());

		gravityScriptList = new List<s_PlanetGravity>();

	}

	void Update () {
		if (ghostFlying) {
			ghostTime += Time.deltaTime;
			if (ghostTime >= ghostMaxTime) {
				LaunchGhost(lastDirection);
			}
		}
	}

	public void ActivateGhost() {
		// Enable the flight vector guy
		if (ghostActive) {
			flightVectorGameobject.SetActive(true);
			flightVectorTrail.enabled = true;
		}
	}

	public void LaunchGhost(Vector2 direction) {
		flightVectorTrail.time = ghostMaxTime;	// Set the time before launching
		lastDirection = direction;
		// Reset the ghost first if it is flying
		ResetPlayerGhost();

		// Enable the ghost
		ActivateGhost();
		
		if (ghostFlying == false) {
			// Launch the flight vector prefab to simulate the flight
			flightVectorBody.AddForce(direction);
			ghostFlying = true;
		}
	}

	/// <summary>
	/// Resets the player ghost, clearing all movement velocity and resetting the local position.
	/// Also disables the ghost object and resets the flying time and flying state.
	/// </summary>
	public void ResetPlayerGhost() {
		if (ghostActive) {
			TrailRendererExtensions.Reset(flightVectorTrail, this);
			flightVectorTrail.enabled = false;
			flightVectorBody.velocity = Vector2.zero;
			flightVectorGameobject.SetActive(false);
			flightVectorTransform.localPosition = Vector2.zero;
			ghostFlying = false;
			ghostTime = 0;

			// When resetting the player, set the pulled ghost object to null for each planet we've visited
			foreach (s_PlanetGravity pg in gravityScriptList) {
				//Debug.Log("<color=#ff00ffff>Removing from list</color>");
				pg.ghostTransform = null;
			}

			// Clear all the planets from the list
			gravityScriptList.Clear();
		}
	}
}


/// <summary>
/// Trail renderer extensions.
/// Handles logic for hiding the trail renderer's trail when moving it from one location to another. 
/// </summary>
public static class TrailRendererExtensions
{
	/// <summary>
	/// Reset the trail so it can be moved without streaking
	/// </summary>
	public static void Reset(this TrailRenderer trail, MonoBehaviour instance)
	{
		instance.StartCoroutine(ResetTrail(trail));   
	}
	
	/// <summary>
	/// Coroutine to reset a trail renderer trail
	/// </summary>
	/// <param name="trail"></param>
	/// <returns></returns>
	static IEnumerator ResetTrail(TrailRenderer trail)
	{
		var trailTime = trail.time;
		trail.time = 0;
		yield return new WaitForEndOfFrame();
		//Debug.Log("Setting trail time back to normal");
		trail.time = trailTime;
	}        
}


