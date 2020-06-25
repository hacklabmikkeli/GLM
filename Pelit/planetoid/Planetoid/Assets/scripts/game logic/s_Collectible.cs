using UnityEngine;
using System.Collections;

public class s_Collectible : MonoBehaviour {

	public int CollectibleAmount;

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Player") {
			print ("Collectible collected!");
			other.gameObject.SetActive (false);
			CollectibleAmount--;

		}
	}

}
