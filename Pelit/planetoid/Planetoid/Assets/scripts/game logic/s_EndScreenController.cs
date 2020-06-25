using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class s_EndScreenController : MonoBehaviour
{
	private s_GameController gameControllerScript;
	private s_GameUIController gameUIScript;

	public Text endScreenText;
	public Button nextLevelButton;

	void Start(){
		this.gameControllerScript = GameObject.FindGameObjectWithTag("GameController").GetComponent<s_GameController>();
		gameUIScript = GameObject.FindGameObjectWithTag("GameUI").GetComponent<s_GameUIController>();
		SetupEndScreen();
	}

	void SetupEndScreen()
	{
		if (gameControllerScript.CurrentGameState() == GameState.PassedLevel) {
			nextLevelButton.gameObject.SetActive(true);
			gameUIScript.ChangeButtonState();
			endScreenText.text = "Level Cleared!";
		}
		else if (gameControllerScript.CurrentGameState() == GameState.FailedLevel) {

			// Find the state of death from the player script
			s_PlayerControls playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<s_PlayerControls>();
			CauseOfDeath currentState = playerScript.deathState;

			switch(currentState) {
			case CauseOfDeath.Boundary:
				// Death by map boundaries
				endScreenText.text = "Lost in space, forever and ever";
				break;
			case CauseOfDeath.Burn:
				// Death by burning planet
				endScreenText.text = "You're toast!";
				break;
			case CauseOfDeath.Oxygen:
				// Death by lack of oxygen
				endScreenText.text = "You're out of breath!";
				break;
			case CauseOfDeath.Projectile:
				// Death by a projectile
				endScreenText.text = "Boom headshot!";
				break;
			default:
				// This shouldn't happen, but if it does, show default death text
				endScreenText.text = "You're spacejunk!";
				break;
			}

			nextLevelButton.gameObject.SetActive(false);
			gameUIScript.ChangeButtonState();

		}
		else if (gameControllerScript.CurrentGameState() == GameState.PausedLevel) {
			nextLevelButton.gameObject.SetActive(false);
			endScreenText.text = "Paused!";
		}
	}

	public void MenuButtonPressed()
	{
		this.gameControllerScript.LoadMainMenu();
	}

	public void RestartButtonPressed()
	{
		this.gameControllerScript.ResetLevel();
	}

	public void NextLevelButtonPressed()
	{
		this.gameControllerScript.LoadNextLevel();
	}
}
