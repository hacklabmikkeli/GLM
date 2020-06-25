using UnityEngine;
using System.Collections;

public class s_RopeScript : MonoBehaviour {

	private LineRenderer lr;
	public GameObject target;

	// Use this for initialization
	void Start () {
		lr = GetComponent<LineRenderer>();
		lr.SetWidth(0.1f, 0.1f);
		lr.SetColors(Color.black, Color.black);
	}
	
	// Update is called once per frame
	void Update () {

		lr.SetPosition(0, transform.position);
		lr.SetPosition(1, target.transform.position);
	}
}
