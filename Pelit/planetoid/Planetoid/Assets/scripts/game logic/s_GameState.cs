using UnityEngine;
using System.Collections;

public class s_GameState : MonoBehaviour {

	public bool valuesHaveBeenChanged = false;

	public float planetGravity = -500;
	public float planetDistanceMultiplier = 100;

	public static s_GameState gameStateScript ;

	// Use this for initialization
	void Start () {
		gameStateScript = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Call this after every change in the debug UI
	void ValuesChanged() {

	}
}
