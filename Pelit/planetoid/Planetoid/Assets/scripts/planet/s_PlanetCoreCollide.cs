using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Planet core collide script
 * Planets can have different types that take different actions when player touches them
 * 
 * PLANET TYPES:
 * Normal = Basic planet that the player character can jump to.
 * Destructive = Planet that self-destructs after few seconds when player character touches it.
 * Deadly = Planet that kills the player character instantly when he/she touches it.
 */
public enum PlanetType {Normal, Destructive, Shooting, Deadly, DeadlyNShooting, Asteroid};


public class s_PlanetCoreCollide : MonoBehaviour
{
    /// <summary>
    /// Core collision event. Should 
    /// occure when player touches a planet.
    /// Registered scripts:
    /// - s_CountingTask.cs
    /// - s_explotionScript.cs
    /// </summary>
    public delegate void PlanetCoreEvent(PlanetType planetType);
    public static event PlanetCoreEvent OnEventBegan;

	/// <summary>
	/// The type of the planet.
	/// Normal means a planet that the player can land on.
	/// Destructive means 
	/// </summary>
	public PlanetType planetType;

	public bool hasPlayerVisited = false;

	/// <summary>
	/// Called when player touches down on the planet. Handles the actions of different types of planets.
	/// </summary>
	/// <param name="col">The collider that collided with the planet.</param>
	void StartPlanetEffects(Collision2D col)
	{
		s_PlayerControls playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<s_PlayerControls>();

        switch (planetType)
        {
		case PlanetType.Normal:
			// Set this planet as the player's parent
			ParentPlayer(col.transform, this.transform);

			if (col.gameObject.tag == "Player" && hasPlayerVisited == false && OnEventBegan != null)
			{
                OnEventBegan(PlanetType.Normal);
				hasPlayerVisited = true;
			}
            break;
        case PlanetType.Deadly:
			if (col.gameObject.tag == "Player")
			{
				s_SoundScript.audioSource.PlaySoundAtPosition(s_SoundScript.audioSource.burnSound, col.transform.position);
				playerScript.KillPlayer(CauseOfDeath.Burn);
			}
			break;
		case PlanetType.DeadlyNShooting:
			if (col.gameObject.tag == "Player")
			{
				playerScript.KillPlayer(CauseOfDeath.Burn);
			}
			else if(col.gameObject.tag == "Meteor")
			{
				Destroy(col.gameObject);
			}
			break;
		case PlanetType.Destructive:
			s_SoundScript.audioSource.PlaySoundAtPosition(s_SoundScript.audioSource.breakingPlanet, transform.position);
            OnEventBegan(PlanetType.Destructive);
			break;
		default:
			break;
		}
	}

    /// <summary>
    /// Raises the collision enter2d event.
    /// </summary>
    /// <param name="col">col</param>
	void OnCollisionEnter2D(Collision2D col)
    {
		if (col.gameObject.tag == "Player" || col.gameObject.tag == "Meteor")
        {
			StartPlanetEffects(col);
		}
	}

    /// <summary>
    /// Raises the collision exit2d event.
    /// </summary>
    /// <param name="col">col</param>
	void OnCollisionExit2D (Collision2D col)
    {
		// Player is leaving the planet ground, resume gravity attraction on the gravity field
		if (col.gameObject.tag == "Player")
        {
			ParentPlayer(col.transform, null);
		}
	}

	/// <summary>
	/// Parents the player.
	/// </summary>
	/// <param name="player">Player transform.</param>
	/// <param name="parent">The transform which will act as a parent.</param>
	void ParentPlayer(Transform player, Transform parent)
    {
		player.SetParent(parent);
	}
}
