using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class s_FPS : MonoBehaviour {

	float deltaTime = 0.0f;
	public Text FPSCounter;
	
	// Update is called once per frame
	void Update () {
		deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
		float msec = deltaTime * 1000.0f;
		float fps = 1.0f / deltaTime;
		FPSCounter.text = string.Format ("{0:0.0} ms ({1:0.} fps)", msec, fps);
	}


}
