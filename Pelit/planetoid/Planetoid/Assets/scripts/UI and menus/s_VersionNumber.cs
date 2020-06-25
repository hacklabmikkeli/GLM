using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class s_VersionNumber : MonoBehaviour {

	private Text versionNumber;

	// Use this for initialization
	void Start () {
		versionNumber = transform.GetComponent<Text> ();
		versionNumber.text = "WIP Alpha v" + Application.version;
	}
}