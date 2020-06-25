using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class s_GameDebugMenu : MonoBehaviour {

	public Slider gravitySlider;
	public Text gravitySliderText;
	public Slider distanceMultiplierSlider;
	public Text distanceMultText;

	public Camera mainCamera;
	public Slider cameraSlider;

	// Use this for initialization
	void Start () {
		mainCamera = Camera.main;
		//gravitySliderText.text = "" + gravitySlider.value;
		//distanceMultText.text = "" + distanceMultiplierSlider.value;

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void UpdateGravity () {
		s_GameState.gameStateScript.planetGravity = -gravitySlider.value;
		gravitySliderText.text = "" + gravitySlider.value;
	}

	public void UpdateDistanceMultiplier () {
		s_GameState.gameStateScript.planetDistanceMultiplier = distanceMultiplierSlider.value;
		distanceMultText.text = "" + distanceMultiplierSlider.value;
	}

	public void SetCameraZoom() {
		mainCamera.orthographicSize = cameraSlider.value;
	}
}
