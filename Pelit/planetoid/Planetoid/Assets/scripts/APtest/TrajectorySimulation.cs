using UnityEngine;

/// <summary>
/// Controls the Laser Sight for the player's aim
/// </summary>
public class TrajectorySimulation : MonoBehaviour
{

	//public GameObjectgi

	// Reference to the LineRenderer we will use to display the simulated path
	public LineRenderer sightLine;
	
	// Reference to a Component that holds information about fire strength, location of cannon, etc.
	public PlayerFire playerFire;
	
	// Number of segments to calculate - more gives a smoother line
	public int segmentCount = 20;
	
	// Length scale for each segment
	public float segmentScale = 1;
	
	// gameobject we're actually pointing at (may be useful for highlighting a target, etc.)
	private Collider2D _hitObject;
	public Collider2D hitObject { get { return _hitObject; } }

	public LayerMask gravityMask;

	private bool doShit = false;

	public GameObject testBall;
	private GameObject[] balls;

	void Start() {
		balls = new GameObject[segmentCount];
		for(int i = 0; i < segmentCount; i++) {
			balls[i] = Instantiate(testBall, Vector3.zero, Quaternion.identity) as GameObject;
		}
	}

	void Update() {

	}

	void FixedUpdate()
	{
		simulatePath();
	}
	
	/// <summary>
	/// Simulate the path of a launched ball.
	/// Slight errors are inherent in the numerical method used.
	/// </summary>
	void simulatePath()
	{
		Vector3[] segments = new Vector3[segmentCount];
		
		// The first line point is wherever the player's cannon, etc is
		segments[0] = playerFire.transform.position;

		Vector3 segVelocity = Vector3.zero;

		// The player can only jump when he's on a planet.
		// Get the first pull direction from the planet we are on.
/*		Collider2D firstPlanet = Physics2D.OverlapPoint(segments[0], gravityMask);
		if (firstPlanet != null) {
			Debug.Log("First point on a planet area");
			// Calculate pull direction for the planet
			Vector3 firstPlanetPullDir = firstPlanet.transform.position - segments[0];

			segVelocity = (playerFire.transform.up + firstPlanetPullDir) * playerFire.fireStrength * Time.deltaTime;
		} else {
			// Jumping in space...?
		}*/

		segVelocity = playerFire.transform.up * playerFire.fireStrength * Time.deltaTime;

		Debug.Log(segVelocity);


		// reset our hit object
		_hitObject = null;

		// Boolean to check if we have entered a gravity field
		bool isInsideGravityField = false;

		for (int i = 1; i < segmentCount; i++)
		{
			// Time it takes to traverse one segment of length segScale (careful if velocity is zero)
			float segTime = (segVelocity.sqrMagnitude != 0) ? segmentScale / segVelocity.magnitude : 0;
			
			// ORIGINAL: Add velocity from gravity for this segment's timestep
			//segVelocity = segVelocity + Physics.gravity * segTime;

			// Because there is no gravity in space, the default velocity when in space is just the segment velocity times segment time
			segVelocity = segVelocity * segTime;

			// Check to see if we're going to hit a physics object
			RaycastHit2D hit;
			if (hit = Physics2D.Raycast(segments[i - 1], segVelocity, segmentScale)) //out hit, segmentScale))
			{
				// remember who we hit
				_hitObject = hit.collider;
			
				// If we aren't inside a gravity field yet
				if (_hitObject.tag == "GravityArea" && !isInsideGravityField) {
					// set next position to the position where we hit the physics object
					segments[i] = segments[i - 1] + segVelocity.normalized * hit.distance;

					isInsideGravityField = true;

					//Debug.Log("Segment no. " + i);

				} else {
					// We are inside a gravity area
					// Apply gravity to velocity
					s_PlanetGravity gscript = _hitObject.GetComponent<s_PlanetGravity>();

					float pullPower = -gscript.pullForce * Time.deltaTime;

					// Calculate the direction we are supposed to go as a normalized vector
					Vector3 pullDir = (_hitObject.gameObject.transform.position - segments[i-1]).normalized;

					// Calculate the power to be added each frame
					float playerMass = 1;	// The player's mass should be 1 at all times. If this changes THIS BREAKS!!!!
					Vector3 pullDirWithPower = pullDir * pullPower * playerMass * segTime;

					segVelocity = segVelocity + pullDirWithPower;

					segments[i] = segments[i-1] + segVelocity;

					//segVelocity = segVelocity + pullDir * (segmentScale - hit.distance) / segVelocity.magnitude;
				}


				// Calculate pull dir

				// correct ending velocity, since we didn't actually travel an entire segment
				//segVelocity = segVelocity + pullDir * (segmentScale - hit.distance) / segVelocity.magnitude; 


				//segVelocity = segVelocity - Physics.gravity * (segmentScale - hit.distance) / segVelocity.magnitude;
				// flip the velocity to simulate a bounce
				//segVelocity = Vector3.Reflect(segVelocity, hit.normal);
				
			
				
				/*
			 * Here you could check if the object hit by the Raycast had some property - was 
			 * sticky, would cause the ball to explode, or was another ball in the air for 
			 * instance. You could then end the simulation by setting all further points to 
			 * this last point and then breaking this for loop.
			 */

/*				if (_hitObject.tag == "GravityArea") {
					// This is a gravity field, add force towards the center
					
					Vector3 pull = _hitObject.gameObject.transform.position - segments[i];
					segVelocity =  segVelocity + pullDir * segTime;
				}*/

			}
			// If our raycast hit no objects, then set the next position to the last one plus v*t
			else
			{
				segments[i] = segments[i - 1] + segVelocity * segTime;

				isInsideGravityField = false;
			}

			// DEBUG
			balls[i].transform.position = segments[i];

		}
		
		// At the end, apply our simulations to the LineRenderer
		
		// Set the colour of our path to the colour of the next ball
		Color startColor = playerFire.nextColor;
		Color endColor = startColor;
		startColor.a = 1;
		endColor.a = 0;
		sightLine.SetColors(startColor, endColor);
		
		sightLine.SetVertexCount(segmentCount);
		for (int i = 0; i < segmentCount; i++)
			sightLine.SetPosition(i, segments[i]);
	}
}

