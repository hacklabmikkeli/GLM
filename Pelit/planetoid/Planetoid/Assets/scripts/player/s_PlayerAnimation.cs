using UnityEngine;
using System.Collections;

public class s_PlayerAnimation : MonoBehaviour {

	// All possible animation states for the player
	public enum PlayerAnimationState {idle, jump, land, flying, lowOxygen, lowOxygenLoop};

	private PlayerAnimationState currentState;	// The current animation state we are in

	public Transform playerParent;		// The player parent transform
	public Transform playerRenderer;	// The renderer gameobject childed to the player
	private Animator anim;				// The animator on the renderer

	private s_PlayerControls playerControls;

	// Use this for initialization
	void Start () {
		playerParent = transform.parent.transform;
		//Debug.Log(playerParent);

		playerRenderer = gameObject.transform;
		anim = GetComponent<Animator>();

		playerControls = playerParent.GetComponent<s_PlayerControls>();
	}


	/// <summary>
	/// Changes the current animation state and makes the player animator act accordingly.
	/// </summary>
	/// <param name="state">New state.</param>
	public void ChangeAnimationState(PlayerAnimationState state) {
		currentState = state;
		switch(currentState) {
		case PlayerAnimationState.idle:
			SetAnimatorTrigger("Idle");
			break;
		case PlayerAnimationState.jump:
			SetAnimatorTrigger("Jump");
			break;
		case PlayerAnimationState.flying:
			break;
		case PlayerAnimationState.lowOxygen:
			break;
		case PlayerAnimationState.lowOxygenLoop:
			break;
		default:
			break;
		}

	}

	public PlayerAnimationState GetCurrentState() {
		return currentState;
	}

	/// <summary>
	/// Returns true if the current animation playing has the parameter name.
	/// </summary>
	/// <returns><c>true</c>, if the current animation name matches the parameter, <c>false</c> otherwise.</returns>
	/// <param name="name">Name.</param>
	public bool CurrentAnimIsName(string name) {
		return anim.GetCurrentAnimatorStateInfo(0).IsName(name);
	}

	/// <summary>
	/// Sets the animator trigger to the given parameter.
	/// Does NOT check for trigger name correctness.
	/// </summary>
	/// <param name="triggerName">Trigger name.</param>
	public void SetAnimatorTrigger(string triggerName) {
		anim.SetTrigger(triggerName);
	}

	/// <summary>
	/// Resets the animator trigger given as parameter.
	/// Does NOT check for trigger name correctness.
	/// </summary>
	/// <param name="name">Trigger name.</param>
	public void ResetAnimatorTrigger(string name) {
		anim.ResetTrigger(name);
	}

	public void ResetAnimatorTriggers()
	{
		anim.ResetTrigger("Jump");
		anim.ResetTrigger("Land");
		anim.ResetTrigger("ChargeJump");
	}

	public void CheckGroundedForAnimator()
	{
		if (playerControls.CheckPlayerState() == PlayerState.Grounded)
		{
			SetAnimatorTrigger("Land");
		}
	}
}
