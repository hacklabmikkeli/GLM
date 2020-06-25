using UnityEngine;
using System.Collections;

/// <summary>
/// S_ player power up stats.
/// Handles all power up information for the player, for example jetpack length and power.
/// </summary>

public enum ActivePowerUp {Jetpack, GrapplingHook, Teleport};
public enum PassivePowerUp {JumpBoots, GravityKiller, ExtraOxygenTank};

public class s_PlayerPowerUpStats : MonoBehaviour {

	////////////////////// ACTIVE POWERUPS //////////////////////
	[Header("Active powerups")]
	// JETPACK
	/*	Power, which affects the push force of the jetpack.
	 * 	Duration, which affects the duration of the jetpack per level. Think of this like a fuel amount, which lasts for X seconds.
	 * */
	private float _baseJetpackPower = 3f;		// the vector multiplier is 3
	private float _baseJetpackDuration = 2f;	// 2 seconds
	private float _maxJetpackPower = 10f;
	private float _maxJetpackDuration = 5f;

	public float currentJetpackPower = 3f;
	public float currentJetpackDuration = 2f;

	// GRAPPLING HOOK
	/*	Range, the distance in unity units in which the ghook can be shot.
	 *  Uses, the grappling hook has a certain amount of uses per level.
	 * */
	private float _baseGhookRange = 5f;
	private int _baseGhookUses = 5;
	private float _maxGhookRange = 20f;
	private int _maxGhookUses = 20;

	public float currentGhookRange = 5f;
	public int currentGhookUses = 5;

	// TELEPORT
	/* Range, the distance in unity units from the player where the teleport is active
	 * Uses, the teleport has a certain amount of uses per level
	 * */
	private float _baseTeleportRange;
	private int _baseTeleportUses;
	private float _maxTeleportRange;
	private int _maxTeleportUses;

	public float currentTeleportRange;
	public float currentTeleportUses;

	// GRAVITY BALLS
	////////////////////// ACTIVE POWERUPS //////////////////////

	////////////////////// PASSIVE POWERUPS //////////////////////
	[Header("Passive powerups")]
	// JUMP BOOTS
	/* Power, jump boots have a power that increases the jump power of the player
	 * */
	private float _baseJumpBootsPower;
	private float _maxJumpBootsPower;

	public float currentJumpBootsPower;

	// OXYGEN TANK
	/* The amount of oxygen available to the player can be upgraded
	 * */
	private float _baseOxygenAmount = 100;
	private float _maxOxygenAmount = 200;
	
	public float currentOxygenAmount = 100;

	////////////////////// PASSIVE POWERUPS //////////////////////


	public void UpdateActivePowerUp (ActivePowerUp p) {

	}

	public void UpdatePassivePowerUp (PassivePowerUp p) {

	}
	
	public void CallSaveFunction() {

	}
}
