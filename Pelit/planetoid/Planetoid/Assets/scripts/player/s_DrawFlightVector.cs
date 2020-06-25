using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (LineRenderer))]
public class s_DrawFlightVector : MonoBehaviour {

	public Transform startPointTransform;
	public Vector2 startPoint;

	private LineRenderer vectorLineRenderer;

	public int lineRendererPointsAmount = 20;

	public float launchPower;

	public Vector2 currentGravity;

	// Use this for initialization
	void Start () {
		currentGravity = Vector2.zero;	// Currently we are not inside a gravity area
		startPoint = startPointTransform.position;
		vectorLineRenderer = GetComponent<LineRenderer>();
		vectorLineRenderer.SetVertexCount(lineRendererPointsAmount);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.R)) Application.LoadLevel(Application.loadedLevel);

		if (Input.GetMouseButtonDown(0)) {
			Vector2 mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			//Debug.Log("Mouse Position in world coordinates: " + mouseWorldPoint);

			Vector2 direction = CalculateLaunchDirection(mouseWorldPoint, launchPower);
//			DrawLine(startPoint, direction.normalized);

			//UpdateTrajectory(transform.position, direction, launchPower, 0.1f, 100.0f);

			// Launch the player towards the direction with launchPower
			Rigidbody2D rb = GetComponent<Rigidbody2D>();
			rb.isKinematic = false;
			rb.AddForce(direction * launchPower);
		}
	}

	public Vector2 CalculateLaunchDirection (Vector2 point, float power) {
		Vector2 dir = (point - startPoint);
		Debug.Log("Launch dir: " + dir);
		return dir;
	}


	public void DrawLine (Vector2 startPos, Vector2 direction, float power, float timeStep, float maxDistance) {

		// Save calculated positions into a list
		List<Vector2> positions = new List<Vector2>();

		Vector2 lastPos = startPos; // Set start position to the current position
		Vector2 currentPos = startPos;	// Current position is also the current position

		positions.Add(startPos);	// Add the start position to the list

		float curTraveledDistance = 0;	// Keep in memory the traveled distance

		// While we are still allowed to travel
		while (curTraveledDistance < maxDistance) {
			curTraveledDistance += power * timeStep;	// Increase traveled distance by time step and power, because this is how far we'll move during one timestep


		}
		for (int i = 0; i < lineRendererPointsAmount; i++) {
			Debug.Log("Drawing at " + direction * i);
			Vector2 pos = direction * i - Physics2D.gravity;
			vectorLineRenderer.SetPosition(i, pos);

			// Add new positions


			lastPos = currentPos;	// The last position is our current position
			currentPos = positions[positions.Count - 1];	// Currently we are on the last saved position
			direction = currentPos - lastPos;	// Save new direction for next iteration
			direction.Normalize();
		}
	}

	void UpdateTrajectory(Vector2 startPos, Vector2 direction, float speed, float timePerSegmentInSeconds, float maxTravelDistance)
	{
		List<Vector2> positions = new List<Vector2>();
		Vector2 lastPos = startPos;
		Vector2 currentPos = transform.position;

		positions.Add(startPos);
		
		var traveledDistance = 0.0f;
		while(traveledDistance < maxTravelDistance)
		{
			traveledDistance += speed * timePerSegmentInSeconds;
			var hasHitSomething = TravelTrajectorySegment(currentPos, direction, speed, timePerSegmentInSeconds, positions);
			if (hasHitSomething)
			{
				break;
			}
			lastPos = currentPos;
			currentPos = positions[positions.Count - 1];
			direction = currentPos - lastPos;
			direction.Normalize();
		}
		
		BuildTrajectoryLine(positions);
	}
	
	bool TravelTrajectorySegment(Vector2 startPos, Vector2 direction, float speed, float timePerSegmentInSeconds, List<Vector2> positions)
	{
		Vector2 newPos = startPos + direction * speed * timePerSegmentInSeconds + Physics2D.gravity * timePerSegmentInSeconds;
		
		RaycastHit hitInfo;
		var hasHitSomething = Physics.Linecast(startPos, newPos, out hitInfo);
		if (hasHitSomething)
		{
			newPos = hitInfo.point;
		}
		positions.Add(newPos);
		
		return hasHitSomething;
	}
	
	void BuildTrajectoryLine(List<Vector2> positions)
	{
		vectorLineRenderer.SetVertexCount(positions.Count);
		for (var i = 0; i < positions.Count; ++i)
		{
			vectorLineRenderer.SetPosition(i, positions[i]);
		}
	}
	
}
