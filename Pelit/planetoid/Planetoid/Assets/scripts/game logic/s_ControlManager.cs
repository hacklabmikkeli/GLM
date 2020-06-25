using UnityEngine;
using System.Collections;

/// <summary>
/// S_ control manager.
/// Handles the touch input received during scene gameplay
/// and sends the right calls to the right gameobjects.
/// 
/// This includes:
/// - The main player GameObject
/// - The main camera
/// 
/// </summary>

// Control state enum for handling touch input
public enum ControlState {none, tap, pinch, longPress, drag, touchPad, pan};

public class s_ControlManager : MonoBehaviour {

	// Static reference to script
	public static s_ControlManager controlScript;

	// Other scripts
	public s_GameController gameControllerScript;

	public bool allowInput = false;

	// Needed gameobject references
	// Player
	public GameObject playerGameObject;
	private s_PlayerControls playerScript;
	// Camera
	public GameObject mainCamera;
	private s_StartCameraZoom cameraScript;
	private s_PinchZoom pinchZoomScript;
	private s_PanCamera panCameraScript;
	private s_CameraFollow cameraFollowScript;

	// Enum references
	public PlayerState currentPlayerState;
	public ControlState currentControlState;
	
	// Animation curve for the touch pad control
	public AnimationCurve touchPadInputCurve = AnimationCurve.Linear( 0.0f, 0.0f, 1.0f, 1.0f );

	// Check for touch pad gesture
	private bool touchPadGestureActive = false;

	// Drag control
	public Vector2 dragTouchPointV2;
	public float dragAreaWidth = 10;
	public float dragAreaHeight = 10;
	private bool useCenterOfTKScreen = false;	// Needs more testing. False = Use Whole Screen, True= Use middle point of screen
												// with dimensions from dragAreaWidth/Height.
	private bool useTKAutoScale = true;		// Use TouchKit autoscale. 
	private float screenDPIMultipier;
	public LayerMask touchAreaLayer;
	public Collider2D col = null;

	void Awake () {
		// Create the static reference for this script
		controlScript = this;
	}

	void OnEnable()
	{
		s_GameController.OnStateChange += OnStateChange;
	}

	void OnDisable()
	{
		s_GameController.OnStateChange -= OnStateChange;
	}

	void OnStateChange (GameState newState)
	{
		if (newState == GameState.FailedLevel || newState == GameState.PassedLevel || newState == GameState.PausedLevel || newState == GameState.PreparingGame || newState == GameState.RestartLevel)
		{
			RemoveAllRecognizers();
		}
		if (newState == GameState.PlayingLevel)
		{
			AddRecognizers();
		}
	}
    
    void Start () {

		useTKAutoScale = TouchKit.instance.autoScaleRectsAndDistances;

		// Find the gamecontroller object
		GameObject gController = GameObject.FindGameObjectWithTag("GameController");
		// Find gamecontroller script
		gameControllerScript = gController.GetComponent<s_GameController>();
		// Find the player gameobject from the current scene
		playerGameObject = GameObject.FindGameObjectWithTag("Player");
		// Get the playercontrols script component

		if (playerGameObject != null)
		{
			playerScript = playerGameObject.GetComponent<s_PlayerControls>();
		}


		// Find main camera
		mainCamera = Camera.main.gameObject;
		cameraScript = mainCamera.GetComponent<s_StartCameraZoom>();
		pinchZoomScript = mainCamera.GetComponent<s_PinchZoom>();
		panCameraScript = mainCamera.GetComponent<s_PanCamera>();
		cameraFollowScript = mainCamera.GetComponent<s_CameraFollow>();

		// Add all the touch recognizers
		AddRecognizers();


	}
	
	
	// Handles all the input received
	void Update () {
		// DEBUG
		if (Input.GetKeyDown(KeyCode.R)) {
			gameControllerScript.ResetLevel();
		}

		if (Input.GetKeyDown(KeyCode.T)) {
			RemoveAllRecognizers();
		}

		if (playerScript == null && playerGameObject != null) {
			playerGameObject.GetComponent<s_PlayerControls>();
		}

		// Debug for data saving
		if (Input.GetKeyDown(KeyCode.S)) {
			// Find gamecontroller data saving script
			//gameControllerScript.dataSaving.Save();
			SaveLoadData.Save();
		}
	}

	/// <summary>
	/// Updates the state saved in the ControlManager.
	/// </summary>
	/// <param name="newState">New state.</param>
	public void UpdatePlayerState (PlayerState newState) {
		this.currentPlayerState = newState;
	}

	/// <summary>
	/// Changes the state of the input.
	/// </summary>
	/// <param name="allowed">If set to <c>true</c> allowed.</param>
	public void ChangeInputState (bool allowed) {
		this.allowInput = allowed;
	}

	/// <summary>
	/// Changes the state of the current input.
	/// </summary>
	/// <param name="newState">New state.</param>
	private void ChangeControlState (ControlState newState) {
		this.currentControlState = newState;
	}

	/// <summary>
	/// Removes all Touchkit recognizers.
	/// Use this to disable player input during gameplay.
	/// </summary>
	public void RemoveAllRecognizers () {
		TouchKit.removeAllGestureRecognizers();
		Destroy(GameObject.Find("TouchKit"));
	}

	/// <summary>
	/// Adds the tap recognizers.
	/// Call this to allow player input during gameplay.
	/// </summary>
	public void AddRecognizers () {

		// Remove all recognizers first if any exist
		// This way reloading a scene doesn't cause an error
		TouchKit.removeAllGestureRecognizers();

		// Create all wanted recognizers
	

		//  ---------- TAP RECOGNIZER ---------- 
		var tapRecognizer = new TKTapRecognizer();

		tapRecognizer.gestureRecognizedEvent += ( r ) =>
		{
			//Debug.Log( "tap recognizer fired: " + r );
			if (gameControllerScript.CurrentGameState() == GameState.PreparingGame)
			{
				// TÄTÄ EI KUTSUTA KOSKA BUGI
				gameControllerScript.ChangeGameState(GameState.PlayingLevel);
				cameraScript.StopStartZoom();
			}

			if (playerScript != null && playerScript.CheckPlayerState() != PlayerState.Grounded)
			{
				cameraFollowScript.StartFollowing();
			}

			if (gameControllerScript.CurrentGameState() == GameState.PlayingLevel) {
				Vector2 touchLocation = tapRecognizer.touchLocation();
				playerScript.CallGrapplingHook(touchLocation);
			}
		};
		TouchKit.addGestureRecognizer( tapRecognizer );
		//  ------------------------------------- 



		//  ---------- LONG PRESS RECOGNIZER ---------- 
		var longPressRecognizer = new TKLongPressRecognizer();
		longPressRecognizer.allowableMovementCm = 100;	// Allow movement through the whole screen, as jetpack controls require this
		longPressRecognizer.gestureRecognizedEvent += ( r ) =>
		{
			if (gameControllerScript.CurrentGameState() == GameState.PlayingLevel)
			{
				if (CheckControlState(ControlState.none) && (playerScript.CheckPlayerState() == PlayerState.Flying || playerScript.CheckPlayerState() == PlayerState.InSpace))
					ChangeControlState(ControlState.longPress);
				
				if (CheckControlState(ControlState.longPress)) {
				// Call the player script to activate the jetpack procedure
				Vector2 touchLocation = longPressRecognizer.touchLocation();
				//Debug.Log("Starting jetpack");
				playerScript.CallJetpack(true, touchLocation);
				}
				
				//Debug.Log( "long press recognizer fired: " + r );
			}
		};
		longPressRecognizer.gestureCompleteEvent += ( r ) =>
		{
			if (gameControllerScript.CurrentGameState() == GameState.PlayingLevel)
			{
				// Disable the jetpack
				playerScript.CallJetpack(false, Vector2.zero);
				if (CheckControlState(ControlState.longPress))
					ChangeControlState(ControlState.none);
				//Debug.Log( "long press recognizer finished: " + r );
			}
		};
		TouchKit.addGestureRecognizer( longPressRecognizer );
		//  ------------------------------------- 




		// ---------- PINCH RECOGNIZER ----------
		var pinchRecognizer = new TKPinchRecognizer();
		pinchRecognizer.gestureRecognizedEvent += ( r ) =>
		{
			if (gameControllerScript.CurrentGameState() == GameState.PlayingLevel)
			{
				//Debug.Log( "pinch recognizer fired: " + r );
				
				// Check the current control state, don't start the gesture if we have other input coming in
				if (CheckControlState(ControlState.none)) {
					ChangeControlState(ControlState.pinch);
					//Debug.Log(pinchRecognizer.deltaScale);
				} else if (CheckControlState(ControlState.pinch)) {
					//cameraScript.StopStartZoom();
					pinchZoomScript.ZoomCamera(pinchRecognizer.deltaScale);
				}
			}
		};
		pinchRecognizer.gestureCompleteEvent += r =>
		{
			if (gameControllerScript.CurrentGameState() == GameState.PlayingLevel)
			{
				if (CheckControlState(ControlState.pinch))
					ChangeControlState(ControlState.none);
				//Debug.Log( "touchpad gesture complete" );
			}
		};
		
		TouchKit.addGestureRecognizer( pinchRecognizer );
		//  ------------------------------------- 



		// ---------- DRAG RECOGNIZER ---------- 
		dragTouchPointV2 = Vector2.zero;

		if (useCenterOfTKScreen) { // If TouchKit has autoScaleRectsAndDistances = true, then this will work to some extent
			if (useTKAutoScale) {
				// Automated scale, count with TK rects
				dragTouchPointV2 = new Vector2( 320 / 2, 180 / 2);
			} else {
				// Non-automated scale, use screen width
				dragTouchPointV2 = new Vector2(Screen.width / 2, Screen.height / 2);
			}
		} else {
			// Use whole screen
			dragTouchPointV2 = new Vector2(Screen.width / 2, Screen.height / 2);

			// Crazy huge area for input
			dragAreaWidth = 10000;
			dragAreaHeight = 10000;

		}
		// Create the recognizer
		var dragRecognizer = new TKTouchPadRecognizer(new TKRect (dragAreaWidth, dragAreaHeight, dragTouchPointV2));
		dragRecognizer.inputCurve = touchPadInputCurve;
		//col = null;
		
		dragRecognizer.gestureRecognizedEvent += ( r ) =>
		{
			if (gameControllerScript.CurrentGameState() == GameState.PlayingLevel)
			{
				if (playerScript.CheckPlayerState() == PlayerState.Grounded) {
					if (col == null)
					{
						Vector3 touchInWorld = Vector3.zero;
						float xPos = dragRecognizer.startTouchLocation().x;
						float yPos = dragRecognizer.startTouchLocation().y;
						float zPos = -Camera.main.transform.position.z;
						touchInWorld = Camera.main.ScreenToWorldPoint(new Vector3(xPos, yPos, zPos));
						col = Physics2D.OverlapCircle(touchInWorld, 0.1f, touchAreaLayer.value);
					}
					
					// Check the current control state
					if (CheckControlState(ControlState.none) && col != null)
					{
						ChangeControlState(ControlState.drag);
						dragTouchPointV2 = Vector2.zero;
					}
					
					if (CheckControlState(ControlState.drag))
					{
						dragTouchPointV2 = dragRecognizer.touchLocation();
						
						if (col != null && playerScript.CheckPlayerState() == PlayerState.Grounded)
						{
							if (!touchPadGestureActive)
							{
								touchPadGestureActive = true;
								// Send the world point of the touch to the player script
								playerScript.StartJumping(dragTouchPointV2);
							} 
							else 
							{
								// Update the player jump procedure
								playerScript.UpdateJumping(dragTouchPointV2);
							}
						}
					}
				}
				}


		};
		
		// continuous gestures have a complete event so that we know when they are done recognizing
		dragRecognizer.gestureCompleteEvent += r =>
		{
			if (gameControllerScript.CurrentGameState() == GameState.PlayingLevel)
			{
				if (CheckControlState(ControlState.drag))
				{
					ChangeControlState(ControlState.none);
					//Debug.Log( "touchpad gesture complete" );
					// End the jumping procedure, send the player flying!
					if (col != null)
					{
						playerScript.EndJumping(dragTouchPointV2);
						col = null;
						cameraFollowScript.StartFollowing();
					}
					dragTouchPointV2 = Vector2.zero;
					touchPadGestureActive = false;
				}
			}
		};
		TouchKit.addGestureRecognizer( dragRecognizer );
		//  ------------------------------------- 






		// ------------ PAN RECOGNIZER ------------------
		var panRecognizer = new TKPanRecognizer();
		panRecognizer.maximumNumberOfTouches = 1;
		
		// when using in conjunction with a pinch or rotation recognizer setting the min touches to 2 smoothes movement greatly
		/*if( Application.platform == RuntimePlatform.IPhonePlayer )
		{
			panRecognizer.minimumNumberOfTouches = 2;
		}*/
		
		panRecognizer.gestureRecognizedEvent += ( r ) =>
		{
			if (gameControllerScript.CurrentGameState() == GameState.PlayingLevel)
			{	
				if (CheckControlState(ControlState.none) && playerScript.CheckPlayerState() == PlayerState.Grounded)
				{
					Debug.Log("StartPan");
					cameraFollowScript.StopFollowing();
					ChangeControlState(ControlState.pan);
				}

				if (CheckControlState(ControlState.pan))
				{
					//Debug.Log("Touch Delta: " + Input.touches[0].deltaTime);
					//Debug.Log("Time Delta: " + Time.deltaTime);

					//Debug.Log( "pan recognizer fired: " + r );
					panCameraScript.PanCamera(panRecognizer.deltaTranslation);
				}
			}
			else if (gameControllerScript.CurrentGameState() == GameState.PreparingGame)
			{
				//Debug.Log("PANNING");
				Debug.Log("Delta: " + panRecognizer.deltaTranslation.y);
			}
		};
		
		// continuous gestures have a complete event so that we know when they are done recognizing
		panRecognizer.gestureCompleteEvent += r =>
		{
			if (gameControllerScript.CurrentGameState() == GameState.PlayingLevel)
			{
				if (CheckControlState(ControlState.pan))
				{
					//Debug.Log( "pan gesture complete" );
					ChangeControlState(ControlState.none);
				}
			}
		};
		TouchKit.addGestureRecognizer( panRecognizer );
		// ---------------------------------------------


	}

	public void ChangeInputAllowedState (bool state) {
		this.allowInput = state;
	}

	public bool CheckControlState(ControlState checkedState) {
		if (currentControlState == checkedState) return true;
		else return false;
	}

}
