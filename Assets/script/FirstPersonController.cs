using UnityEngine;
using System.Collections;

public class FirstPersonController : MonoBehaviour {
	public Camera camera;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton("Horizontal")) {
			transform.position = new Vector3 (transform.position.x + Input.GetAxis("Horizontal"),
							transform.position.y, transform.position.z);
		}
		if (Input.GetButton("Vertical")) {
			transform.position = new Vector3 (transform.position.x,
				transform.position.y, transform.position.z + Input.GetAxis("Vertical"));
		}
	}
}
