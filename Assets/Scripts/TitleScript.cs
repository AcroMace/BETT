﻿using UnityEngine;
using System.Collections;

public class TitleScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
//		if(GUI.Button (new Rect(10, 10, 50, 50), "Hello")) {
//			Application.LoadLevel("BETT");
//		}
		if (Event.current.type == EventType.KeyDown) {
			Application.LoadLevel ("BETT");
		}
	}

}
