using UnityEngine;
using System.Collections;

public class s_PinchZoom : MonoBehaviour
{
	//Variables for orthographic camera
	public float pinchZoomSpeedOrthographic;
	public float maxZoomDistanceOrthografic;
	public float minZoomDistanceOrthografic;

	//Variables for perspective camera
	public float pinchZoomSpeedPerspective;
	public float maxZoomDistancePerspective;
	public float minZoomDistancePerspective;

	public void ZoomCamera(float deltaScale)
	{
		if (Camera.main.orthographic == true)
		{
			Camera.main.orthographicSize += deltaScale * pinchZoomSpeedOrthographic;
			
			Camera.main.orthographicSize = Mathf.Max(Camera.main.orthographicSize, maxZoomDistanceOrthografic);
			Camera.main.orthographicSize = Mathf.Min(Camera.main.orthographicSize, minZoomDistanceOrthografic);
		}
		else
		{
			float xPos = Camera.main.transform.position.x;
			float yPos = Camera.main.transform.position.y;
			float zPos = Camera.main.transform.position.z + deltaScale * pinchZoomSpeedPerspective;
			
			zPos = Mathf.Max(zPos, maxZoomDistancePerspective);
			zPos = Mathf.Min(zPos, minZoomDistancePerspective);
			
			Camera.main.transform.position = new Vector3(xPos, yPos, zPos);
		}
	}
}