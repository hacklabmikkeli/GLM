using UnityEngine;
using System.Collections;

/// <summary>
/// This script is used for controlling the camera transitions
/// between main menu, shop and level select screens.
/// </summary>

public class s_MenuCameraController : MonoBehaviour
{
	public delegate void MoveEvent();
	public static event MoveEvent OnMoveToLevels;
	public static event MoveEvent OnMoveToMainMenu;

	public GameObject mainCanvas;
	public GameObject shopCanvas;
	public Canvas levelCanvas;

	/// <summary>
	/// This is called when player taps the "Levels"-button in the main menu.
	/// It starts the camera transition from main menu to level select screen.
	/// </summary>
	void MovedToLevels()
	{
		if (OnMoveToLevels != null)
		{
			OnMoveToLevels();
		}

		levelCanvas.enabled = true;
	}

	/// <summary>
	/// This is called when player taps the back-button in the main menu.
	/// It starts the camera transition from level select to main menu screen.
	/// </summary>
	void MovedToMainMenu()
	{
		if (OnMoveToMainMenu != null)
		{
			OnMoveToMainMenu();
		}
	}

	/// <summary>
	/// Shows the show menu canvas.
	/// </summary>
	void OpenShopMenu()
	{
		shopCanvas.SetActive (true);
	}

	/// <summary>
	/// Shows the main menu canvas.
	/// </summary>
	void OpenMainMenu()
	{
		mainCanvas.SetActive(true);
	}
}