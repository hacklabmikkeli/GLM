using UnityEngine;
using System.Collections;

public class s_FinishLevelTask : MonoBehaviour
{
	s_GameUIController gameUIScript;

	void Start()
	{
		gameUIScript = GameObject.FindGameObjectWithTag("GameUI").GetComponent<s_GameUIController>();
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
		if (newState == GameState.PassedLevel)
		{
			gameUIScript.SetAsCompleted(gameObject);
		}
	}
}
