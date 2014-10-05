using UnityEngine;
using System.Collections;

public class TitleScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
//		if (Event.current.type == EventType.KeyDown) {
//			Application.LoadLevel ("BETT");
//		}
		if (Input.GetButtonDown ("Confirm")) {
			Application.LoadLevel ("BETT");
		}
	}

}
