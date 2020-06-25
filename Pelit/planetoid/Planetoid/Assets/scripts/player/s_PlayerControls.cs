using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

// Public player state enum
public enum PlayerState {Grounded, Flying, InSpace, Dead, Finished};

// Public death enum
public enum CauseOfDeath {None, Oxygen, Burn, Boundary, Projectile};

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent (typeof (LineRenderer))]

[RequireComponent (typeof (s_PlayerFlightPath))]
[RequireComponent (typeof (s_SoundScript))]
[RequireComponent (typeof (s_OxygenMeter))]
[RequireComponent (typeof (s_PowerUpJetpack))]
[RequireComponent (typeof (s_PowerUpGrapplinghook))]

public class s_PlayerControls : MonoBehaviour {

	//This could be used to raise and event every time gamestate is changed.
	public delegate void PlayerEvent();
	public static event PlayerEvent OnEventBegan;

	private bool gatheringJumpPower = false;
	public float jumpMultiplier;
	public float playerMaxVelocity;
	private float maxJumpForce = 400;
	public bool debugClampJumpPower = true;	// Set to false to test different jump powers. This way the jump power wont be clamped.

	public GameObject groundCheck;
	public bool grounded = false;
	public bool debugInfiniteJumps = false;
	public LayerMask groundedMask;	// The layers that cause the player to go to grounded status.

	// Flight ghost script
	public GameObject flightPathGameObject;
	private s_PlayerFlightPath flightPathScript;
	private bool useGhost = false;

	// The smoke particles emitted from the player hitting a planet
	public ParticleSystem smokeParticles;

	// Vectors for line drawing
	// Touch controls
	private Vector3 touchLastPosition;
	public Vector3 touchCurrentPosition;
	public Vector3 touchStartPosition;
	public Vector3 touchEndPosition;

	// Debug UI
	private bool useDebugUI = false;
	private Vector2 lastJump;
	private Vector2 theJumpBeforeLastJump;

	// The oxygen script
	private s_OxygenMeter oxygenScript;
	private float oxygenAmount = 5;	// The amount of oxygen to be deducted/added when in space

	// The sound manager script
	private s_SoundScript soundScript;

	// The power up scripts
	private s_PowerUpJetpack jetpackScript;
	private s_PowerUpGrapplinghook hookScript;

	// Equipped powerups
	public bool JetpackEquipped = false;
	public bool GrapplinghookEquipped = false;

	// Gamecontroller script
	private s_GameController gameControllerScript;

	// Current player state
	public PlayerState state;

	// Death state for end screen checks
	public CauseOfDeath deathState;

	// The line renderer for the jump vector
	private LineRenderer playerLineRenderer;

	// Players rigidbody
	private Rigidbody2D playerRigidbody;

	// Player animator
	private string rendererName = "Renderer";
	private Transform playerRendererTransform;	// The transform with player animator
	public s_PlayerAnimation animationScript;	// The animation script found on the renderer

	// If the player is facing right, headingRight is true.
	private bool headingRight = true;
	private Vector3 playerVelocity;

	// Use this for initialization
	void Start () {
		// Find gamecontroller
		gameControllerScript = s_GameController.gameControllerSingleton;
	
		// Find our renderer child
		playerRendererTransform = transform.FindChild(rendererName);

		// Find the audio manager
		soundScript = s_SoundScript.audioSource;
	
		// Find playercomponent scripts
		jetpackScript = GetComponent<s_PowerUpJetpack>();
		hookScript = GetComponent<s_PowerUpGrapplinghook>();
		oxygenScript = GetComponent<s_OxygenMeter>();
		flightPathScript = GetComponent<s_PlayerFlightPath>();
		animationScript = playerRendererTransform.GetComponent<s_PlayerAnimation>();	// Animation script on renderer child
		//Debug.Log(animationScript);

		// Get other components from gameobject
		playerLineRenderer = GetComponent<LineRenderer>();
		playerRigidbody = GetComponent<Rigidbody2D>();
		//playerAnimator = GetComponent<Animator>();

		
		// Set up all components
		// Player always starts alive
		playerLineRenderer.enabled = false;	// Disable the line renderer at the start
		deathState = CauseOfDeath.None;

		JetpackEquipped = PlayerData.current.jetpackEquipped;

		PlayerData.current.ClearLevelJumpCount(SceneManager.GetActiveScene().buildIndex);
	}
	
	// Update is called once per frame
	void Update () {

		CheckForGrounded();
		if (debugInfiniteJumps) grounded = true;

		bool getOxygen = false;
		bool changeHeading = false;

		switch(state) {
		case PlayerState.Grounded:
			getOxygen = true;
			break;
		case PlayerState.InSpace:
			getOxygen = false;
			changeHeading = true;
			break;
		case PlayerState.Flying:
			getOxygen = true;
			changeHeading = true;
			break;
		case PlayerState.Dead:
			getOxygen = false;
			break;
		default:
			getOxygen = true;
			break;
		}

		// Are we getting back oxygen or losing it?
		if (getOxygen) {
			oxygenScript.ChangeOxygenLevel(oxygenAmount);
		} else {
			oxygenScript.ChangeOxygenLevel(-oxygenAmount);
		}

		// Change our heading depending on our state
		if (changeHeading) {
			Vector2 vel = playerRigidbody.velocity;
			Vector2 nextPoint = (Vector2)transform.position + vel;
			ChangePlayerHeading(nextPoint);
		}

		//Debug.Log("Player velocity: " + playerRigidbody.velocity.magnitude);

		playerRigidbody.velocity = Vector2.ClampMagnitude(playerRigidbody.velocity, playerMaxVelocity);
	}

	// Starts the jumping procedure
	public void StartJumping (Vector2 touchScreenStartPoint)
	{
		// The player can only start jumping if he is grounded
		if (state == PlayerState.Grounded)
		{
			// Activate the flight path ghost
			if (useGhost && flightPathScript != null) flightPathScript.ActivateGhost();

			// Reset all of the touch positions
			touchStartPosition = Vector2.zero;
			touchCurrentPosition = Vector2.zero;
			touchEndPosition = Vector2.zero;

			Vector3 touchWorldPoint;
				
			touchWorldPoint = Camera.main.ScreenToWorldPoint(new Vector3(touchScreenStartPoint.x, touchScreenStartPoint.y, -Camera.main.transform.position.z));
				
			// Remember the start position
			touchStartPosition = touchWorldPoint;

			gatheringJumpPower = true;

			if (animationScript.CurrentAnimIsName("idle") || animationScript.CurrentAnimIsName("landing"))
			{
				animationScript.SetAnimatorTrigger("ChargeJump");
            }
		}
	}

	// Called to update the jumping procedure.
	// Draws the vector line.
	public void UpdateJumping (Vector2 touchScreenCurrentPoint)
	{
		if (state == PlayerState.Grounded)
		{
			// If we are gathering jump power and are grounded
			if (gatheringJumpPower)
			{
				touchLastPosition = touchCurrentPosition;

				// Calculate the current point of the touch
				touchCurrentPosition = Camera.main.ScreenToWorldPoint(new Vector3(touchScreenCurrentPoint.x, touchScreenCurrentPoint.y, -Camera.main.transform.position.z));

				// GHOST BLOCK
				if (useGhost && flightPathScript != null) {
					if (flightPathScript.ghostActive && touchLastPosition != touchCurrentPosition) {
						
						// Calculate the vector to send the player ghost
						Vector3 direction = touchStartPosition - touchCurrentPosition;
						Vector2 directionV2 = direction * jumpMultiplier;
						Vector2 directionV2Clamped = Vector2.ClampMagnitude(directionV2, maxJumpForce);
						
						flightPathScript.LaunchGhost(directionV2Clamped);
					}
				}

				// Draw and update the line renderer
				DrawVectorLine(touchStartPosition, touchCurrentPosition, 0);
			}
		}
	}

	public void EndJumping (Vector2 touchScreenEndPoint)
	{
		if (state == PlayerState.Grounded)
		{
			gatheringJumpPower = false;

			touchEndPosition = Camera.main.ScreenToWorldPoint(new Vector3(touchScreenEndPoint.x, touchScreenEndPoint.y, -Camera.main.transform.position.z));

			// Calculate the vector to send the player
			Vector3 direction = touchStartPosition - touchEndPosition;
			
			// Add force to the player to send them flying towards the direction
			Vector2 directionV2 = direction * jumpMultiplier;

			if (debugClampJumpPower) {
				// This is the default version of the jump. No infinite jump allowed.
				Vector2 directionV2Clamped = Vector2.ClampMagnitude(directionV2, maxJumpForce);

				if (lastJump == Vector2.zero) {
					lastJump = directionV2Clamped;
				} else {
					theJumpBeforeLastJump = lastJump;
					lastJump = directionV2Clamped;
				}

				playerRigidbody.AddForce(directionV2Clamped);
			} else {
				// This is mainly for testing purposes. Inifnite jumping power is allowed.
				playerRigidbody.AddForce(directionV2);
			}

			
			// Play the jump sound
			soundScript.PlaySoundAtPosition(soundScript.jumpSound, transform.position);
			EmitSmokeParticles();
			
			// Remove the line renderer and disable it
			playerLineRenderer.enabled = false;

			// Reset the flight path ghost
			if (useGhost && flightPathScript != null)	flightPathScript.ResetPlayerGhost();

			PlayerData.current.AddToJumpCount(Application.loadedLevel);

			if (OnEventBegan != null)
			{
				OnEventBegan();
			}

			//Debug.Log(directionV2.magnitude);

			// Handle animation
			if (directionV2.magnitude > 1f)
			{
				// If the magnitude of our jump was big enough, play the animation
				animationScript.SetAnimatorTrigger("Jump");
			}
			else
			{
				// If the magnitude was too small, don't play the jump animation.
				animationScript.ResetAnimatorTrigger("ChargeJump");
				animationScript.SetAnimatorTrigger("CancelJump");
			}
		}
	}

	/// <summary>
	/// Draws the vector line.
	/// </summary>
	/// <param name="start">Start.</param>
	/// <param name="end">End.</param>
	/// <param name="time">Time. NOTE: Currently does nothing!</param>
	public void DrawVectorLine (Vector3 start, Vector3 end, float time) {
		// Find the line renderer component on the player
		//LineRenderer lr = GetComponent<LineRenderer>(); 
		// Calculate the line direction
		//Vector3 lineDir = mouseStartPosition - mouseCurrentPosition;

		// The line renderer needs to be drawn on the player layer
		//playerLineRenderer.sortingLayerName = "player";

		float maxLineLenght = (maxJumpForce/jumpMultiplier);

		Vector3 lineStart = transform.position;
		Vector3 lineDir = new Vector3(start.x - end.x, start.y - end.y, -1); //start - end;

		Vector3 lineDirClamped = Vector3.ClampMagnitude(lineDir, maxLineLenght);
		Vector3 lineEnd = lineStart + lineDirClamped;

		// Set the position for the line
		playerLineRenderer.SetPosition(0, lineStart);
		playerLineRenderer.SetPosition(1, lineEnd);

		playerLineRenderer.enabled = true;

		// Change the player heading to match that of the line renderer's end point.
		ChangePlayerHeading(lineEnd);
	}



	void OnCollisionEnter2D (Collision2D col) {
		if (col.gameObject.tag == "Ground") {

			// Stop the player movement
			playerRigidbody.velocity = Vector3.zero;

			// Play thump sound
			soundScript.PlaySoundAtPosition(soundScript.thumpSound, col.transform.position);
			// Emit some particles
			EmitSmokeParticles();

			if (!animationScript.CurrentAnimIsName("idle")) // playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("idle") == false)
			{
				animationScript.SetAnimatorTrigger("Land");
			}


		} else {
			// All other collisions
			// Emit the smoke particles at hit point instead of the feet.
			EmitSmokeParticlesAtLocation(col.contacts[0].point);

		}
	}
	
	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.tag == "GravityArea") {
			// Entered a gravity area on a planet
			//Debug.Log("Entered gravity area");
			if (state != PlayerState.Flying)
				state = PlayerState.Flying;
		}
	}

	void OnTriggerExit2D (Collider2D other) {
		if (other.gameObject.tag == "GravityArea") {
			// Left a gravity area, in other words entered space
			//Debug.Log ("Left gravity area");
			if (state != PlayerState.InSpace)
				state = PlayerState.InSpace;
		}
	}

	public PlayerState CheckPlayerState()
	{
		return state;
	}


	public void ChangePlayerState (PlayerState newState)
	{
		state = newState;
	}

	/// <summary>
	/// Checks for player grounded status. Sets the player state to grounded if the overlap circle hits a ground collider.
	/// </summary>
	void CheckForGrounded() {
		// Check if the player is grounded
		grounded = Physics2D.OverlapCircle(groundCheck.transform.position, 0.1f, groundedMask);
		if (grounded) ChangePlayerState(PlayerState.Grounded);
		// If the player isn't grounded, set kinematic body to false 
		if (!grounded) {
			if (state == PlayerState.Grounded)
				ChangePlayerState(PlayerState.Flying);
		} 
	}


	public void ResetPlayerPosition() {
		playerRigidbody.velocity = Vector2.zero;
		playerRigidbody.isKinematic = false;
		transform.position = new Vector2(0, 0);
	}

	public void EmitSmokeParticlesAtLocation(Vector2 location) {
		// Save old location of smoke particles gameobject
		Vector2 oldLoc = smokeParticles.transform.position;
		// Move the smoke particles to the wanted location
		smokeParticles.transform.position = location;
		EmitSmokeParticles();
		smokeParticles.transform.position = oldLoc;
	}

	public void EmitSmokeParticles() {
		smokeParticles.Emit(10);
	}

	/// <summary>
	/// Changes the player rigidbody kinematic state.
	/// </summary>
	/// <param name="enabled">If set to <c>true</c> enabled.</param>
	public void KinematicState (bool enabled) {
		if (enabled) playerRigidbody.isKinematic = true;
		else playerRigidbody.isKinematic = false;
	}

	/// <summary>
	/// Kills the player with given state.
	/// The state is used to determine the end message on the ending screen.
	/// </summary>
	/// <param name="state">State.</param>
	public void KillPlayer(CauseOfDeath state) {
		// Change the player script death state to wanted state and call the end screen
		deathState = state;

		// Call GameController to end the scene
		gameControllerScript.ChangeGameState(GameState.FailedLevel);

	}

	/// <summary>
	/// Changes the player heading to face towards a point in the world on the X-scale.
	/// </summary>
	/// <param name="pointInWorld">Point in world.</param>
	public void ChangePlayerHeading (Vector3 pointInWorld) {
		/* If we are heading right and our x-scale is less than zero (facing left), change the local scale to
		 * match heading right.
		 * 
		 * If we are heading left instead, and our x-scale is more than zero (facing right), change the local scale to
		 * match heading left.
		 * 
		 * If none of the conditions are met, do nothing.
		 * */
		Vector3 localPoint = transform.InverseTransformPoint(pointInWorld);

		// Check the world direction as a local point
		if (localPoint.x > 0f) {
			headingRight = true;
		} else {
			headingRight = false;
		}

		// Get the local scale from the renderer gameobject
		Vector3 renderScale = playerRendererTransform.localScale;

		// Change the renderer's x-scale to match the heading
		if (headingRight && playerRendererTransform.localScale.x < 0) {
			playerRendererTransform.localScale = new Vector3(-renderScale.x, renderScale.y, renderScale.z);
		} else if (!headingRight && playerRendererTransform.localScale.x > 0) {
			playerRendererTransform.localScale = new Vector3(-renderScale.x, renderScale.y, renderScale.z);
		}
	}

	///////////////////////////////////////////////////////
	#region PowerUp
	/*
	 * POWER UP SCRIPT CALLS
	 * */


	public void CallJetpack(bool jetpackActivatedState, Vector2 pointOnScreen) {
		if (JetpackEquipped) {
			// Check for applicable player states before starting the jetpack
			if (state == PlayerState.Flying || state == PlayerState.InSpace) {
				//Debug.Log ("Jetpack call with values " + jetpackActivatedState + " point: " + pointOnScreen);
				if (jetpackActivatedState) {
					// We want to activate the jetpack
					jetpackScript.ActivateJetpack(true, pointOnScreen);
				} else {
					// We want to disable the jetpack
					jetpackScript.ActivateJetpack(false, pointOnScreen);
				}
			} else {
				jetpackScript.ActivateJetpack(false, pointOnScreen);
			}
		}
	}


	public void CallGrapplingHook(Vector2 pointOnScreen) {
		if (GrapplinghookEquipped) {
			// Check for applicable player states before starting the jetpack
			if (state == PlayerState.Flying || state == PlayerState.InSpace) {
				hookScript.LaunchHook(pointOnScreen);
			}
		}
	}

	#endregion
	///////////////////////////////////////////////////////

	// UI for debugging
	void OnGUI() {
		if (useDebugUI) {
			GUI.Label(new Rect(0, 0, 200, 24), "Latest jump: " + lastJump);
			GUI.Label(new Rect(0, 24, 300, 24), "Jump before latest jump: " + theJumpBeforeLastJump);
		}
	}
}
