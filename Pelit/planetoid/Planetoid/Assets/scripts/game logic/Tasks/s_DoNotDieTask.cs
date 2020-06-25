using UnityEngine;
using System.Collections;

public class s_DoNotDieTask : MonoBehaviour
{
	private s_GameUIController gameUIScript;

	private static bool hasDied = false;

	void Start()
	{
		gameUIScript = GameObject.FindGameObjectWithTag("GameUI").GetComponent<s_GameUIController>();
	}

	void OnEnable()
	{
		s_GameController.OnStateChange += OnGameStateChange;
	}

	void OnDisable()
	{
		s_GameController.OnStateChange -= OnGameStateChange;
	}

	void OnGameStateChange (GameState newState)
	{
        if (newState == GameState.FailedLevel)
		{
			hasDied = true;
		}

		if (newState == GameState.PassedLevel && hasDied == false)
		{
			gameUIScript.SetAsCompleted(gameObject);
		}
    }
}
