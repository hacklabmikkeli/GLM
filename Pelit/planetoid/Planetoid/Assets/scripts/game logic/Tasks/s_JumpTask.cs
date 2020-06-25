using UnityEngine;
using System.Collections;

public class s_JumpTask : MonoBehaviour
{
	//This could be used to raise and event every time gamestate is changed.
	public delegate void JumpTaskEvent(GameObject taskTitle);
	public static event JumpTaskEvent OnTaskComplete;

	public int jumpCount;
	public int maxJumpCount;

	void OnEnable()
	{
		s_PlayerControls.OnEventBegan += CountJumps;
		s_GameController.OnStateChange += OnStateChange;
	}

	void OnDisable()
	{
		s_PlayerControls.OnEventBegan -= CountJumps;
		s_GameController.OnStateChange -= OnStateChange;
	}

	void CountJumps()
	{
		jumpCount++;
	}

	void OnStateChange (GameState newState)
	{
		if (newState == GameState.PassedLevel)
		{
			if (jumpCount < maxJumpCount)
			{
				OnTaskComplete(gameObject);
			}
		}
	}
}
