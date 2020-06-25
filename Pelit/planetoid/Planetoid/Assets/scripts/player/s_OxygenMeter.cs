using UnityEngine;
using System.Collections;

public class s_OxygenMeter : MonoBehaviour {

	public float maxOxygenAmount = 100;
	public float currentOxygenAmount;
	public float lowOxygenAmount;

	public float restoreMultiplier = 10;
	public float lowerMultiplier = 4;

	public bool runningOutOfOxygenFailsLevel = true;

	public GameObject oxygenBubble;
	public Color oxygenBubbleColor;
	public SpriteRenderer obSpriteRenderer;

	private s_PlayerAnimation animationScript;

	// Use this for initialization
	void Start () {
		obSpriteRenderer = oxygenBubble.GetComponent<SpriteRenderer>();
		currentOxygenAmount = maxOxygenAmount;

		animationScript = GetComponent<s_PlayerControls>().animationScript;	// Get the animation script from playercontrols
		//Debug.Log(animationScript);
	}

	/// <summary>
	/// Changes the oxygen level.
	/// </summary>
	/// <param name="amountPerTimeStep">Amount per time step. This needs to be negative or positive to have any impact.</param>
	public void ChangeOxygenLevel (float amountPerTimeStep) {
		//Debug.Log("Changing oxygen level by: " + amount);
		// Set the multiplier so the player gets oxygen back faster than loses it
		float multiplier = 1;
		if (amountPerTimeStep < 0) {
			// We are losing oxygen
			multiplier = 2;
		} else {
			multiplier = 10;
		}

		float amount = Time.deltaTime * amountPerTimeStep * multiplier;


		if (currentOxygenAmount <= 0) {
			// TODO: Player loses level through an animation
			if (runningOutOfOxygenFailsLevel && s_GameController.gameControllerSingleton.currentGameState != GameState.FailedLevel) {
				currentOxygenAmount = maxOxygenAmount;
				s_PlayerControls playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<s_PlayerControls>();
				playerScript.KillPlayer(CauseOfDeath.Oxygen);
			}
		} else if (currentOxygenAmount <= maxOxygenAmount) {
			currentOxygenAmount += amount;
			ChangeAnimation();
		} else if (currentOxygenAmount > maxOxygenAmount) {
			currentOxygenAmount = maxOxygenAmount;
		}

		//Debug.Log ("Lowering B and G by " + amount);
		ChangeBubbleAlphaColor(amount);

		ChangeBubbleSize();

	}

	/// <summary>
	/// Changes the animation when oxygen level is low or when it returns back to normal levels.
	/// </summary>
	void ChangeAnimation()
	{
		if (currentOxygenAmount < lowOxygenAmount && animationScript.CurrentAnimIsName("flying"))
		{
			animationScript.SetAnimatorTrigger("LowOxygen");
		}
		else if (currentOxygenAmount > lowOxygenAmount && animationScript.CurrentAnimIsName("LowOxygen2"))
		{
			animationScript.SetAnimatorTrigger("NormalOxygen");
		}
	}
	

	/// <summary>
	/// Changes the Alpha color of the Oxygen bubble.
	/// </summary>
	/// <param name="amount">Amount to be changed. Max alpha is 255, min is 0.</param>
	public void ChangeBubbleAlphaColor (float amount) {
		amount = currentOxygenAmount / 100;

		// Get the old color
		Color oldColor = obSpriteRenderer.color;
		// Calculate the new color
		Color newColor = new Color(oldColor.r, amount, amount, 1);

		//Debug.Log ("Current Color: " + obSpriteRenderer.color);
		oxygenBubbleColor = newColor;
		obSpriteRenderer.color = newColor;

	}

	/// <summary>
	/// Changes the size of the bubble.
	/// </summary>
	public void ChangeBubbleSize()
	{

		if (currentOxygenAmount <= maxOxygenAmount)
		{
			float amount = currentOxygenAmount / 100;
			oxygenBubble.transform.localScale = new Vector3(amount, amount);
		}
		else if (currentOxygenAmount > maxOxygenAmount)
		{
			oxygenBubble.transform.localScale = Vector3.one;
		}

	}
}
