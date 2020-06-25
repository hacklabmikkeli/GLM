using UnityEngine;
using System.Collections;

public class s_StartCameraZoom : MonoBehaviour
{
	public float defaultCameraDistance;
	public float cameraZoomSpeed;
	public float startDelay;

	private s_GameController gameControllerScript;
	private GameObject boundariesGameObject;
	private Transform boundaries;
	private Transform player;
	public bool isZooming = true;
	private bool startZoom = false;
	
	void Start()
	{	
		gameControllerScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<s_GameController>();
		player = GameObject.FindGameObjectWithTag("Player").transform;
		boundariesGameObject = GameObject.FindGameObjectWithTag("CameraBoundaries");

		if (boundariesGameObject != null && s_GameController.allowStartZoom == true)
		{
			boundaries = GameObject.FindGameObjectWithTag("CameraBoundaries").transform;
			SetupInitialCameraDistance();
			StartCoroutine("BeginStartZoom");
		}
		else
		{
			StopStartZoom();
			gameControllerScript.ChangeGameState(GameState.PlayingLevel);
		}
	}
	
	void Update()
	{
		if (/*Time.timeSinceLevelLoad > startDelay*/ startZoom == true)
		{
			if (Camera.main.orthographic != true)
			{
				if (isZooming == true)
				{
					float xPos = player.position.x;
					float yPos = player.position.y;
					float zPos = defaultCameraDistance;
					Vector3 target = new Vector3(xPos, yPos, zPos);
					transform.position = Vector3.MoveTowards(transform.position, target, cameraZoomSpeed * Time.deltaTime);

					if (transform.position == target)
					{
						isZooming = false;
						gameControllerScript.ChangeGameState(GameState.PlayingLevel);					
					}
				}
			}
		}
	}

	void SetupInitialCameraDistance()
	{
		/*
		 * This checks cameraBoundaries collider size and and setups the
		 * camera so that the cameraBoundaries fits the screen completely
		 * before start zoom in.
		*/

		//VERTICAL
		float halfBoundaryY = boundaries.GetComponent<BoxCollider2D>().size.y / 2;
		float halfFOV = Camera.main.fieldOfView / 2;
		float FOVInRad = Mathf.Deg2Rad * halfFOV;
		float zPosVertical = halfBoundaryY / Mathf.Tan(FOVInRad);
		
		//HORIZONTAL
		float halfBoundaryX = boundaries.GetComponent<BoxCollider2D>().size.x / 2;
		float halfFOVHorizontal = (Camera.main.fieldOfView / 2) * Camera.main.aspect;
		float FOVInRadHorizontal = Mathf.Deg2Rad * halfFOVHorizontal;
		float zPosHorizontal = halfBoundaryX / Mathf.Tan(FOVInRadHorizontal);
		
		float zPos = 0;
		
		if (zPosVertical > zPosHorizontal)
		{
			zPos = zPosVertical;
		}
		else
		{
			zPos = zPosHorizontal;
		}
		
		transform.position = new Vector3(boundaries.position.x, boundaries.position.y, -zPos);
	}

	public IEnumerator BeginStartZoom()
	{
		yield return new WaitForSeconds(startDelay);
		startZoom = true;
		s_GameController.allowStartZoom = false;
	}
	
	public void StopStartZoom()
	{
		if (isZooming == true)
		{
			isZooming = false;

			float xPos = player.transform.position.x;
			float yPos = player.transform.position.y;
			float zPos = defaultCameraDistance;
			
			Camera.main.transform.position = new Vector3(xPos, yPos, zPos);

			//gameControllerScript.ChangeGameState(GameState.PlayingLevel);
		}
	}
}
