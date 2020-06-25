using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// S_ player character skin.
/// Sets the player skin to the values matching those foudn in PlayerData.
/// </summary>
public class s_PlayerCharacterSkin : MonoBehaviour {
	// All of the possible skins in some sort of list or array
	public List<string> skinNames;		// The names of all the files inside the playerSkins folder
	public List<Sprite[]> skinSprites;	// All of the loaded multi-sprites in a list
	private Sprite[] _loadedSprite;		// The wanted multi-sprite parts in a list

	// The path inside the Resources folder
	private string skinResourcePath = "PlayerSkins/";


	public Transform body;
	public Transform head;
	public Transform LShoulder;
	public Transform LArm;
	public Transform RShoulder;
	public Transform RArm;
	public Transform RLeg;
	public Transform LLeg;
	public Transform antennaBottom;
	public Transform antennaTop;
	public Transform backPack;

	private SpriteRenderer bodyRenderer;
	private SpriteRenderer headRenderer;
	private SpriteRenderer LShoulderRenderer;
	private SpriteRenderer LArmRenderer;
	private SpriteRenderer RShoulderRenderer;
	private SpriteRenderer RArmRenderer;
	private SpriteRenderer RLegRenderer;
	private SpriteRenderer LLegRenderer;
	private SpriteRenderer antennaBottomRenderer;
	private SpriteRenderer antennaTopRenderer;
	private SpriteRenderer backPackRenderer;

	private int _currentIndex = 0;
	private int _helmetIndex = 0;
	private int _bodyIndex = 0;
	private int _legsIndex = 0;



	// Use this for initialization
	void Start () {
		// Get all sprite renderers
		bodyRenderer = body.GetComponent<SpriteRenderer>();
		headRenderer = head.GetComponent<SpriteRenderer>();
		LShoulderRenderer = LShoulder.GetComponent<SpriteRenderer>();
		LArmRenderer = LArm.GetComponent<SpriteRenderer>();
		RShoulderRenderer = RShoulder.GetComponent<SpriteRenderer>();
		RArmRenderer = RArm.GetComponent<SpriteRenderer>();
		RLegRenderer = RLeg.GetComponent<SpriteRenderer>();
		LLegRenderer = LLeg.GetComponent<SpriteRenderer>();
		antennaBottomRenderer = antennaBottom.GetComponent<SpriteRenderer>();
		antennaTopRenderer = antennaTop.GetComponent<SpriteRenderer>();
		backPackRenderer = backPack.GetComponent<SpriteRenderer>();

		// Add all possible skins to a list
		skinNames = new List<string>();
		skinNames.Add("skin_red");
		skinNames.Add("skin_blue");
		skinNames.Add("skin_green");
		skinNames.Add("skin_yellow");
		skinNames.Add("skin_purple");
		skinNames.Add("skin_turqoise");
		skinNames.Add("skin_premium_snowfox");
		skinNames.Add("skin_premium_knight");

		// Load all of the sprites into another list for easier access
		skinSprites = new List<Sprite[]>();
		for (int i = 0; i < skinNames.Count; i++) {
			skinSprites.Add(Resources.LoadAll<Sprite>(skinResourcePath + skinNames[i]));
		}
		
		//LoadWholeSkin(0);
		//LoadWholeSkin(PlayerData.current.equippedSkin);

		ChangeHelmet(PlayerData.current.equippedHead);
		ChangeBody(PlayerData.current.equippedBody);
		ChangeLegs(PlayerData.current.equippedLegs);

	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.H)) {
			if (_helmetIndex + 1 >= skinNames.Count) _helmetIndex = 0;
			else _helmetIndex++;

			ChangeHelmet(_helmetIndex);
		}

		if (Input.GetKeyDown(KeyCode.B)) {
			if (_bodyIndex + 1 >= skinNames.Count) _bodyIndex = 0;
			else _bodyIndex++;
			
			ChangeBody(_bodyIndex);
		}

		if (Input.GetKeyDown(KeyCode.L)) {
			if (_legsIndex + 1 >= skinNames.Count) _legsIndex = 0;
			else _legsIndex++;
			
			ChangeLegs(_legsIndex);
		}

/*		if (Input.GetKeyDown(KeyCode.RightArrow)) {
			if (currentIndex+1 >= skinNames.Count) currentIndex = 0;
			else currentIndex++;

			string nameToLoad = skinNames[currentIndex];

			Debug.Log(skinNames[currentIndex]);
			ChangeSkin(nameToLoad);
		}*/
	}

	public void ChangeHelmet(int index) {
		// Load the wanted skin into memory
		_loadedSprite = skinSprites[index];
		List<string> parts = LoadNamesIntoList(_loadedSprite);
		// Change the wanted parts
		headRenderer.sprite = _loadedSprite[parts.IndexOf("head")];
	}

	public void ChangeBody(int index) {
		// Load the wanted skin into memory
		_loadedSprite = skinSprites[index];
		List<string> parts = LoadNamesIntoList(_loadedSprite);

		// Change the wanted parts
		bodyRenderer.sprite = _loadedSprite[parts.IndexOf("body")];
		LArmRenderer.sprite = _loadedSprite[parts.IndexOf("larm")];
		RArmRenderer.sprite = _loadedSprite[parts.IndexOf("rarm")];
		RShoulderRenderer.sprite = _loadedSprite[parts.IndexOf("rshoulder")];
		LShoulderRenderer.sprite = _loadedSprite[parts.IndexOf("lshoulder")];
		backPackRenderer.sprite = _loadedSprite[parts.IndexOf("backpack")];
		antennaTopRenderer.sprite = _loadedSprite[parts.IndexOf("antenna_top")];
		antennaBottomRenderer.sprite = _loadedSprite[parts.IndexOf("antenna_bottom")];
	}

	public void ChangeLegs(int index) {
		// Load the wanted skin into memory
		_loadedSprite = skinSprites[index];
		List<string> parts = LoadNamesIntoList(_loadedSprite);
		// Change the wanted parts
		RLegRenderer.sprite = _loadedSprite[parts.IndexOf("rleg")];
		LLegRenderer.sprite = _loadedSprite[parts.IndexOf("lleg")];

	}

	public void LoadWholeSkin(int index) {
		//Debug.Log(index);

		ChangeHelmet(index);
		ChangeBody(index);
		ChangeLegs(index);
	}


	List<string> LoadNamesIntoList(Sprite[] multiSprite) {
		List<string> parts = new List<string>();
		// Load the parts into a list and return it
		for (int i = 0; i < multiSprite.Length; i++) {
			parts.Add(multiSprite[i].name.ToLower());	// Add all the parts into a list in lower case
		}

		return parts;
	}
}
