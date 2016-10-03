using UnityEngine;
using System.Collections;

public class LightUp : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		Debug.Log (other.gameObject.name);
		if (other.tag == "light") {
			other.gameObject.GetComponent<Light> ().enabled = true;
		}
	}
}
